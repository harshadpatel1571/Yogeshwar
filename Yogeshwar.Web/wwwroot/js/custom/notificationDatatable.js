$(function () {
    $('#grid').DataTable({
        paging: true,
        lengthChange: true,
        searching: true,
        ordering: true,
        lengthMenu: [[10, 25, 50, 75, 100, -1], [10, 25, 50, 75, 100, 'All']],
        info: true,
        autoWidth: true,
        responsive: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: {
            url: "/Notification/BindData/",
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
            { data: "productName", name: "Product Name", "autoWidth": true },
            { data: "productAccessoriesName", name: "Product Accessories Name", "autoWidth": true },
            { data: "orderId", name: "Order Id", "autoWidth": true },
            { data: "strIsCompleted", name: "StrIsCompleted", "autoWidth": true },
            //{
            //    bSortable: false,
            //    autoWidth: true,
            //    sDefaultContent: "<div class=\"hstack gap-3 flex-wrap\">\n" +
            //        "                                    <a href=\"javascript:void(0);\" class=\"link-success fs-20\">\n" +
            //        "                                        <i class=\"ri-edit-2-line\"></i>\n" +
            //        "                                    </a>\n" +
            //        "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick='deleteRecord(0)'>\n" +
            //        "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
            //        "                                    </a>\n" +
            //        "                                </div>",
            //    render: function (data, type, row) {
            //        return "<div class=\"hstack gap-3 flex-wrap\">\n" +
            //            "                                    <a href='/Notification/AddEdit/" + row.id + "' class=\"link-success fs-20\">\n" +
            //            "                                        <i class=\"ri-edit-2-line\"></i>\n" +
            //            "                                    </a>\n" +
            //            "                                    <a class=\"link-danger fs-20 sa-warning\" onclick='deleteRecord(" + row.id + ")'>\n" +
            //            "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
            //            "                                    </a>\n" +
            //            "                                </div>";
            //    }
            //},
        ],
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
            {
                extend: 'csvHtml5',
                text: 'CSV',
                titleAttr: 'Generate CSV',
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [1, 2, 3, 4]
                }
            },
            {
                extend: 'colvis',
                text: 'Column Visibility',
            },
        ]
    }).buttons().container().appendTo('#grid_wrapper .col-md-6:eq(0)');
});

$(document).ready(function () {
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
        if (!t.isConfirmed) return;
        $.ajax({
            type: "POST",
            url: "/Notification/Delete/" + id,
            success: function () {
                t.value && Swal.fire({
                    title: "Deleted!",
                    text: "Your record has been deleted.",
                    icon: "success",
                    confirmButtonClass: "btn btn-primary w-xs mt-2",
                    buttonsStyling: !1
                }).then(function () {
                    const table = $("#grid").DataTable();
                    table.ajax.reload(null, false);
                });
            },
            error: function (response) {
                let message = "This entity is being referred somewhere else.";

                if (response.status === 404) {
                    message = "Entity can not be found.";
                }

                t.value && Swal.fire({
                    title: "Unable to delete.",
                    text: message,
                    icon: "warning",
                    confirmButtonClass: "btn btn-primary w-xs mt-2",
                    buttonsStyling: !1
                })
            }
        });
    });
}