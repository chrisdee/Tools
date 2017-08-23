EXPORTED_SYMBOLS = [ "BadCertHandler", "checkCert" ];

/**
 * Only allow built-in certs for HTTPS connections.  See bug 340198.
 */
function checkCert(channel) {
  if (!channel.originalURI.schemeIs("https"))  // bypass
    return;

  const Ci = Components.interfaces;  
  var cert =
      channel.securityInfo.QueryInterface(Ci.nsISSLStatusProvider).
      SSLStatus.QueryInterface(Ci.nsISSLStatus).serverCert;

  var issuer = cert.issuer;
  while (issuer && !cert.equals(issuer)) {
    cert = issuer;
    issuer = cert.issuer;
  }

  var errorstring = "cert issuer is not built-in";
  if (!issuer)
    throw errorstring;

  issuer = issuer.QueryInterface(Ci.nsIX509Cert3);
  var tokenNames = issuer.getAllTokenNames({});

  if (!tokenNames.some(isBuiltinToken))
    throw errorstring;
}

function isBuiltinToken(tokenName) {
  return tokenName == "Builtin Object Token";
}

/**
 * This class implements nsIBadCertListener.  Its job is to prevent "bad cert"
 * security dialogs from being shown to the user.  It is better to simply fail
 * if the certificate is bad. See bug 304286.
 */
function BadCertHandler() {
}
BadCertHandler.prototype = {

  // nsIChannelEventSink
  onChannelRedirect: function(oldChannel, newChannel, flags) {
    // make sure the certificate of the old channel checks out before we follow
    // a redirect from it.  See bug 340198.
    // Don't call checkCert for internal redirects. See bug 569648.
    if (!(flags & Components.interfaces.nsIChannelEventSink.REDIRECT_INTERNAL))
      checkCert(oldChannel);
  },

  // Suppress any certificate errors
  notifyCertProblem: function(socketInfo, status, targetSite) {
    return true;
  },

  // Suppress any ssl errors
  notifySSLError: function(socketInfo, error, targetSite) {
    return true;
  },

  // nsIInterfaceRequestor
  getInterface: function(iid) {
    return this.QueryInterface(iid);
  },

  // nsISupports
  QueryInterface: function(iid) {
    if (!iid.equals(Components.interfaces.nsIChannelEventSink) &&
        !iid.equals(Components.interfaces.nsIBadCertListener2) &&
        !iid.equals(Components.interfaces.nsISSLErrorListener) &&
        !iid.equals(Components.interfaces.nsIInterfaceRequestor) &&
        !iid.equals(Components.interfaces.nsISupports))
      throw Components.results.NS_ERROR_NO_INTERFACE;
    return this;
  }
};
