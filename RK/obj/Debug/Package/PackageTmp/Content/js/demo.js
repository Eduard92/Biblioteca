(function(root, factory) {
  if (typeof define === 'function' && define.amd) {
    define([], factory);
  } else if (typeof exports === 'object') {
    module.exports = factory();
  } else {
    root.pxDemo = factory();
  }
}(this, function() {
  'use strict';

  function attachOnLoadHandler(cb) {
    if (window.attachEvent) {
      window.attachEvent('onload', cb);
    } else if (window.onload) {
      var curronload = window.onload;

      window.onload = function(evt) {
        curronload(evt);
        cb(evt);
      };
    } else {
      window.onload = cb;
    }
  }

  var locationParts = String(document.location).replace(/\\/g, '/').split('/');
  var DIST_PATH = locationParts[locationParts.length - 2] === 'docs' ? '..' : '../..';

  var pxDemo = (function() {

    // Constants

    var COLORS = [
      '#0288D1',
      '#FF4081',
      '#4CAF50',
      '#D32F2F',
      '#FFC107',
      '#673AB7',
      '#FF5722',
      '#CDDC39',
      '#795548',
      '#607D8B',
      '#009688',
      '#E91E63',
      '#9E9E9E',
      '#E040FB',
      '#00BCD4',
    ];

    var BACKGROUNDS = [
      DIST_PATH + '/dist/demo/bgs/1.jpg',
      DIST_PATH + '/dist/demo/bgs/2.jpg',
      DIST_PATH + '/dist/demo/bgs/3.jpg',
      DIST_PATH + '/dist/demo/bgs/4.jpg',
      DIST_PATH + '/dist/demo/bgs/5.jpg',
      DIST_PATH + '/dist/demo/bgs/6.jpg',
      DIST_PATH + '/dist/demo/bgs/7.jpg',
      DIST_PATH + '/dist/demo/bgs/8.jpg',
      DIST_PATH + '/dist/demo/bgs/9.jpg',
    ];

    var THEMES = [
      'default',
      'asphalt',
      'purple-hills',
      'adminflare',
      'dust',
      'frost',
      'fresh',
      'silver',
      'clean',
      'white',
    ];

    var demoSettings = (function loadDemoSettings() {
      var result = {
        fixed_navbar:  '0',
        fixed_nav:     '0',
        right_nav:     '0',
        offcanvas_nav: '0',
        footer:        'bottom',
        theme:         THEMES[0],
      };

      var cookie = ';' + document.cookie + ';';

      var re;
      var found;

      for (var key in result) {
        if (Object.prototype.hasOwnProperty.call(result, key)) {
          re = new RegExp(';\\s*' + encodeURIComponent('px-demo-' + key) + '\\s*=\\s*([^;]+)\\s*;');
          found = cookie.match(re);

          if (found) {
            result[key] = decodeURIComponent(found[1]);
          }
        }
      }

      // Guards
      result.fixed_navbar  = [ '0', '1' ].indexOf(result.fixed_navbar) !== -1 ? result.fixed_navbar : '0';
      result.fixed_nav     = [ '0', '1' ].indexOf(result.fixed_nav) !== -1 ? result.fixed_nav : '0';
      result.right_nav     = [ '0', '1' ].indexOf(result.right_nav) !== -1 ? result.right_nav : '0';
      result.offcanvas_nav = [ '0', '1' ].indexOf(result.offcanvas_nav) !== -1 ? result.offcanvas_nav : '0';
      result.footer        = [ 'static', 'bottom', 'fixed' ].indexOf(result.footer) !== -1 ? result.footer : 'bottom';
      result.theme         = THEMES.indexOf(result.theme) !== -1 ? result.theme : THEMES[0];

      return result;
    })();

    // Private

    function updateDemoSettings(settings) {
      $.extend(demoSettings, settings);

      for (var key in demoSettings) {
        if (Object.prototype.hasOwnProperty.call(demoSettings, key)) {
          document.cookie =
            encodeURIComponent('px-demo-' + key) + '=' +
            encodeURIComponent(demoSettings[key]);
        }
      }
    }

    function setTheme(themeName) {
      var themePath = DIST_PATH + '/dist/css/themes/' + themeName + '.min.css';
      var head      = document.getElementsByTagName('head')[0];
      var link      = document.createElement('link');

      link.type      = 'text/css';
      link.rel       = 'stylesheet';
      link.href      = themePath;
      link.className = 'px-demo-theme-stylesheet';

      document.documentElement.className += ' px-demo-no-transition';

      // Async load

      var r = false;

      link.onload = link.onreadystatechange = function() {
        if (!r && (!this.readyState || this.readyState === 'complete')) {
          r = true;

          var links = document.getElementsByClassName('px-demo-theme-stylesheet');

          if (links.length > 1) {
            for (var i = 0, l = links.length - 1; i < l; i++) {
              head.removeChild(links[i]);
            }
          }

          document.documentElement.className =
            document.documentElement.className.replace(/\s*px-demo-no-transition/ig, '');
        }
      };

      head.appendChild(link);
    }

    function loadTheme() {
      setTheme(demoSettings.theme);
    }

    function placeNav(side) {
      var navEl  = document.getElementById('px-demo-nav');

      navEl.className =
        navEl.className
          .replace(new RegExp("^\\s*px-nav-(?:left|right)\\s*", 'i'), '')
          .replace(new RegExp("\\s*px-nav-(?:left|right)\\s*$", 'i'), '')
          .replace(new RegExp("\\s+px-nav-(?:left|right)\\s+", 'ig'), ' ') +
        ' px-nav-' + side;
    }

    function setFooterPosition(pos) {
      var footer = document.getElementById('px-demo-footer');

      if (!footer) { return; }

      footer.className = footer.className
        .replace(/^\s*px-footer-(?:bottom|fixed)\s*/i, '')
        .replace(/\s*px-footer-(?:bottom|fixed)\s*$/i, '')
        .replace(/\s+px-footer-(?:bottom|fixed)\s+/gi, ' ') +
        ((pos === 'bottom' || pos === 'fixed') ? (' px-footer-' + pos) : '');
    }

    function capitalizeAllLetters(str, splitter) {
      var parts = str.split(splitter || ' ');

      for (var i = 0, l = parts.length; i < l; i++) {
        parts[i] = parts[i].charAt(0).toUpperCase() + parts[i].slice(1);
      }

      return parts.join(' ');
    }

    // Public

    function shuffle(a) {
      var j;
      var x;
      var i;

      for (i = a.length; i; i -= 1) {
        j = Math.floor(Math.random() * i);
        x = a[i - 1];
        a[i - 1] = a[j];
        a[j] = x;
      }
    }

    function getRandomData(max, min) {
      return Math.floor(Math.random() * ((max || 100) - (min || 0))) + (min || 0);
    }

    function getRandomColors(count) {
      if (count && count > COLORS.length) {
        throw new Error('Have not enough colors');
      }

      var clrLeft = count || COLORS.length;
      var source  = [].concat(COLORS);
      var result  = [];

      while (clrLeft-- > 0) {
        result.unshift(source[source.length > 1 ? getRandomData(source.length - 1) : 0]);
        source.splice(source.indexOf(result[0]), 1);
      }

      shuffle(result);

      return result;
    }

    function initializeDemo() {
      if (!document.getElementById('px-demo-footer')) {
        $('#px-demo-footer-position').remove();

        if (!document.getElementById('px-demo-nav')) {
          $('#px-demo-togglers').remove();
        }
      }

      $('input#px-demo-fixed-navbar-toggler').on('change', function() {
        updateDemoSettings({
          fixed_navbar: $(this).is(':checked') ? '1' : '0',
        });

        $(document.body)[
          $(this).is(':checked') ? 'addClass' : 'removeClass'
        ]('px-navbar-fixed');

        var $fixedNavToggler = $('input#px-demo-fixed-nav-toggler');

        if (!$(this).is(':checked') && $fixedNavToggler.is(':checked')) {
          $fixedNavToggler.click();
        }
      });

      $('input#px-demo-fixed-nav-toggler').on('change', function() {
        updateDemoSettings({
          fixed_nav: $(this).is(':checked') ? '1' : '0',
        });

        $('#px-demo-nav')[
          $(this).is(':checked') ? 'addClass' : 'removeClass'
        ]('px-nav-fixed');

        var $fixedNavbarToggler = $('input#px-demo-fixed-navbar-toggler');

        if ($(this).is(':checked') && !$fixedNavbarToggler.is(':checked')) {
          $fixedNavbarToggler.click();
        }

        $(window).trigger('scroll');
      });

      $('input#px-demo-nav-right-toggler').on('change', function() {
        updateDemoSettings({
          right_nav: $(this).is(':checked') ? '1' : '0',
        });

        placeNav($(this).is(':checked') ? 'right' : 'left');
      });

      $('input#px-demo-nav-off-canvas-toggler').on('change', function() {
        updateDemoSettings({
          offcanvas_nav: $(this).is(':checked') ? '1' : '0',
        });

        $('#px-demo-nav')[
          $(this).is(':checked') ? 'addClass' : 'removeClass'
        ]('px-nav-off-canvas');

        $(window).trigger('resize');
      });

      $('select#px-demo-footer-position-select').on('change', function() {
        updateDemoSettings({
          footer: $(this).val(),
        });

        setFooterPosition($(this).val());

        $(window).trigger('resize');
      });

      $('input[name="px-demo-current-theme"]').on('change', function() {
        var themeName = THEMES.indexOf(this.value) !== -1 ? this.value : THEMES[0];

        updateDemoSettings({ theme: themeName });
        setTheme(themeName);
      });


      // Initialize "close" button
      //

      $('#demo-px-nav-box .close').on('click', function(e) {
        e.preventDefault();

        var $box     = $(this).parents('.px-nav-box').addClass('no-animation');
        var $wrapper = $('<div></div>').css({ overflow: 'hidden' });

        // Remove close button
        $(this).remove();

        $wrapper
          .insertBefore($box)
          .append($box)
          .animate({
            opacity: 0,
            height:  'toggle',
          }, 400, function() {
            $wrapper.remove();
          });
      });
    }

    function initializeBgsDemo(selector, defaultBgIndex, overlay, afterCall) {
      var isBgSet = false;

      if (defaultBgIndex) {
        $(selector).pxResponsiveBg({
          backgroundImage: BACKGROUNDS[defaultBgIndex - 1],
          overlay:         overlay,
        });

        isBgSet = true;

        if (afterCall) { afterCall(isBgSet); }
      }

      var elementsHtml = '<a href="#" class="px-demo-bgs-container px-demo-bgs-clear">&times;</a>';

      for (var i = 0, l = BACKGROUNDS.length; i < l; i++) {
        elementsHtml += '<a href="#" class="px-demo-bgs-container"><img src="' + BACKGROUNDS[i] + '" alt=""></a>';
      }

      var $block = $('<div class="px-demo-bgs">' + elementsHtml + '</div>');

      $block.on('click', '.px-demo-bgs-container', function(e) {
        e.preventDefault();

        var $container = $(this);

        if ($container.hasClass('px-demo-bgs-clear')) {
          if (!isBgSet) { return; }

          $(selector).pxResponsiveBg('destroy', true);

          isBgSet = false;

          if (afterCall) { afterCall(isBgSet); }
        } else {
          if (isBgSet) { $(selector).pxResponsiveBg('destroy'); }

          $(selector).pxResponsiveBg({
            backgroundImage: $container.find('> img').attr('src'),
            overlay:         overlay,
          });

          isBgSet = true;

          if (afterCall) { afterCall(isBgSet); }
        }
      });

      $('body').append($block);
    }

    function initializeDemoSidebar() {
      var sidebarEl = document.createElement('DIV');

      sidebarEl.id          = 'px-demo-sidebar';
      sidebarEl.className   = 'px-sidebar-right';
      sidebarEl.style.width = '242px';
      sidebarEl.innerHTML   = '<a href="#" id="px-demo-sidebar-toggle" data-toggle="sidebar" data-target="#px-demo-sidebar"><i class="ion-ios-gear"></i><i class="ion-android-close"></i></a>';

      var contentEl = document.createElement('DIV');

      contentEl.className = 'px-sidebar-content';
      sidebarEl.appendChild(contentEl);

      var content  = '';
      var navEl    = document.getElementById('px-demo-nav');

      content += '<div id="px-demo-togglers">';

      content += '<h6 class="px-demo-sidebar-header">SETTINGS</h6>';

      // Togglers

      if (navEl) {
        content += '<div><div class="box m-a-0 border-radius-0 bg-transparent">';

        // Fixed navbar

        content +=
          '<div class="box-row">' +
            '<div class="box-cell p-l-3"><label for="px-demo-fixed-navbar-toggler">Fixed navbar</label></div>' +
            '<div class="box-cell p-r-3" style="width: 70px;">' +
              '<label for="px-demo-fixed-navbar-toggler" class="switcher switcher-blank switcher-sm switcher-primary"><input type="checkbox" id="px-demo-fixed-navbar-toggler"' + (demoSettings.fixed_navbar === '1' ? ' checked' : '') + '><div class="switcher-indicator"><div class="switcher-yes"><i class="fa fa-check"></i></div><div class="switcher-no"><i class="fa fa-close"></i></div></div></label>' +
            '</div>' +
          '</div>';

        if (demoSettings.fixed_navbar === '1') {
          document.body.className += ' px-navbar-fixed';
        }

        // Fixed nav

        content +=
          '<div class="box-row">' +
            '<div class="box-cell p-l-3"><label for="px-demo-fixed-nav-toggler">Fixed nav</label></div>' +
            '<div class="box-cell p-r-3" style="width: 70px;">' +
              '<label for="px-demo-fixed-nav-toggler" class="switcher switcher-blank switcher-sm switcher-primary"><input type="checkbox" id="px-demo-fixed-nav-toggler"' + (demoSettings.fixed_nav === '1' ? ' checked' : '') + '><div class="switcher-indicator"><div class="switcher-yes"><i class="fa fa-check"></i></div><div class="switcher-no"><i class="fa fa-close"></i></div></div></label>' +
            '</div>' +
          '</div>';

        if (demoSettings.fixed_nav === '1') {
          navEl.className += ' px-nav-fixed';
        }

        // Right nav

        content +=
          '<div class="box-row">' +
            '<div class="box-cell p-l-3"><label for="px-demo-nav-right-toggler">Right nav</label></div>' +
            '<div class="box-cell p-r-3" style="width: 70px;">' +
              '<label for="px-demo-nav-right-toggler" class="switcher switcher-blank switcher-sm switcher-primary"><input type="checkbox" id="px-demo-nav-right-toggler"' + (demoSettings.right_nav === '1' ? ' checked' : '') + '><div class="switcher-indicator"><div class="switcher-yes"><i class="fa fa-check"></i></div><div class="switcher-no"><i class="fa fa-close"></i></div></div></label>' +
            '</div>' +
          '</div>';

        placeNav(demoSettings.right_nav === '1' ? 'right' : 'left');

        // Off canvas nav

        content +=
          '<div class="box-row">' +
            '<div class="box-cell p-l-3"><label for="px-demo-nav-off-canvas-toggler">Off canvas nav</label></div>' +
            '<div class="box-cell p-r-3" style="width: 70px;">' +
              '<label for="px-demo-nav-off-canvas-toggler" class="switcher switcher-blank switcher-sm switcher-primary"><input type="checkbox" id="px-demo-nav-off-canvas-toggler"' + (demoSettings.offcanvas_nav === '1' ? ' checked' : '') + '><div class="switcher-indicator"><div class="switcher-yes"><i class="fa fa-check"></i></div><div class="switcher-no"><i class="fa fa-close"></i></div></div></label>' +
            '</div>' +
          '</div>';

        if (demoSettings.offcanvas_nav === '1') {
          navEl.className += ' px-nav-off-canvas';
        }

        content += '</div></div>';
      }


      // Footer

      content +=
        '<div id="px-demo-footer-position"><div class="box m-a-0 border-radius-0 bg-transparent">' +
          '<div class="box-row">' +
            '<div class="box-cell p-l-3"><label for="px-demo-footer-position-select">Footer</label></div>' +
            '<div class="box-cell p-r-3">' +
              '<select class="custom-select form-control input-sm" id="px-demo-footer-position-select"><option value="static"' + (demoSettings.footer === 'static' ? ' selected' : '') + '>Static</option><option value="bottom"' + (demoSettings.footer === 'bottom' ? ' selected' : '') + '>Bottom</option><option value="fixed"' + (demoSettings.footer === 'fixed' ? ' selected' : '') + '>Fixed</option></select>' +
            '</div>' +
          '</div>' +
        '</div></div>';

      attachOnLoadHandler(function() {
        setFooterPosition(demoSettings.footer);
      });

      content += '</div>';


      // Themes

      content += '<h6 class="px-demo-sidebar-header">THEMES</h6>';
      content += '<div class="px-demo-themes-list clearfix">';

      for (var i = 0, l = THEMES.length; i < l; i++) {
        content += '<label class="px-demo-themes-item">';

          content += '<input type="radio" class="px-demo-themes-toggler" name="px-demo-current-theme" value="' + THEMES[i] + '"' + (demoSettings.theme === THEMES[i] ? ' checked' : '') + '>';
          content += '<img src="' + DIST_PATH + '/dist/demo/themes/' + THEMES[i] + '.png" class="px-demo-themes-thumbnail">';
          content += '<div class="px-demo-themes-title">' + capitalizeAllLetters(THEMES[i], '-') + '</div>';

        content += '</label>';
      }

      content += '</div>';

      contentEl.innerHTML = content;
      document.body.appendChild(sidebarEl);
    }

    // Return

    return {
      COLORS: COLORS,

      shuffle:         shuffle,
      getRandomData:   getRandomData,
      getRandomColors: getRandomColors,

      initializeDemo:        initializeDemo,
      //initializeBgsDemo:     initializeBgsDemo,
      initializeDemoSidebar: initializeDemoSidebar,

      loadTheme: loadTheme,
    };
  })();

  return pxDemo;
}));
