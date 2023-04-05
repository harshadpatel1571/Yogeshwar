$(function () {
    const columns = [
        {data: "id", name: "Id", "autoWidth": true},
        {
            name: "Customer Name", "autoWidth": true,
            sDefaultContent: "--",
            render: function (data, type, row) {
                return "<a href='/order/detail/" + row.id + "' style='color:hotpink'>" + row.customerName + "</a>";
            }
        },
        {data: "amount", name: "Amount", "autoWidth": true},
        {data: "orderDate", name: "Order Date", "autoWidth": true},
        {data: "orderCount", name: "Order Items", "autoWidth": true},
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
                    // "                                    <a href='/Order/AddEdit/" + row.id + "' class=\"link-success fs-20\">\n" +
                    // "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                    // "                                    </a>\n" +
                    "                                    <a class=\"link-danger fs-20 sa-warning\" onclick=\"deleteRecord('/order/delete/" + row.id + "')\">\n" +
                    "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete Record.\"Title=\"Delete Record.\"s></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>";
            }
        },
    ];

    BindGrid("/order/bindData/", columns, [1], [1], [1]);
});