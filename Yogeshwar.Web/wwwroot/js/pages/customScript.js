$(function () {
    $('#grid').DataTable({
        paging: true,
        lengthChange: true,
        searching: true,
        ordering: true,
        lengthMenu: [10, 25, 50, 75, 100],
        info: true,
        autoWidth: true,
        responsive: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: {
            url: "/Customer/BindData/",
            type: "POST",
            datatype: "json"
        },
        columnDefs: [{
            targets: [0],
            visible: false,
            searchable: false
        }],
        columns: [
            { data: "id", name: "Id", "autoWidth": true },
            { data: "firstName", name: "First name", "autoWidth": true },
            { data: "lastName", name: "Last name", "autoWidth": true },
            { data: "email", name: "Email", "autoWidth": true },
            { data: "phoneNo", name: "Phone no", "autoWidth": true },
            { data: "city", name: "City", "autoWidth": true },
            {
                bSortable: false,
                autoWidth: true,
                sDefaultContent: "<div class=\"hstack gap-3 flex-wrap\">\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick='foo(0)'>\n" +
                    "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>",
                render: function (data, type, row) {
                    return "<div class=\"hstack gap-3 flex-wrap\">\n" +
                        "                                    <a href='/Customer/AddEdit/" + row.id + "' class=\"link-success fs-20\">\n" +
                        "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-danger fs-20 sa-warning\" onclick='deleteRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                        "                                    </a>\n" +
                        "                                </div>";
                }
            },
        ],
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'csvHtml5',
                text: 'CSV',
                titleAttr: 'Generate CSV',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'colvis',
                text: 'Column Visibility',
            },
            //'copy', 'excel', 'csv', 'pdf', {
            //    extend: 'print',
            //    text: 'Print'
            //}, 'colvis'
        ]
    }).buttons().container().appendTo('#grid_wrapper .col-md-6:eq(0)');
});

$(document).ready(function () {
    var dtable = $('#grid').DataTable();
    $('.dataTables_filter input')
        .unbind()
        .bind('input', function (e) {
            if (this.value.length >= 3 || e.keyCode == 13) {
                dtable.search(this.value).draw();
            }
            if (this.value == "") {
                dtable.search("").draw();
            }
        });
});

function deleteRecord(id) {
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
        $.ajax({
            type: "POST",
            url: "/Customer/Delete/" + id,
            success: function () {
                t.value && Swal.fire({
                    title: "Deleted!",
                    text: "Your record has been deleted.",
                    icon: "success",
                    confirmButtonClass: "btn btn-primary w-xs mt-2",
                    buttonsStyling: !1
                }).then(function () {
                    var table = $("#grid").DataTable();
                    table.ajax.reload(null, false);
                });
            }
        });
    });
}