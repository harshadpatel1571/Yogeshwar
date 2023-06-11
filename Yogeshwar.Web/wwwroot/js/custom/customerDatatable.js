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
            sDefaultContent: renderDefaultAction(),
            render: function (data, type, row) {
                return renderAction(row, '/customer/addedit/' + row.id, '/customer/delete/' + row.id, '/customer/ActiveInActiveRecord/' + row.id, '/order/addedit/' + row.id);
            }
        },
    ];

    BindGrid("/customer/bindData/", columns, [1], [1], [1]);
});

function addOrder(url) {
    window.location.replace(url);
}