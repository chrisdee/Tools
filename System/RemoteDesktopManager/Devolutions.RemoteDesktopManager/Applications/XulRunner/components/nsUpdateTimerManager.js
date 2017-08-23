/* -*- Mode: C++; tab-width: 8; indent-tabs-mode: nil; c-basic-offset: 2 -*- */
/*
//@line 39 "e:\builds\moz2_slave\rel-m-192-xr-w32-bld\build\toolkit\mozapps\update\src\nsUpdateTimerManager.js"
*/
Components.utils.import("resource://gre/modules/XPCOMUtils.jsm");

const Cc = Components.classes;
const Ci = Components.interfaces;

const PREF_APP_UPDATE_LASTUPDATETIME_FMT  = "app.update.lastUpdateTime.%ID%";
const PREF_APP_UPDATE_TIMER               = "app.update.timer";
const PREF_APP_UPDATE_LOG                 = "app.update.log";

const CATEGORY_UPDATE_TIMER               = "update-timer";

XPCOMUtils.defineLazyServiceGetter(this, "gPref",
                                   "@mozilla.org/preferences-service;1",
                                   "nsIPrefBranch2");

XPCOMUtils.defineLazyServiceGetter(this, "gConsole",
                                   "@mozilla.org/consoleservice;1",
                                   "nsIConsoleService");

XPCOMUtils.defineLazyGetter(this, "gLogEnabled", function tm_gLogEnabled() {
  return getPref("getBoolPref", PREF_APP_UPDATE_LOG, false);
});

function getObserverService() {
  return Cc["@mozilla.org/observer-service;1"].getService(Ci.nsIObserverService);
}

/**
//@line 78 "e:\builds\moz2_slave\rel-m-192-xr-w32-bld\build\toolkit\mozapps\update\src\nsUpdateTimerManager.js"
 */
function getPref(func, preference, defaultValue) {
  try {
    return gPref[func](preference);
  }
  catch (e) {
  }
  return defaultValue;
}

/**
//@line 92 "e:\builds\moz2_slave\rel-m-192-xr-w32-bld\build\toolkit\mozapps\update\src\nsUpdateTimerManager.js"
 */
function LOG(string) {
  if (gLogEnabled) {
    dump("*** UTM:SVC " + string + "\n");
    gConsole.logStringMessage("UTM:SVC " + string);
  }
}

/**
//@line 104 "e:\builds\moz2_slave\rel-m-192-xr-w32-bld\build\toolkit\mozapps\update\src\nsUpdateTimerManager.js"
 */
function TimerManager() {
  getObserverService().addObserver(this, "xpcom-shutdown", false);
}
TimerManager.prototype = {
  /**
   * The Checker Timer
   */
  _timer: null,

  /**
//@line 117 "e:\builds\moz2_slave\rel-m-192-xr-w32-bld\build\toolkit\mozapps\update\src\nsUpdateTimerManager.js"
   */
   _timerInterval: null,

  /**
   * The set of registered timers.
   */
  _timers: { },

  /**
//@line 132 "e:\builds\moz2_slave\rel-m-192-xr-w32-bld\build\toolkit\mozapps\update\src\nsUpdateTimerManager.js"
   */
  get _fudge() {
    return Math.round(Math.random() * this._timerInterval / 1000);
  },

  /**
   * See nsIObserver.idl
   */
  observe: function TM_observe(aSubject, aTopic, aData) {
    switch (aTopic) {
    case "profile-after-change":
      this._start();
      break;
    case "xpcom-shutdown":
      let os = getObserverService();
      os.removeObserver(this, "xpcom-shutdown");

      // Release everything we hold onto.
      if (this._timer) {
        this._timer.cancel();
        this._timer = null;
      }
      for (var timerID in this._timers)
        delete this._timers[timerID];
      this._timers = null;
      break;
    }
  },

  _start: function TM__start() {
    this._timerInterval = getPref("getIntPref", "app.update.timer", 600000);
    this._timer = Cc["@mozilla.org/timer;1"].createInstance(Ci.nsITimer);
    this._timer.initWithCallback(this, this._timerInterval,
                                 Ci.nsITimer.TYPE_REPEATING_SLACK);
  },
  /**
//@line 171 "e:\builds\moz2_slave\rel-m-192-xr-w32-bld\build\toolkit\mozapps\update\src\nsUpdateTimerManager.js"
   */
  notify: function TM_notify(timer) {
    var prefLastUpdate;
    var lastUpdateTime;
    var now = Math.round(Date.now() / 1000);
    var catMan = Cc["@mozilla.org/categorymanager;1"].
                 getService(Ci.nsICategoryManager);
    var entries = catMan.enumerateCategory(CATEGORY_UPDATE_TIMER);
    while (entries.hasMoreElements()) {
      let entry = entries.getNext().QueryInterface(Ci.nsISupportsCString).data;
      let value = catMan.getCategoryEntry(CATEGORY_UPDATE_TIMER, entry);
      let [cid, method, timerID, prefInterval, defaultInterval] = value.split(",");
      defaultInterval = parseInt(defaultInterval);
      // cid and method are validated below when calling notify.
      if (!timerID || !defaultInterval || isNaN(defaultInterval)) {
        LOG("TimerManager:notify - update-timer category registered" +
            (cid ? " for " + cid : "") + " without required parameters - " +
             "skipping");
        continue;
      }

      let interval = getPref("getIntPref", prefInterval, defaultInterval);
      prefLastUpdate = PREF_APP_UPDATE_LASTUPDATETIME_FMT.replace(/%ID%/,
                                                                  timerID);
      if (gPref.prefHasUserValue(prefLastUpdate)) {
        lastUpdateTime = gPref.getIntPref(prefLastUpdate);
      }
      else {
        lastUpdateTime = now + this._fudge;
        gPref.setIntPref(prefLastUpdate, lastUpdateTime);
        continue;
      }

      if ((now - lastUpdateTime) > interval) {
        try {
          Components.classes[cid][method](Ci.nsITimerCallback).notify(timer);
          LOG("TimerManager:notify - notified " + cid);
        }
        catch (e) {
          LOG("TimerManager:notify - error notifying component id: " +
              cid + " ,error: " + e);
        }
        lastUpdateTime = now + this._fudge;
        gPref.setIntPref(prefLastUpdate, lastUpdateTime);
      }
    }

    for (var timerID in this._timers) {
      var timerData = this._timers[timerID];

      if ((now - timerData.lastUpdateTime) > timerData.interval) {
        if (timerData.callback instanceof Ci.nsITimerCallback) {
          try {
            timerData.callback.notify(timer);
            LOG("TimerManager:notify - notified timerID: " + timerID);
          }
          catch (e) {
            LOG("TimerManager:notify - error notifying timerID: " + timerID +
                ", error: " + e);
          }
        }
        else {
          LOG("TimerManager:notify - timerID: " + timerID + " doesn't " +
              "implement nsITimerCallback - skipping");
        }
        lastUpdateTime = now + this._fudge;
        timerData.lastUpdateTime = lastUpdateTime;
        prefLastUpdate = PREF_APP_UPDATE_LASTUPDATETIME_FMT.replace(/%ID%/, timerID);
        gPref.setIntPref(prefLastUpdate, lastUpdateTime);
      }
    }
  },

  /**
   * See nsIUpdateTimerManager.idl
   */
  registerTimer: function TM_registerTimer(id, callback, interval) {
    LOG("TimerManager:registerTimer - id: " + id);
    var prefLastUpdate = PREF_APP_UPDATE_LASTUPDATETIME_FMT.replace(/%ID%/, id);
    var lastUpdateTime;
    if (gPref.prefHasUserValue(prefLastUpdate)) {
      lastUpdateTime = gPref.getIntPref(prefLastUpdate);
    } else {
      lastUpdateTime = Math.round(Date.now() / 1000) + this._fudge;
      gPref.setIntPref(prefLastUpdate, lastUpdateTime);
    }
    this._timers[id] = { callback       : callback,
                         interval       : interval,
                         lastUpdateTime : lastUpdateTime };
  },

  classDescription: "Timer Manager",
  contractID: "@mozilla.org/updates/timer-manager;1",
  classID: Components.ID("{B322A5C0-A419-484E-96BA-D7182163899F}"),
  _xpcom_categories: [{ category: "profile-after-change" }],
  QueryInterface: XPCOMUtils.generateQI([Ci.nsIUpdateTimerManager,
                                         Ci.nsITimerCallback,
                                         Ci.nsIObserver])
};

function NSGetModule(compMgr, fileSpec)
  XPCOMUtils.generateModule([TimerManager]);
