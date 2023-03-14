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
}

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