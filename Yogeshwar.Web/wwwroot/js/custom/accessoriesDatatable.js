$(function () {
    var columns = [
        { data: "id", name: "Id", "autoWidth": true },
        {
            name: "name", "autoWidth": true,
            sDefaultContent: "--",
            render: function (data, type, row) {
                return "<a href='/accessories/detail/" + row.id + "' style='color:hotpink' >" + row.name + "</a>";
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
            sDefaultContent: renderDefaultAction(),
            render: function (data, type, row) {
                return renderAction(row, '/accessories/addedit/' + row.id, '/accessories/delete/' + row.id, '/accessories/ActiveInActiveRecord/' + row.id);
            }
        },
    ];

    BindGrid("/accessories/bindData/", columns, [1, 2, 3], [1, 2, 3], [1, 2, 3]);
});