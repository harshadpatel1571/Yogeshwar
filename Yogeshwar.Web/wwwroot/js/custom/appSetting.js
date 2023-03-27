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
});

var ckClassicEditor = document.querySelectorAll(".ckeditor-classic"),
    snowEditor = (ckClassicEditor &&
        Array.from(ckClassicEditor).forEach(function () {
            ClassicEditor.create(document.querySelector(".ckeditor-classic"), {
                removePlugins: ['CKFinderUploadAdapter', 'CKFinder', 'EasyImage', 'Image',
                    'ImageCaption', 'ImageStyle', 'ImageToolbar', 'ImageUpload', 'MediaEmbed']
            })
                .then(function (e) {
                    console.log(e.plugins._availablePlugins);
                    e.ui.view.editable.element.style.height = "200px";
                })
                .catch(function (e) {
                    console.error(e);
                });
        })
    );


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
        lengthMenu: [[5, 10, 25, 50, 75, 100, -1], [5, 10, 25, 50, 75, 100, 'All']],
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