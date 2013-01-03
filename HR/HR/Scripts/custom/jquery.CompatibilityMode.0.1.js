/**
* CompatitibilityMode is designed to detect if the user is using Internet Explorer and if it
* is running in compatibility mode
*
* popUp r1 // 2012.06.13 // jQuery 8.3+
*  
* CompatibilityMode is currently available for use in all personal or commercial 
* projects under both MIT and GPL licenses. This means that you can choose 
* the license that best suits your project, and use it accordingly.
* 
* // basic usage (just like .hover) receives onMouseOver and onMouseOut functions
* $.CompatitibilityMode( callback );
* 
* @param  callback function || method to fire if compatibility mode tests true
* @param  g  onMouseOut function  || Nothing (use configuration options object)
* @author    Jessica Janiuk
*/

(function ($) {
    $.CompatibilityMode = function (callback) {
        // default configuration options
        if ($.browser.msie && ($.browser.version == '7.0' && navigator.userAgent.indexOf('Trident') != -1) || (document.documentMode != undefined && (Number($.browser.version) != Number(document.documentMode)))) {
            if(typeof callback == 'function') {
                callback.call(this);
            }
        }
    };
})(jQuery);