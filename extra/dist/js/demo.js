/**
 * AdminLTE Demo Menu
 * ------------------
 * You should not use this file in production.
 * This file is for demo purposes only.
 */
(function ($, AdminLTE) {

  "use strict";

  /**
   * List of all the available skins
   *
   * @type Array
   */
  var my_skins = [
    "skin-blue",
    "skin-black",
    "skin-red",
    "skin-yellow",
    "skin-purple",
    "skin-green",
    "skin-blue-light",
    "skin-black-light",
    "skin-red-light",
    "skin-yellow-light",
    "skin-purple-light",
    "skin-green-light"
  ];

  //Create the new tab
  var tab_pane = $("<div />", {
    "id": "control-sidebar-theme-demo-options-tab",
    "class": "tab-pane active"
  });

  //Create the tab button
  var tab_button = $("<li />", {"class": "active"})
      .html("<a href='#control-sidebar-theme-demo-options-tab' data-toggle='tab'>"
      + "<i class='fa fa-wrench'></i>"
      + "</a>");

  //Add the tab button to the right sidebar tabs
  $("[href='#control-sidebar-home-tab']")
      .parent()
      .before(tab_button);

  //Create the menu
  var demo_settings = $("<div />");

  //Layout options
  demo_settings.append(
      "<h4 class='control-sidebar-heading'>"
      + "Theme Options"
      + "</h4>"
  );
  var skins_list = $("<ul />", {"class": 'control-sidebar-menu'});

  //Dark sidebar skins
  var skin_blue =
      $("<li />")
          .append("<a href='javascript:void(0);' data-skin='skin-blue' class='clearfix full-opacity-hover'>"
          + "<i class='menu-icon fa fa-cog bg-light-blue'></i>"
          + "<div class='menu-info'>"
          + "<h4 class='control-sidebar-subheading'>Light Blue Theme</h4>"
          + "<p>Change your theme color</p>"
          + "</div>"
          + "</a>");
  skins_list.append(skin_blue);

  var skin_purple =
      $("<li />")
          .append("<a href='javascript:void(0);' data-skin='skin-purple' class='clearfix full-opacity-hover'>"
          + "<i class='menu-icon fa fa-cog bg-purple'></i>"
          + "<div class='menu-info'>"
          + "<h4 class='control-sidebar-subheading'>Purple Theme</h4>"
          + "<p>Change your theme color</p>"
          + "</div>"
          + "</a>");
  skins_list.append(skin_purple);

  var skin_green =
      $("<li />")
          .append("<a href='javascript:void(0);' data-skin='skin-green' class='clearfix full-opacity-hover'>"
          + "<i class='menu-icon fa fa-cog bg-green'></i>"
          + "<div class='menu-info'>"
          + "<h4 class='control-sidebar-subheading'>Green Theme</h4>"
          + "<p>Change your theme color</p>"
          + "</div>"
          + "</a>");
  skins_list.append(skin_green);

  var skin_red =
      $("<li />")
          .append("<a href='javascript:void(0);' data-skin='skin-red' class='clearfix full-opacity-hover'>"
          + "<i class='menu-icon fa fa-cog bg-red'></i>"
          + "<div class='menu-info'>"
          + "<h4 class='control-sidebar-subheading'>Red Theme</h4>"
          + "<p>Change your theme color</p>"
          + "</div>"
          + "</a>");
  skins_list.append(skin_red);

  var skin_yellow =
      $("<li />")
          .append("<a href='javascript:void(0);' data-skin='skin-yellow' class='clearfix full-opacity-hover'>"
          + "<i class='menu-icon fa fa-cog bg-yellow'></i>"
          + "<div class='menu-info'>"
          + "<h4 class='control-sidebar-subheading'>Yellow Theme</h4>"
          + "<p>Change your theme color</p>"
          + "</div>"
          + "</a>");
  skins_list.append(skin_yellow);

  demo_settings.append(skins_list);

  tab_pane.append(demo_settings);
  $("#control-sidebar-home-tab").after(tab_pane);

  setup();

  /**
   * Toggles layout classes
   *
   * @param String cls the layout class to toggle
   * @returns void
   */
  function change_layout(cls) {
    $("body").toggleClass(cls);
    AdminLTE.layout.fixSidebar();
    //Fix the problem with right sidebar and layout boxed
    if (cls == "layout-boxed")
      AdminLTE.controlSidebar._fix($(".control-sidebar-bg"));
    if ($('body').hasClass('fixed') && cls == 'fixed') {
      AdminLTE.pushMenu.expandOnHover();
      AdminLTE.layout.activate();
    }
    AdminLTE.controlSidebar._fix($(".control-sidebar-bg"));
    AdminLTE.controlSidebar._fix($(".control-sidebar"));
  }

  /**
   * Replaces the old skin with the new skin
   * @param String cls the new skin class
   * @returns Boolean false to prevent link's default action
   */
  function change_skin(cls) {
    $.each(my_skins, function (i) {
      $("body").removeClass(my_skins[i]);
    });

    $("body").addClass(cls);
    store('skin', cls);
    return false;
  }

  /**
   * Store a new settings in the browser
   *
   * @param String name Name of the setting
   * @param String val Value of the setting
   * @returns void
   */
  function store(name, val) {
    if (typeof (Storage) !== "undefined") {
      localStorage.setItem(name, val);
    } else {
      window.alert('Please use a modern browser to properly view this template!');
    }
  }

  /**
   * Get a prestored setting
   *
   * @param String name Name of of the setting
   * @returns String The value of the setting | null
   */
  function get(name) {
    if (typeof (Storage) !== "undefined") {
      return localStorage.getItem(name);
    } else {
      window.alert('Please use a modern browser to properly view this template!');
    }
  }

  /**
   * Retrieve default settings and apply them to the template
   *
   * @returns void
   */
  function setup() {
    var tmp = get('skin');
    if (tmp && $.inArray(tmp, my_skins))
      change_skin(tmp);

    //Add the change skin listener
    $("[data-skin]").on('click', function (e) {
      e.preventDefault();
      change_skin($(this).data('skin'));
    });

    //Add the layout manager
    $("[data-layout]").on('click', function () {
      change_layout($(this).data('layout'));
    });

    $("[data-controlsidebar]").on('click', function () {
      change_layout($(this).data('controlsidebar'));
      var slide = !AdminLTE.options.controlSidebarOptions.slide;
      AdminLTE.options.controlSidebarOptions.slide = slide;
      if (!slide)
        $('.control-sidebar').removeClass('control-sidebar-open');
    });

    $("[data-sidebarskin='toggle']").on('click', function () {
      var sidebar = $(".control-sidebar");
      if (sidebar.hasClass("control-sidebar-dark")) {
        sidebar.removeClass("control-sidebar-dark")
        sidebar.addClass("control-sidebar-light")
      } else {
        sidebar.removeClass("control-sidebar-light")
        sidebar.addClass("control-sidebar-dark")
      }
    });

    $("[data-enable='expandOnHover']").on('click', function () {
      $(this).attr('disabled', true);
      AdminLTE.pushMenu.expandOnHover();
      if (!$('body').hasClass('sidebar-collapse'))
        $("[data-layout='sidebar-collapse']").click();
    });

    // Reset options
    if ($('body').hasClass('fixed')) {
      $("[data-layout='fixed']").attr('checked', 'checked');
    }
    if ($('body').hasClass('layout-boxed')) {
      $("[data-layout='layout-boxed']").attr('checked', 'checked');
    }
    if ($('body').hasClass('sidebar-collapse')) {
      $("[data-layout='sidebar-collapse']").attr('checked', 'checked');
    }

  }
})(jQuery, $.AdminLTE);
