$(document).ready(function () {
    select2Init();

    const mod = $.cookie("mod");

    if (mod !== undefined) {
        $('#theme').attr('data-layout-mode', mod);
    }

    const view = $.cookie("view");

    if (view !== undefined && view !== '') {
        $('body').addClass(view);
    }  

    var uri = window.location.href.toString();
    var msg = getParameterByName("msg", uri);
    if (msg != undefined && msg == "success") {
        showToaster("success", "Saved","Record have been saved successfully.")
    }
    else if (msg != undefined && msg == "error") {
        showToaster("error", "Opps !!", "Something whent wrong. Try again.")
    }
    RemoveQueryParameter(uri);
});

var ckClassicEditor = document.querySelectorAll(".ckeditor-classic"),
    snowEditor = (ckClassicEditor &&
        Array.from(ckClassicEditor).forEach(function () {
            ClassicEditor.create(document.querySelector(".ckeditor-classic"), {
                removePlugins: ['CKFinderUploadAdapter', 'CKFinder', 'EasyImage', 'Image',
                    'ImageCaption', 'ImageStyle', 'ImageToolbar', 'ImageUpload', 'MediaEmbed']
            })
                .then(function (e) {
                    e.ui.view.editable.element.style.height = "200px";
                })
                .catch(function (e) {
                    console.error(e);
                });
        })
    );

document.addEventListener("keydown", function (event) {
    if (event.altKey && event.code === "KeyC") {
        event.preventDefault();
        navigateToUrl("/customer");
    }
    else if (event.altKey && event.code === "KeyP") {
        event.preventDefault();
        navigateToUrl("/product");
    }
    else if (event.altKey && event.code === "KeyO") {
        event.preventDefault();
        navigateToUrl("/order");
    }
    else if (event.altKey && event.code === "KeyA") {
        event.preventDefault();
        navigateToUrl("/accessories");
    }
    else if (event.altKey && event.code === "KeyG") {
        event.preventDefault();
        navigateToUrl("/categories");
    }
    else if (event.altKey && event.code === "KeyF") {
        event.preventDefault();
        navigateToUrl("/configuration");
    }
    else if (event.altKey && event.code === "KeyD") {
        event.preventDefault();
        navigateToUrl("/");
    }
});

function navigateToUrl(url) {
    window.location.href = url;
}

function changeMode() {
    const mod = $.cookie("mod");

    if (mod === undefined || mod === '') {
        $.cookie("mod", 'dark', { expires: 1000, path: '/' });
        return;
    }

    if (mod === 'dark') {
        $.cookie("mod", 'light', { expires: 1000, path: '/' });
    } else {
        $.cookie("mod", 'dark', { expires: 1000, path: '/' });
    }
}

function changeView() {
    const view = $.cookie("view");

    if (view === undefined || view === '') {
        $.cookie("view", 'fullscreen-enable', { path: '/' });
        return;
    }

    if (view === 'fullscreen-enable') {
        $.cookie("view", '', { path: '/' });
    }
}

function showToaster(type, title, message) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    toastr[type](message, title);
}

function BindGrid(pURL, pColumns, pdfColumn, excelColumn, printColumn) {
    $('#grid').DataTable().destroy();
    $('#grid').DataTable({
        paging: true,
        lengthChange: true,
        searching: true,
        ordering: true,
        lengthMenu: [[10, 25, 50, 75, 100, 250], [10, 25, 50, 75, 100, 250]],
        info: true,
        autoWidth: true,
        responsive: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: {
            url: pURL,
            type: "POST",
            datatype: "json"
        },
        columnDefs: [{
            targets: [0],
            visible: false,
            searchable: false
        }],
        columns: pColumns,
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: pdfColumn
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: excelColumn
                }
            },
            //{
            //    extend: 'csvHtml5',
            //    text: 'CSV',
            //    titleAttr: 'Generate CSV',
            //    exportOptions: {
            //        columns: [1, 2, 3]
            //    }
            //},
            //{
            //    extend: 'copyHtml5',
            //    text: 'Copy',
            //    titleAttr: 'Copy to clipboard',
            //    exportOptions: {
            //        columns: [1, 2, 3]
            //    }
            //},
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: printColumn
                }
            },
            //{
            //    extend: 'colvis',
            //    text: 'Column Visibility',
            //},
        ]
    }).buttons().container().appendTo('#grid_wrapper .col-md-6:eq(0)');

    restrictSearchFilter();
}

function restrictSearchFilter() {
    const table = $('#grid').DataTable();
    $('.dataTables_filter input')
        .unbind()
        .bind('input', function (e) {
            if (this.value.length >= 3 || e.keyCode === 13) {
                table.search(this.value).draw();
            }
            if (this.value === "") {
                table.search("").draw();
            }
        });
}

function getActiveInActiveHTML(flag, id, url) {
    if (flag) {
        return "                                   <div class='form-check form-switch form-switch-custom form-switch-success'>" +
            `                                       <input type ='checkbox' class='form-check-input' Title='Active And Inactive Record.' onclick='changeRecordStatus(\"btnActive_${id}\", \"${url}\")' active='${flag}' checked id='btnActive_${id}'>` +
            "                                   </div>"
    }

    return "                                   <div class='form-check form-switch form-switch-custom form-switch-success'>" +
        `                                       <input type ='checkbox' class='form-check-input' Title='Active And Inactive Record.' onclick='changeRecordStatus(\"btnActive_${id}\", \"${url}\")' active='${flag}' id='btnActive_${id}'>` +
        "                                   </div>"
}

function changeRecordStatus(id, url) {
    const field = $(`#${id}`);

    $.ajax({
        type: "POST",
        url: url,
        success: function (flag) {
            if (flag) {
                field.attr('Checked', 'Checked');
                showToaster("success", "Active", "Record has been Activated.");
            }
            else {
                field.removeAttr('Checked');
                showToaster("success", "InActive", "Record has been InActivated.");
            }
        },
        error: function () {
            showToaster("error", "Error !!", "Error while performing operation.");
        }
    });
}

function deleteRecord(pURL) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: !0,
        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
        cancelButtonClass: "btn btn-danger w-xs mt-2",
        confirmButtonText: "Yes, delete it!",
        buttonsStyling: !1,
        showCloseButton: !0,
    }).then(function (t) {
        if (!t.isConfirmed) return;
        $.ajax({
            type: "POST",
            url: pURL,
            success: function () {
                showToaster("success", "Deleted", "Your record has been deleted.");
                const table = $("#grid").DataTable();
                table.ajax.reload(null, false);
            },
            error: function (response) {
                let message = "This entity is being referred somewhere else.";

                if (response.status === 404) {
                    message = "Entity can not be found.";
                }
                showToaster("error", "Error !!", message);
            }
        });
    });
}

function select2Init() {

    $(".js-example-basic-single").select2(),
        $(".js-example-basic-multiple").select2();

    $(".js-example-disabled").select2(),
        $(".js-example-disabled-multi").select2(),
        $(".js-programmatic-enable").on("click", function () {
            $(".js-example-disabled").prop("disabled", !1),
                $(".js-example-disabled-multi").prop("disabled", !1);
        }),
        $(".js-programmatic-disable").on("click", function () {
            $(".js-example-disabled").prop("disabled", !0),
                $(".js-example-disabled-multi").prop("disabled", !0);
        });
}

function flatInit() {
    var m = document.querySelectorAll("[data-provider]");
    Array.from(m).forEach(function (e) {
        var t, a, o;
        "flatpickr" == e.getAttribute("data-provider")
            ? ((t = {}),
                (o = e.attributes)["data-date-format"] && (t.dateFormat = o["data-date-format"].value.toString()),
                o["data-enable-time"] && ((t.enableTime = !0), (t.dateFormat = o["data-date-format"].value.toString() + " H:i")),
                o["data-altFormat"] && ((t.altInput = !0), (t.altFormat = o["data-altFormat"].value.toString())),
                o["data-minDate"] && ((t.minDate = o["data-minDate"].value.toString()), (t.dateFormat = o["data-date-format"].value.toString())),
                o["data-maxDate"] && ((t.maxDate = o["data-maxDate"].value.toString()), (t.dateFormat = o["data-date-format"].value.toString())),
                o["data-deafult-date"] && ((t.defaultDate = o["data-deafult-date"].value.toString()), (t.dateFormat = o["data-date-format"].value.toString())),
                o["data-multiple-date"] && ((t.mode = "multiple"), (t.dateFormat = o["data-date-format"].value.toString())),
                o["data-range-date"] && ((t.mode = "range"), (t.dateFormat = o["data-date-format"].value.toString())),
                o["data-inline-date"] && ((t.inline = !0), (t.defaultDate = o["data-deafult-date"].value.toString()), (t.dateFormat = o["data-date-format"].value.toString())),
                o["data-disable-date"] && ((a = []).push(o["data-disable-date"].value), (t.disable = a.toString().split(","))),
                o["data-week-number"] && ((a = []).push(o["data-week-number"].value), (t.weekNumbers = !0)),
                flatpickr(e, t))
            : "timepickr" == e.getAttribute("data-provider") &&
            ((a = {}),
                (o = e.attributes)["data-time-basic"] && ((a.enableTime = !0), (a.noCalendar = !0), (a.dateFormat = "H:i")),
                o["data-time-hrs"] && ((a.enableTime = !0), (a.noCalendar = !0), (a.dateFormat = "H:i"), (a.time_24hr = !0)),
                o["data-min-time"] && ((a.enableTime = !0), (a.noCalendar = !0), (a.dateFormat = "H:i"), (a.minTime = o["data-min-time"].value.toString())),
                o["data-max-time"] && ((a.enableTime = !0), (a.noCalendar = !0), (a.dateFormat = "H:i"), (a.minTime = o["data-max-time"].value.toString())),
                o["data-default-time"] && ((a.enableTime = !0), (a.noCalendar = !0), (a.dateFormat = "H:i"), (a.defaultDate = o["data-default-time"].value.toString())),
                o["data-time-inline"] && ((a.enableTime = !0), (a.noCalendar = !0), (a.defaultDate = o["data-time-inline"].value.toString()), (a.inline = !0)),
                flatpickr(e, a));
    });
}

function ckEditorInit(id) {
    var ckClassicEditor = document.querySelectorAll(id + " .ckeditor-classic"),
        snowEditor = (ckClassicEditor &&
            Array.from(ckClassicEditor).forEach(function () {
                ClassicEditor.create(document.querySelector(id + " .ckeditor-classic"), {
                    removePlugins: ['CKFinderUploadAdapter', 'CKFinder', 'EasyImage', 'Image',
                        'ImageCaption', 'ImageStyle', 'ImageToolbar', 'ImageUpload', 'MediaEmbed']
                })
                    .then(function (e) {
                        e.ui.view.editable.element.style.height = "200px";
                    })
                    .catch(function (e) {
                        console.error(e);
                    });
            })
        );
}

function RemoveQueryParameter(uri) {
    if (uri.indexOf("?") > 0) {
        var clean_uri = uri.substring(0, uri.indexOf("?"));
        window.history.replaceState({}, document.title, clean_uri);
    }
}

function getParameterByName(name, url) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}