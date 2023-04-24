$(function () {
    const columns = [
        { data: "id", name: "Id", "autoWidth": true },
        {
            name: "image", "autoWidth": true,
            sDefaultContent: "--",
            render: function (data, type, row) {
                if (row.image != undefined) {
                    return "<img src='" + row.image + "' height='50' width='50'/>";
                }
                else {
                    return "<img src='https://localhost:44320/DataImages/Category/d5305eda5d1a44ccb6076986dda7c8a2.png' height='50' width='50'/>"
                }
            }
        },
        { data: "name", name: "Name", "autoWidth": true },
        { data: "hsnNo", name: "HsnNo", "autoWidth": true },
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
                    "                                    <a href='/categories/addedit/" + row.id + "' class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\" Title=\"Edit Record.\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick=\"deleteRecord('/categories/delete/" + row.id + "')\">\n" +
                    "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete Record.\"></i>\n" +
                    "                                    </a>\n" + getActiveInActiveHTML(row.isActive, row.id, '/Categories/ActiveInActiveRecord/' + row.id) +
                    "                                </div>";
            }
        },
    ];
    BindGrid("/categories/bindData/", columns, [1], [1], [1]);
});