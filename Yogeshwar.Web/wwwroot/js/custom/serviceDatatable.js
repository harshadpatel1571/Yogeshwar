$(function () {
    var columns = [
        { data: "id", name: "Id", "autoWidth": true },
        {
            name: "Worker name", "autoWidth": true,
            sDefaultContent: "--",
            render: function (data, type, row) {
                return "<a href='/service/detail/" + row.id + "' style='color:hotpink'>" + row.workerName + "</a>";
            }
        },
        { data: "orderCustomerName", name: "OrderCustomerName", "autoWidth": true },
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
                    "                                    <a href='/service/addedit/" + row.id + "' class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\" Title=\"Edit Record.\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a class=\"link-danger fs-20 sa-warning\" onclick=\"deleteRecord('/service/delete/" + row.id + "')\">\n" +
                    "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete Record.\"></i>\n" +
                    "                                    </a>\n" +
                    "                                   <div class=\"form-check form-switch form-switch-custom form-switch-success\">" +
                    "                                       <input type =\"checkbox\" class=\"form-check-input\" Title=\"Active And Inactive Record.\"> " +
                    "                                   </div>" +
                    "                                </div>";
            }
        },
    ];

    BindGrid("/service/bindData/", columns, [1], [1], [1]);
});