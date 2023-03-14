$(function () {
    var columns = [
        { data: "id", name: "Id", "autoWidth": true },
        {
            name: "name", "autoWidth": true,
            sDefaultContent: "--",
            render: function (data, type, row) {
                return "<a href='/Accessories/Detail/" + row.id + "' style='color:hotpink' >" + row.name + "</a>";
            }
        },
        {
            name: "Description", "autoWidth": true,
            sDefaultContent: "",
            render: function (data, type, row) {
                if ((row.description != null || row.description != undefined) && row.description.length > 20) {
                    return row.description.substring(0, 20) + '...'
                }

                return row.description;
            }
        },
        { data: "quantity", name: "Quantity", "autoWidth": true },
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
                    "                                    <a href='/Accessories/AddEdit/" + row.id + "' class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a class=\"link-danger fs-20 sa-warning\" onclick=\"deleteRecord('/Accessories/Delete/" + row.id + "')\">\n" +
                    "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>";
            }
        },
    ];

    BindGrid("/Accessories/BindData/", columns, [1, 2, 3], [1, 2, 3], [1, 2, 3]);
});