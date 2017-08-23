    const Ci = Components.interfaces;
    const Cu = Components.utils;
    const Cc = Components.classes;

    Cu.import("resource://gre/modules/XPCOMUtils.jsm");

    function MozCacheAccessor() { }

	var _metaData = [];
	
    MozCacheAccessor.prototype = {
      classDescription: "CacheManager Addon",
      classID:          Components.ID("{1C220952-E8C5-4824-8F20-42D11791C51E}"),
      contractID:       "@se7en-soft.com/moz/cacheaccessor;1",
      QueryInterface: XPCOMUtils.generateQI([Components.interfaces.nsIMozCacheAccessor]),
  
		getCacheEntry : function(session, key, mode, block){
			var svc = Cc["@mozilla.org/network/cache-service;1"].getService(Ci.nsICacheService);
			var ssn = svc.createSession(session, mode, true);
			ssn.doomEntriesIfExpired = false;
						
			var entry = {};
			try
			{
				entry = ssn.openCacheEntry(key, Ci.nsICache.ACCESS_READ, true);
			}
			catch(e)
			{
				const kNetBase = 2152398848; // 0x804B0000
				var NS_ERROR_CACHE_KEY_NOT_FOUND = kNetBase + 61
				if (e.result == NS_ERROR_CACHE_KEY_NOT_FOUND) {
					return;
				} else {
				  throw e;
				}
			}
			return entry;
		},
		
		asyncOpenCacheEntry : function(session, key, mode, listener){
			var svc = Cc["@mozilla.org/network/cache-service;1"].getService(nsICacheService);
			var ssn = svc.createSession(session, mode, true);
			ssn.doomEntriesIfExpired = false;
			
			try
			{
				ssn.asyncOpenCacheEntry(key, nsICache.ACCESS_READ, listener);
			}
			catch(e)
			{
				const kNetBase = 2152398848; // 0x804B0000
				var NS_ERROR_CACHE_KEY_NOT_FOUND = kNetBase + 61
				if (e.result == NS_ERROR_CACHE_KEY_NOT_FOUND) {
					return;
				} else {
				  throw e;
				}
			}
		},
		
		visitMetaData: function(descriptor){
			var visitor = {
				visitMetaDataElement: function(aKey, aValue) {
					_metaData.push(aKey + ":" + aValue);
					return true;
				}
			};
			
			_metaData = [];
			descriptor.visitMetaData(visitor);
		},
		
		fetchMetaData: function(){
			return _metaData;
		}
  
    };
    var components = [MozCacheAccessor];
    function NSGetModule(compMgr, fileSpec) {
      return XPCOMUtils.generateModule(components);
    }