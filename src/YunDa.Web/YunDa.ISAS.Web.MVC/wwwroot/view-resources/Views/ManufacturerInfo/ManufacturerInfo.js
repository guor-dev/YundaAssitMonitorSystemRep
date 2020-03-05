$(function () {
    manufactureList.initEditModalByVue("editManufactureModal");
    manufactureList.initEditFormValidate("editForm");
    $("#searchManufactureBtn").click(function () {
        manufactureList.refreshTable();
    });
    $("#addManufactureBtn").click(function () {
        manufactureList.initEditModalValues("");
    });
    $("#delManufactureBtn").click(function () {
        manufactureList.delManufacture();
    });
    manufactureList.initTable("tableManufacture", "tableToolbar");
    softDelManufacture.initSoftDelManufacture();
});

var manufactureList = {
    mainTableElementId: null,
    editManufactureModalVue: null,
    editFormId: null,
    initTable: function (tableId, toolBarId) {
        manufactureList.mainTableElementId = tableId;
        if (!manufactureList.mainTableElementId) return;
        let tableHeight = document.body.clientHeight - 10;
        isas.bootstrapTable({
            el: '#' + manufactureList.mainTableElementId,
            toolBarEl: '#' + toolBarId,
            url: AppServiceUrl.ManufacturerInfo_FindDatas,
            //height: tableHeight,
            toolbarAlign: 'left',
            showToggle: 'true',
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        manufacturerName: $("#searchManufactureName").val(),
                        isOnlyDeleted: false
                    },
                    sorting: "manufacturerName"
                }
                return c
            },
            columns: [
                {
                    checkbox: true, // 显示一个勾选框
                    align: 'center', // 居中显示
                    valign: 'middle',
                    class: "checkbox checkbox-primary",
                    width: 50,
                    formatter: function (value, row, index) {
                        //let pageSize = $('#' + manufactureList.mainTableElementId).bootstrapTable("getOptions").pageSize;
                        //let pageNumber = $('#' + manufactureList.mainTableElementId).bootstrapTable("getOptions").pageNumber;
                        return value;
                    }
                },
                {
                    title: '行号',
                    align: 'center',
                    valign: 'middle',
                    width: 50,
                    formatter: function (value, row, index) {
                        let pageSize = $('#' + manufactureList.mainTableElementId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + manufactureList.mainTableElementId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'manufacturerName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '厂商名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'phoneNumber',
                    title: '厂商电话',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'emailAddress',
                    title: '厂商邮件地址',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'manufacturerAddress',
                    title: '厂商地址',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'remark',
                    title: '备注',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'isActive',
                    title: '状态',
                    align: 'center',
                    valign: 'middle',
                    width: 80,
                    formatter: function (value, row, index) {
                        //value： 该列的字段值；
                        //row： 这一行的数据对象；
                        // index： 行号，第几行，从0开始计算
                        var text = '-';
                        if (value) {
                            text = "<span class='text-success' style='font-size:16px;'><i class='fa fa-check'></i></span>";
                        } else {
                            text = "<span class='text-danger' style='font-size:16px;'><i class='fa fa-close'></i></span>";
                        }
                        return text;
                    },
                }
                ,
                {
                    title: "操作",
                    align: 'center',
                    valign: 'middle',
                    width: 80, // 定义列的宽度，单位为像素px
                    formatter: function (value, row, index) {
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#editManufactureModal' onclick='manufactureList.initEditModalValues(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-pencil' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }
            ]
        });
    },
    initEditModalByVue: function (editManufactureModalId) {
        if (!editManufactureModalId) return;
        manufactureList.editManufactureModalVue = new Vue({
            el: '#' + editManufactureModalId,
            data: {
                id: null,
                header: '编辑',
                manufacturerName: "",
                phoneNumber: "",
                emailAddress: "",
                manufacturerAddress: "",
                remark: "",
                isActive: true
            },

            methods: {
                save: function (event) {
                    if (!manufactureList.mainTableElementId || !manufactureList.editFormId) return;
                    if (!$("#" + manufactureList.editFormId).valid()) return;
                    let data = {
                        id: this.id,
                        manufacturerName: this.manufacturerName,
                        phoneNumber: this.phoneNumber,
                        manufacturerAddress: this.manufacturerAddress,
                        emailAddress: this.emailAddress,
                        remark: this.remark,
                        isActive: this.isActive
                    }
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.ManufacturerInfo_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    $(manufactureList.editManufactureModalVue.$el).modal('hide');
                                    manufactureList.refreshTable();
                                }
                            }
                        }
                    })
                }
            }
        })
    },
    initEditFormValidate: function (editFormId) {
        if (!editFormId) return;
        manufactureList.editFormId = editFormId
        $("#" + editFormId).validate({
            rules: {
                manufacturerName: {
                    required: true,
                },
                phoneNumber: {
                    phone: true,
                },
                emailAddress: {
                    address: true,
                }
            },
            messages: {
                manufacturerName: {
                    required: "名称不能为空",
                },
                phoneNumber: {
                    phone: "请输入有效的电话号码",
                },
                emailAddress: {
                    address: "请输入有效的邮箱地址",
                }
            }
        });
        /*手机号格式验证*/
        $.validator.addMethod("phone", function (value, element, params) {
            //0571 - 88075998
            //var reg = /^1[34578]\d{9}$/;//手机号码
            //var isMob = /^0?1[3|4|5|8][0-9]\d{8}$/;// 座机格式
            var reg = /^\d+-?\d+$/;
            return this.optional(element) || reg.test(value);
        }, "phone error");
        /*邮箱格式验证*/
        $.validator.addMethod("address", function (value, element, params) {
            var reg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
            return this.optional(element) || reg.test(value);
        }, "address error");
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + manufactureList.editFormId).validate().resetForm();
        $("#" + manufactureList.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    initEditModalValues: function (uniqueId) {
        if (!manufactureList.editManufactureModalVue || !manufactureList.mainTableElementId) return;
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + manufactureList.mainTableElementId).bootstrapTable('getRowByUniqueId', uniqueId);

        manufactureList.resetFormValidate()
        if (!rowData) {
            rowData = {
                id: null,
                manufacturerName: "",
                phoneNumber: "",
                emailAddress: "",
                manufacturerAddress: "",
                remark: "",
                isActive: true
            }
            manufactureList.editManufactureModalVue.header = '添加';
        } else
            manufactureList.editManufactureModalVue.header = '编辑';
        manufactureList.editManufactureModalVue.id = rowData.id;
        manufactureList.editManufactureModalVue.manufacturerName = rowData.manufacturerName;
        manufactureList.editManufactureModalVue.phoneNumber = rowData.phoneNumber;
        manufactureList.editManufactureModalVue.emailAddress = rowData.emailAddress;
        manufactureList.editManufactureModalVue.manufacturerAddress = rowData.manufacturerAddress;
        manufactureList.editManufactureModalVue.remark = rowData.remark;
        manufactureList.editManufactureModalVue.isActive = rowData.isActive;
    },
    delManufacture: function () {
        if (!manufactureList.mainTableElementId) return;
        let row = $("#" + manufactureList.mainTableElementId).bootstrapTable('getSelections');
        if (!row || row.length == 0) {
            layer.alert("请选择要删除的行！");
            return;
        }
        let ids = new Array();
        row.forEach(function (r) {
            ids.push(r.id);
        });
        isas.ajax({
            confirm: true,
            //请求地址
            url: AppServiceUrl.ManufacturerInfo_SoftDeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag)
                        manufactureList.refreshTable();
                    softDelManufacture.refreshTable();
                }
            },
        });
    },
    refreshTable: function () {
        $('#' + manufactureList.mainTableElementId).bootstrapTable('refresh');
    }
}

var softDelManufacture = {
    mainTableId: null,
    toolBarId: null,
    initSoftDelManufacture: function () {
        softDelManufacture.mainTableId = "sd_ManufactureTable";
        softDelManufacture.toolBarId = "sd_tableToolbar";
        softDelManufacture.initComponent();
    },
    initComponent: function () {
        $("#sd_searchManufactureBtn").click(function () {
            //刷新软删除的厂商列表
            softDelManufacture.refreshTable();
        });
        $("#sd_recoverManufactureBtn").click(function () {
            //恢复厂商TODO
            softDelManufacture.recover();
        });
        $("#sd_delManufactureBtn").click(function () {
            //彻底删除厂商 TODO
            ////console.log('hahahh')
            softDelManufacture.delete();
        });
        // 打开厂商软删除
        $('.open-small-chat').click(function () {
            $(this).children().toggleClass('fa-trash').toggleClass('fa-remove');
            let smallChatBox = $('.small-chat-box');
            smallChatBox.toggleClass('active');
            if (smallChatBox.hasClass("active"))
                softDelManufacture.initTable();
        });

        // 厂商软删除使用slimscroll
        $('.small-chat-box .content').slimScroll({
            height: '415px',
            railOpacity: 0.4
        });
    },
    initTable: function () {
        if (!softDelManufacture.mainTableId) return;
        //$('#' + softDelManufacture.mainTableId).empty();
        $('#' + softDelManufacture.mainTableId).bootstrapTable('destroy');
        let tableHeight = $("#" + softDelManufacture.mainTableId).parent().parent().height();
        isas.bootstrapTable({
            el: '#' + softDelManufacture.mainTableId,
            toolBarEl: '#' + softDelManufacture.toolBarId,
            url: AppServiceUrl.ManufacturerInfo_FindDatas,
            height: tableHeight,
            pageList: [8, 16, 32],
            pageSize: 8,
            theadClasses: 'thead-default',
            toolbarAlign: 'right',
            showToggle: false,
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        manufacturerName: $("#sd_searchManufactureName").val(),
                        isOnlyDeleted: true
                    },
                    sorting: "manufacturerName"
                }
                //console.log(c)
                return c
            },
            columns: [
                {
                    checkbox: true, // 显示一个勾选框
                    align: 'center', // 居中显示
                    valign: 'middle',
                    class: "checkbox checkbox-primary",
                    width: 50,
                    formatter: function (value, row, index) {
                        return value;
                    }
                },
                {
                    title: '行号',
                    align: 'center',
                    valign: 'middle',
                    width: 50,
                    formatter: function (value, row, index) {
                        let pageSize = $('#' + softDelManufacture.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + softDelManufacture.mainTableId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'manufacturerName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '厂商名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'remark',
                    title: '备注',
                    align: 'center',
                    valign: 'middle'
                }
            ]
        });
    },
    delete: function () {
        if (!softDelManufacture.mainTableId) return;
        let row = $("#" + softDelManufacture.mainTableId).bootstrapTable('getSelections');
        if (!row || row.length == 0) {
            layer.alert("请选择要删除的行！");
            return;
        }
        let ids = new Array();
        row.forEach(function (r) {
            ids.push(r.id);
        });
        isas.ajax({
            confirm: false,
            //请求地址
            url: AppServiceUrl.ManufacturerInfo_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) softDelManufacture.refreshTable();
                }
            },
        });
    },
    recover: function () {
        if (!softDelManufacture.mainTableId) return;
        let row = $("#" + softDelManufacture.mainTableId).bootstrapTable('getSelections');
        if (!row || row.length == 0) {
            layer.alert("请选择要恢复的行！");
            return;
        }
        let ids = new Array();
        row.forEach(function (r) {
            ids.push(r.id);
        });
        isas.ajax({
            confirm: false,
            //请求地址
            url: AppServiceUrl.ManufacturerInfo_RecoverByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        softDelManufacture.refreshTable();
                        manufactureList.refreshTable();
                    }
                }
            },
        });
    },
    refreshTable: function () {
        $('#' + softDelManufacture.mainTableId).bootstrapTable('refresh');
    },
}