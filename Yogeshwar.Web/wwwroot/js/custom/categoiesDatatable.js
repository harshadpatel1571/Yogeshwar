$(function () {
    var columns = [
        { data: "id", name: "Id", "autoWidth": true },
        {
            name: "name", "autoWidth": true,
            sDefaultContent: "--",
            render: function (data, type, row) {
                return "<a href='/Categories/Detail/" + row.id + "' style='color:hotpink' >" + row.name + "</a>";
            }
        },
        {
            bSortable: false,
            autoWidth: true,
            sDefaultContent: "<div class=\"hstack gap-3 flex-wrap\">\n" +
                "                                    <a href=\"javascript:void(0);\" class=\"link-success fs-20\">\n" +
                "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                "                                    </a>\n" +
                "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick='deleteRecord(0)'>\n" +
                "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                "                                    </a>\n" +
                "                                </div>",
            render: function (data, type, row) {
                return "<div class=\"hstack gap-3 flex-wrap\">\n" +
                    "                                    <a href='/Categories/AddEdit/" + row.id + "' class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a class=\"link-danger fs-20 sa-warning\" onclick=\"deleteRecord('/Categories/Delete/" + row.id + "')\">\n" +
                    "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>";
            }
        },
    ];
    BindGrid("/Categories/BindData/", columns, [1], [1], [1]);
});

//$(document).ready(function () {
//    const table = $('#grid').DataTable();
//    $('.dataTables_filter input')
//        .unbind()
//        .bind('input', function (e) {
//            if (this.value.length >= 3 || e.keyCode === 13) {
//                table.search(this.value).draw();
//            }
//            if (this.value === "") {
//                table.search("").draw();
//            }
//        });
//});

//function deleteRecord(id) {
//    Swal.fire({
//        title: "Are you sure?",
//        text: "You won't be able to revert this!",
//        icon: "warning",
//        showCancelButton: !0,
//        confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
//        cancelButtonClass: "btn btn-danger w-xs mt-2",
//        confirmButtonText: "Yes, delete it!",
//        buttonsStyling: !1,
//        showCloseButton: !0,
//    }).then(function (t) {
//        if (!t.isConfirmed) return;
//        $.ajax({
//            type: "POST",
//            url: "/Categories/Delete/" + id,
//            success: function () {
//                t.value && Swal.fire({
//                    title: "Deleted.",
//                    text: "Your record has been deleted.",
//                    icon: "success",
//                    confirmButtonClass: "btn btn-primary w-xs mt-2",
//                    buttonsStyling: !1
//                }).then(function () {
//                    const table = $("#grid").DataTable();
//                    table.ajax.reload(null, false);
//                });
//            },
//            error: function (response) {
//                let message = "This entity is being referred somewhere else.";

//                if (response.status === 404) {
//                    message = "Entity can not be found.";
//                }

//                t.value && Swal.fire({
//                    title: "Unable to delete.",
//                    text: message,
//                    icon: "warning",
//                    confirmButtonClass: "btn btn-primary w-xs mt-2",
//                    buttonsStyling: !1
//                })
//            }
//        });
//    });
//}