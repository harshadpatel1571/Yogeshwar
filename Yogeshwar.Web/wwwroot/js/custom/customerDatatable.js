$(function () {
    var columns = [
        { data: "id", name: "Id", "autoWidth": true },
        {
            name: "First name", "autoWidth": true,
            sDefaultContent: "--",
            render: function (data, type, row) {
                return "<a href='/Customer/Detail/" + row.id + "' style='color:hotpink'>" + row.firstName + "</a>";
            }
        },
        { data: "lastName", name: "Last name", "autoWidth": true },
        {
            data: "email", sDefaultContent: "", name: "Email", "autoWidth": true
        },
        { data: "phoneNo", name: "Phone no", "autoWidth": true },
        { data: "city", name: "City", "autoWidth": true },
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
                    "                                    <a href='/Customer/AddEdit/" + row.id + "' class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\" Title=\"Edit Record.\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a class=\"link-danger fs-20 sa-warning\" onclick=\"deleteRecord('/Customer/Delete/" + row.id + "')\">\n" +
                    "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete Record.\"></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>";
            }
        },
    ];

    BindGrid("/Customer/BindData/", columns, [1], [1], [1]);
});