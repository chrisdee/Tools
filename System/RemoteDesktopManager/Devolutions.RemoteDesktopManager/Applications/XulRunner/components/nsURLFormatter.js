//@line 37 "e:\builds\moz2_slave\rel-m-192-xr-w32-bld\build\toolkit\components\urlformatter\src\nsURLFormatter.js"
/**
 * @class nsURLFormatterService
 *
 * nsURLFormatterService exposes methods to substitute variables in URL formats.
 *
 * Mozilla Applications linking to Mozilla websites are strongly encouraged to use
 * URLs of the following format:
 *
 *   http[s]://%SERVICE%.mozilla.[com|org]/%LOCALE%/
 */

const Cc = Components.classes;
const Ci = Components.interfaces;
const Cu = Components.utils;

Cu.import("resource://gre/modules/XPCOMUtils.jsm");

function nsURLFormatterService() {}
nsURLFormatterService.prototype = {
  classDescription: "Application URL Formatter Service",
  contractID: "@mozilla.org/toolkit/URLFormatterService;1",
  classID: Components.ID("{e6156350-2be8-11db-a98b-0800200c9a66}"),
  QueryInterface: XPCOMUtils.generateQI([Ci.nsIURLFormatter]),

  _defaults: {
    get appInfo() {
      if (!this._appInfo)
        this._appInfo = Cc["@mozilla.org/xre/app-info;1"].
                        getService(Ci.nsIXULAppInfo).
                        QueryInterface(Ci.nsIXULRuntime);
      return this._appInfo;
    },

    LOCALE: function() Cc["@mozilla.org/chrome/chrome-registry;1"].
                       getService(Ci.nsIXULChromeRegistry).getSelectedLocale('global'),
    VENDOR:           function() this.appInfo.vendor,
    NAME:             function() this.appInfo.name,
    ID:               function() this.appInfo.ID,
    VERSION:          function() this.appInfo.version,
    APPBUILDID:       function() this.appInfo.appBuildID,
    PLATFORMVERSION:  function() this.appInfo.platformVersion,
    PLATFORMBUILDID:  function() this.appInfo.platformBuildID,
    APP:              function() this.appInfo.name.toLowerCase().replace(/ /, ""),
    OS:               function() this.appInfo.OS,
    XPCOMABI:         function() this.appInfo.XPCOMABI
  },

  formatURL: function uf_formatURL(aFormat) {
    var _this = this;
    var replacementCallback = function(aMatch, aKey) {
      if (aKey in _this._defaults) // supported defaults
        return _this._defaults[aKey]();
      Cu.reportError("formatURL: Couldn't find value for key: " + aKey);
      return aMatch;
    }
    return aFormat.replace(/%([A-Z]+)%/g, replacementCallback);
  },

  formatURLPref: function uf_formatURLPref(aPref) {
    var format = null;
    var PS = Cc['@mozilla.org/preferences-service;1'].
             getService(Ci.nsIPrefBranch);

    try {
      format = PS.getComplexValue(aPref, Ci.nsISupportsString).data;
    } catch(ex) {
      Cu.reportError("formatURLPref: Couldn't get pref: " + aPref);
      return "about:blank";
    }

    if (!PS.prefHasUserValue(aPref) &&
        /^(data:text\/plain,.+=.+|chrome:\/\/.+\/locale\/.+\.properties)$/.test(format)) {
      // This looks as if it might be a localised preference
      try {
        format = PS.getComplexValue(aPref, Ci.nsIPrefLocalizedString).data;
      } catch(ex) {}
    }

    return this.formatURL(format);
  }
};

function NSGetModule(aCompMgr, aFileSpec)
  XPCOMUtils.generateModule([nsURLFormatterService]);
