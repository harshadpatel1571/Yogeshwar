$(function () {
    var columns = [
        { data: "id", name: "Id", "autoWidth": true },
        {
            name: "Name", "autoWidth": true,
            sDefaultContent: "--",
            render: function (data, type, row) {
                return "<a href='/product/detail/" + row.id + "' style='color:hotpink'>" + row.name + "</a>";
            }
        },
        { data: "modelNo", name: "Model No", "autoWidth": true },
        { data: "price", name: "Price", "autoWidth": true },
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
                    "                                    <a href='/product/addedit/" + row.id + "' class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\" Title=\"Edit Record.\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a class=\"link-danger fs-20 sa-warning\" onclick=\"deleteRecord('/product/delete/" + row.id + "')\">\n" +
                    "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete Record.\"></i>\n" +
                    "                                    </a>\n" + getActiveInActiveHTML(row.isActive, row.id, '/Product/ActiveInActiveRecord/' + row.id) +
                    "                                </div>";
            }
        },
    ];
     
    BindGrid("/product/bindData/", columns, [1], [1], [1]);
});