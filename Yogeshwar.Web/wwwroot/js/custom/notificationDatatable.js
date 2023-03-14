$(function () {
    var columns = [
        { data: "id", name: "Id", "autoWidth": true },
        { data: "productName", name: "Product Name", "autoWidth": true },
        { data: "productAccessoriesName", name: "Product Accessories Name", "autoWidth": true },
        { data: "orderId", name: "Order Id", "autoWidth": true },
        { data: "strIsCompleted", name: "StrIsCompleted", "autoWidth": true },
    ];

    BindGrid("/Notification/BindData/", columns, [1], [1], [1]);
});