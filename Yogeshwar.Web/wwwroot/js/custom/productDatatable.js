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
            sDefaultContent: renderDefaultAction(),
            render: function (data, type, row) {
                return renderAction(row, '/product/addedit/' + row.id, '/product/delete/' + row.id, '/product/ActiveInActiveRecord/' + row.id);
            }
        },
    ];
     
    BindGrid("/product/bindData/", columns, [1], [1], [1]);
});