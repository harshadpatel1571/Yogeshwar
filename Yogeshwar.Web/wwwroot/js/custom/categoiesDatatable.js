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
                    return "<img src='/images/noImage.png' height='60' width='60'/>"
                }
            }
        },
        { data: "name", name: "Name", "autoWidth": true },
        { data: "hsnNo", name: "HsnNo", "autoWidth": true },
        {
            bSortable: false,
            autoWidth: true,
            sDefaultContent: renderDefaultAction(),
            render: function (data, type, row) {
                return renderAction(row, '/categories/addedit/' + row.id, '/categories/delete/' + row.id, '/Categories/ActiveInActiveRecord/' + row.id);
            }
        },
    ];
    BindGrid("/categories/bindData/", columns, [1], [1], [1]);

});