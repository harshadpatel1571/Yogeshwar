$(function () {
    var columns = [
        { data: "id", name: "Id", "autoWidth": true },
        {
            name: "First name", "autoWidth": true,
            sDefaultContent: "--",
            render: function (data, type, row) {
                return "<a href='/customer/detail/" + row.id + "' style='color:hotpink'>" + row.firstName + "</a>";
            }
        },
        { data: "lastName", name: "Last name", "autoWidth": true },
        { data: "email", sDefaultContent: "", name: "Email", "autoWidth": true },
        { data: "phoneNo", name: "Phone no", "autoWidth": true },
        { data: "gstNumber", name: "Gst number", "autoWidth": true, sDefaultContent: "" },
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
                    "                                    <a href='/customer/addedit/" + row.id + "' class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\" Title=\"Edit Record.\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick=\"deleteRecord('/customer/delete/" + row.id + "')\">\n" +
                    "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete Record.\"></i>\n" +
                    "                                    </a>\n" + getActiveInActiveHTML(row.isActive, row.id, '/Customer/ActiveInActiveRecord/' + row.id) +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick=\"addOrder('/order/addedit/" + row.id + "')\">\n" +
                    "                                        <i class=\"ri-shopping-cart-2-line\" Title=\"Add Order to this customer.\"></i>\n" +
                    "                                    </a> " +
                    "                                </div>";
            }
        },
    ];

    BindGrid("/customer/bindData/", columns, [1], [1], [1]);
});

function addOrder(url) {
    window.location.replace(url);
}