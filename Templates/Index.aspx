<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CRM.Templates.WebForm1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="LayoutHiddenArea" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LayoutHeaderDecorator" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="LMainContent" runat="server">

    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">View Templates</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">View Templates</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->

    <!--Start of Page Content-->
    <section class="content">
        
        <div class="">
            <button id="cr-temp-btn" type="button" class="btn btn-sm btn-primary"><span class="fa fa-plus-circle"></span> Create new Template</button>
        </div>

        <br />

        <!-- Template List -->
            <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">Templates</h3>
            </div>
            <div class="box-body">
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr class="table-row-header">
                            <th>ID</th>
                            <th>Name</th>
                            <th>Description</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody id="template-table-tbody"></tbody>
                </table>
            </div>
        </div>
        <!-- Template List -->

        <!--Create new Template modal-->
            <div id="t-create-form-modal" class="modal fade" data-backdrop="false" data-keyboard="false"  tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title"></h4>
                        </div>
                        <div id="t-create-form-modal-body" class="modal-body"></div>
                    </div><!-- /.modal-content -->
                </div><!-- /.modal-dialog -->
            </div><!-- /.modal -->
        <!--Create new Template modal-->

    </section>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="LayoutScriptDecorator" runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootbox.js/4.4.0/bootbox.min.js"></script>
    <script src='https://cloud.tinymce.com/stable/tinymce.min.js'></script>
    <script src="//unpkg.com/mithril/mithril.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            var ajaxing = false;

            $('#cr-temp-btn').click(function () {
                $('body').trigger('createbtn.clicked');
            });
            
            var Template = {
                field: { Id: '', Name: '', Description: '', Type: '', Body: '' },
                tags: [],
                list: [],
                Types: [],
                resetField: function () {
                    Template.field = { Id: '', Name: '', Description: '', Type: '', Body: '' };
                },
                preview: function (id) {
                    var pt = Template.list.find((t) => t.Id == id);
                    var dialog = bootbox.dialog({
                        title: pt.Name,
                        message: pt.Body
                    });
                },
                edit: function (id) {
                    var pt = Template.list.find((t) => t.Id == id);
                    Template.field.Id = pt.Id;
                    Template.field.Name = pt.Name;
                    Template.field.Description = pt.Description;
                    Template.field.Type = pt.Type;
                    Template.field.Body = pt.Body;

                    $('body').trigger('editbtn.clicked');
                },
                save: function () {
                    var payload = {
                        id: Template.field.Id,
                        name: Template.field.Name,
                        description: Template.field.Description,
                        type: Template.field.Type,
                        body: Template.field.Body
                    };

                    if (ajaxing == false) {
                        ajaxing = true;
                        m.request({
                            url: '/xhr.aspx/UpdateTemplate',
                            method: 'POST',
                            data: payload,
                            headers: {
                                'Content-Type': 'application/json',
                            }
                        }).then(function (data) {
                            ajaxing = false;
                            if (data.d.success && data.d.success == true) {
                                Template.resetField();
                                $('#t-create-form-modal').modal('hide');
                                bootbox.alert("Template successfully updated!");
                                $('body').trigger('t-refresh');
                            } else {
                                if (data.d.message) {
                                    bootbox.alert(data.d.message);
                                } else {
                                    bootbox.alert("An error occured while proccessing your request.");
                                }  
                            }
                            
                        });
                    }
                },
                create: function () {

                    var payload = {
                        name: Template.field.Name,
                        description: Template.field.Description,
                        type: Template.field.Type,
                        body: Template.field.Body
                    };

                    if (ajaxing == false) {
                        ajaxing = true;
                        m.request({
                            url: '/xhr.aspx/CreateTemplate',
                            method: 'POST',
                            data: payload,
                            headers: {
                                'Content-Type': 'application/json',
                            }
                        }).then(function (data) {
                            ajaxing = false;
                            if (data.d.success && data.d.success == true) {
                                Template.resetField();
                                $('#t-create-form-modal').modal('hide');
                                bootbox.alert("Template successfully created!");
                                $('body').trigger('t-refresh');
                            } else {
                                if (data.d.message) {
                                    bootbox.alert(data.d.message);
                                } else {
                                    bootbox.alert("An error occured while proccessing your request.");
                                }  
                            }
                            
                        });
                    }
                },
                getTags: function () {
                    m.request({
                        url: '/xhr.aspx/GetTags',
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        }
                    }).then(function (data) {
                        Template.tags = data.d.reverse();
                        $('body').trigger('tags.ready');
                    });
                },
                getTypes: function () {
                    m.request({
                        url: '/xhr.aspx/GetTemplateTypes',
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        }
                    }).then(function (data) {
                        Template.Types = data.d.reverse();
                    });
                },
                loadList: function () {
                    m.request({
                        url: '/xhr.aspx/GetTemplates',
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        }
                    }).then(function (data) {
                        Template.list = data.d.reverse();
                    });
                }
            };
            
            // template list script.
            (function ($) {
                var tbdy = document.querySelector("#template-table-tbody");
                var TemplateList = {
                    oninit: function () {
                        Template.loadList();
                        $('body').on('t-refresh', function () {
                            Template.loadList();
                        });
                    },
                    view: function () {
                        return Template.list.map(function (t) {
                            return m("tr", { class: 'template-list-item' }, [
                                m("td", { class: 'template-list-item-col' }, t.Id),
                                m("td", { class: 'template-list-item-col' }, t.Name),
                                m("td", { class: 'template-list-item-col' }, t.Description),
                                m("td", { class: 'template-list-item-col text-center' }, [
                                    m("a", { key: t.Id, class: 'template-list-item-preview', href: '#', onclick: () => { Template.preview(t.Id) } }, 'Preview'),
                                    m("span", ' | '),
                                    m("a", { key: t.Id, class: 'template-list-item-preview', href: '#', onclick: () => { Template.edit(t.Id) } }, 'Edit'),
                                ])
                            ]);
                        });
                    }
                }
                m.mount(tbdy, TemplateList);
            })(jQuery);

            // Create new Template script
            (function ($) {
                
                var TemplateForm = {
                    submithandler: null,
                    oninit: function () {
                        Template.getTypes();
                        Template.getTags();
                    },
                    oncreate: function () {
                        $('body').on('tags.ready', function () {

                            // inline editor component/
                            var editor = {
                                oncreate: function () {
                                    tinymce.init({
                                        selector: '#rkeditor',
                                        plugins: "contextmenu",
                                        contextmenu: 'placeholder',
                                        setup: function (editor) {
                                            $('body').on('tinymce.trigger.save', function () {
                                                tinymce.triggerSave();
                                            });

                                            editor.addMenuItem('placeholder', {
                                                text: 'Insert Placeholder',
                                                context: 'tools',
                                                menu: TemplateForm.getPlaceholders(editor)
                                            });

                                            editor.on('change', function () {
                                                Template.field.Body = editor.getContent();
                                                $('body').trigger('tinymce.trigger.save');
                                            });
                                        }
                                    });
                                },
                                view: function () {
                                    return m("textarea#rkeditor.form-control[name=t_body] [required]", {
                                        value: Template.field.Body
                                    });
                                }
                            };
                            var rkew = $('#t-create-form-modal-body').find('#rkeditor-wrapper').get()[0];
                            m.mount(rkew, editor);
                        });
                    },
                    getPlaceholders: function (editor) {
                        var ph = Template.tags.map((tag) => {
                            var onclick = function () {
                                editor.insertContent(tag.Value);
                            }
                            return { text: tag.Name, onclick: onclick };
                        });

                        return ph;
                    },
                    ttypeOptions: function () {
                        var options = [];
                        options.push(m("option", { value: ''}, "Select Template type"));
                        Template.Types.forEach(function (t) {
                            options.push(m("option", { value: t.Value }, t.Key));
                        });
                        return options;
                    },
                    view: function () {
                        return m("form#t-form", {
                            onsubmit: function (e) {
                                $('body').trigger('tinymce.trigger.save');
                                TemplateForm.submithandler();
                                e.preventDefault();
                            }
                        }, [
                                m("div.form-group", [
                                    m("input.form-control[type=text] [name=t_name] [placeholder=Template Name][required]", {
                                        oninput: m.withAttr("value", function (value) { Template.field.Name = value }),
                                        value: Template.field.Name
                                    })
                                ]),
                                m("div.form-group", [
                                    m("input.form-control[type=text] [name=t_description] [placeholder=Template Description][required]", {
                                        oninput: m.withAttr("value", function (value) { Template.field.Description = value }),
                                        value: Template.field.Description
                                    })
                                ]),
                                m("div.form-group", [
                                    m("select.form-control [name=t_type] [placeholder=Template Type][required]", {
                                        onchange: m.withAttr("value", function (value) { Template.field.Type = value }),
                                        value: Template.field.Type
                                    }, TemplateForm.ttypeOptions())
                                ]),
                                m("div#rkeditor-wrapper.form-group", [
                                    m("p.text-info.text-center", "Initializing Editor...")
                                ]),
                                m("div.form-group", [
                                    m("button.btn.btn-primary.pull-right[type=submit]", "Save"),
                                    m("div", { style: 'clear:both;'})
                                ]),
                        ]);
                    }
                }
                
                $('body').on('createbtn.clicked', function () {
                    var title = $('#t-create-form-modal').find('.modal-title').get()[0];
                    m.render(title, "Create new Template");
                    TemplateForm.submithandler = Template.create;
                    $('#t-create-form-modal').modal('show');
                });

                $('body').on('editbtn.clicked', function () {
                    var title = $('#t-create-form-modal').find('.modal-title').get()[0];
                    m.render(title, "Edit Template");
                    TemplateForm.submithandler = Template.save;
                    $('#t-create-form-modal').modal('show');
                });


                $('#t-create-form-modal').on('show.bs.modal', function () {
                    var tcfmbody = document.querySelector('#t-create-form-modal-body'); 
                    m.mount(tcfmbody, TemplateForm);         
                });

            })(jQuery);
        });
    </script>

</asp:Content>
