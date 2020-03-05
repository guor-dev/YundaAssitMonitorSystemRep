$(function () {
    PowerSupplyLineList.initEditModalByVue("editPowerSupplyLineModal");
    PowerSupplyLineList.initEditFormValidate("editPowerSupplyLineForm");
    let isClick = true;
    $("#searchPowerSupplyLineBtn").click(function () {
        if (isClick) {
            isClick = false;
            $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable('refresh');
        }
        setTimeout(function () {
            isClick = true;
        }, 1000)
    });
    $("#addPowerSupplyLineBtn").click(function () {
        PowerSupplyLineList.initEditModalValues("");
    });
    $("#delPowerSupplyLineBtn").click(function () {
        PowerSupplyLineList.delLine();
    });
    //$("#searchPowerSupplyLineName").keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable('refresh');
    //    }
    //});
    PowerSupplyLineList.initPowerSupplyLineTable("tranPowerSupplyLineTable", "tablePowerSupplyLineToolbar");
});

var PowerSupplyLineList = {
    mainTableElementId: null,
    editModalVue: null,
    editFormId: null,
    initPowerSupplyLineTable: function (tableId, toolBarId) {
        PowerSupplyLineList.mainTableElementId = tableId;
        if (!PowerSupplyLineList.mainTableElementId) return;
        let tableHeight = document.body.clientHeight - 70;
        tableHeight = tableHeight < 200 ? 200 : tableHeight;
        isas.bootstrapTable({
            el: '#' + PowerSupplyLineList.mainTableElementId,
            toolBarEl: '#' + toolBarId,
            url: AppServiceUrl.PowerSupplyLine_FindDatas,
            //height: tableHeight,
            detailView: true,
            pageList: [14, 20, 26],
            pageSize: 14,
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        LineName: $("#searchPowerSupplyLineName").val()
                    },
                    sorting: "LineName"
                }
                return c
            },
            columns: [
                {
                    checkbox: true, // 显示一个勾选框
                    align: 'center', // 居中显示
                    valign: 'middle',
                    class: "checkbox checkbox-primary",
                    formatter: function (value, row, index) {
                        //let pageSize = $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable("getOptions").pageSize;
                        //let pageNumber = $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable("getOptions").pageNumber;
                        return value;
                    }
                },
                {
                    title: '行号',
                    align: 'center',
                    valign: 'middle',
                    width: 50,
                    formatter: function (value, row, index) {
                        let pageSize = $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'lineName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '线路名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
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
                },
                {
                    field: 'remark',
                    title: '备注',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    title: "操作",
                    align: 'center',
                    valign: 'middle',
                    width: 80, // 定义列的宽度，单位为像素px
                    formatter: function (value, row, index) {
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#editPowerSupplyLineModal' onclick='PowerSupplyLineList.initEditModalValues(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-pencil' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }
            ],
            onExpandRow: function (index, row, $detail) {
                if (SubstationList.ParentGuid !== null && SubstationList.ParentGuid != row.id) {
                    $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable('collapseRow', SubstationList.openedRowIndex);
                }
                SubstationList.ParentGuid = row.id;
                SubstationList.openedRowIndex = index;
                var html = "";
                html += "<div class='table-responsive gray-bg'>";
                html += "<div style='margin:0px 25px'>"
                html += "<div class='form-inline hidden-xs' id='tableSubstationToolbar_" + row.id + "' role='group'>"
                html += "<label class='control-label' for='searchSubstationName_" + row.id + "'><span class='text-danger'></span>变电所名称：</label>"
                html += "<div class='form-group'>"
                html += "<input type='text' class='form-control' placeholder='请输入变电所名称' id='searchSubstationName_" + row.id + "'> "
                html += "</div> "
                html += "<button type='button' class='btn btn-outline btn-primary' id='searchSubtationBtn_" + row.id + "'>"
                html += "<i class='fa fa-search' aria-hidden='true'></i>"
                html += "</button> "
                html += " <button class='btn btn-outline btn-primary' data-toggle='modal' data-target='#editSubstationModal' id ='addSubstationBtn_" + row.id + "'>"
                html += " <i class='fa fa-plus' aria-hidden='true'></i>"
                html += "</button> "
                html += "<button type='button' class='btn btn-outline btn-danger' id='delSubstationBtn_" + row.id + "'>"
                html += "<i class='fa fa-trash' aria-hidden='true'></i>"
                html += "</button>"
                html += "</div>"
                html += "<table id='childTable_" + row.id + "'>"
                html += "</table>"
                html += "</div>"
                html += "</div>"

                $detail.html(html);
                if (SubstationList.editModalVue == null) {
                    SubstationList.initEditModalByVue("editSubstationModal");
                    SubstationList.initEditFormValidate("editSubstationForm");
                };
                //var cur_table = $detail.html('<table  id="tableStationTable"></table>').find('table');
                let isClick = true;
                $("#searchSubtationBtn_" + row.id).click(function () {
                    if (isClick) {
                        isClick = false;
                        $('#' + SubstationList.mainTableElementId).bootstrapTable('refresh');
                    }
                    setTimeout(function () {
                        isClick = true;
                    }, 1000)
                });
                $("#addSubstationBtn_" + row.id).click(function () {
                    SubstationList.initEditModalValues("");
                })
                $("#delSubstationBtn_" + row.id).click(function () {
                    SubstationList.delStation();
                })
                //$("#searchSubstationName_" + SubstationList.ParentGuid).keydown(function (e) {
                //    if (e.keyCode == 13) {
                //        $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable('refresh');
                //    }
                //});
                SubstationList.initSubstationTable("childTable_" + row.id, "tableSubstationToolbar_" + row.id);
            }
        })
    },
    initEditModalByVue: function (editModalId) {
        if (!editModalId) return;
        PowerSupplyLineList.editLineModal = new Vue({
            el: '#' + editModalId,
            data: {
                id: null,
                header: "新增",
                lineName: "",
                remark: "",
                isActive: true
            },
            methods: {
                saveLine: function (event) {
                    if (!PowerSupplyLineList.mainTableElementId || !PowerSupplyLineList.editFormId) return;
                    if (!$("#" + PowerSupplyLineList.editFormId).valid()) return;
                    let data = {
                        id: this.id,
                        lineName: this.lineName,
                        remark: this.remark,
                        isActive: this.isActive
                    }
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.PowerSupplyLine_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable('refresh');
                                    $(PowerSupplyLineList.editLineModal.$el).modal('hide');
                                }
                            }
                        }
                    });
                }
            }
        })
    },
    initEditFormValidate: function (editFormId) {
        if (!editFormId) return;
        PowerSupplyLineList.editFormId = editFormId
        $("#" + editFormId).validate({
            rules: {
                lineName: {
                    required: true,
                },
                remark: {
                    required: false,
                }
            },
            messages: {
                lineName: {
                    required: "线路名称不能为空",
                }
            }
        });
    },
    initEditModalValues: function (uniqueId) {
        if (!PowerSupplyLineList.editLineModal || !PowerSupplyLineList.mainTableElementId) return;
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable('getRowByUniqueId', uniqueId);

        PowerSupplyLineList.resetFormValidate();
        if (!rowData) {
            rowData = {
                id: null,
                lineName: "",
                remark: "",
                isActive: true
            }
            PowerSupplyLineList.editLineModal.header = '添加';
        } else {
            PowerSupplyLineList.editLineModal.header = '编辑';
        }
        PowerSupplyLineList.editLineModal.id = rowData.id;
        PowerSupplyLineList.editLineModal.lineName = rowData.lineName;
        PowerSupplyLineList.editLineModal.remark = rowData.remark;
        PowerSupplyLineList.editLineModal.isActive = rowData.isActive;
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + PowerSupplyLineList.editFormId).validate().resetForm();
        $("#" + PowerSupplyLineList.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    delLine: function () {
        if (!PowerSupplyLineList.mainTableElementId) return;
        let row = $("#" + PowerSupplyLineList.mainTableElementId).bootstrapTable('getSelections');
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
            url: AppServiceUrl.PowerSupplyLine_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag)
                        $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable('refresh');
                }
            },
        });
    },
    refreshTable: function () {
        $('#' + PowerSupplyLineList.mainTableElementId).bootstrapTable('refresh');
    }
}

var SubstationList = {
    mainTableElementId: null,
    editModalVue: null,
    editFormId: null,
    ParentGuid: null,
    openedRowIndex: -1,
    initSubstationTable: function (tableId, toolBarId) {
        SubstationList.mainTableElementId = tableId;
        if (!SubstationList.mainTableElementId) return;
        let tableHeight = $("#" + SubstationList.mainTableElementId).parent().height() - 20;
        tableHeight = tableHeight < 200 ? 200 : tableHeight;
        //tableHeight = 588;
        isas.bootstrapTable({
            el: '#' + SubstationList.mainTableElementId,
            toolBarEl: '#' + toolBarId,
            url: AppServiceUrl.TransformerSubstation_FindDatas,
            //height: tableHeight,
            detailView: false,
            pageList: [5, 8, 10],
            pageSize: 5,
            toolbarAlign: 'right',
            theadClasses: 'thead-default',
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        SubstationName: $("#searchSubstationName_" + SubstationList.ParentGuid).val(),
                        PowerSupplyLineId: SubstationList.ParentGuid
                    },
                    sorting: "SubstationName"
                }
                return c
            },
            columns: [
                {
                    checkbox: true, // 显示一个勾选框
                    align: 'center', // 居中显示
                    valign: 'middle',
                    class: "checkbox checkbox-primary",
                    formatter: function (value, row, index) {
                        //let pageSize = $('#' + SubstationList.mainTableElementId).bootstrapTable("getOptions").pageSize;
                        //let pageNumber = $('#' + SubstationList.mainTableElementId).bootstrapTable("getOptions").pageNumber;
                        return value;
                    }
                },
                {
                    title: '行号',
                    align: 'center',
                    valign: 'middle',
                    width: 50,
                    formatter: function (value, row, index) {
                        let pageSize = $('#' + SubstationList.mainTableElementId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + SubstationList.mainTableElementId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'substationName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '变电站名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'commMgrIP',
                    title: '变电站的通信IP',
                    align: 'center',
                    valign: 'middle',
                    width: 200,
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
                },

                {
                    field: 'remark',
                    title: '备注',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    title: "操作",
                    align: 'center',
                    valign: 'middle',
                    width: 80, // 定义列的宽度，单位为像素px
                    formatter: function (value, row, index) {
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#editSubstationModal' onclick='SubstationList.initEditModalValues(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-pencil' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }

            ]
        })
    },
    initEditModalByVue: function (editModalId) {
        if (!editModalId) return;
        SubstationList.editModalVue = new Vue({
            el: '#' + editModalId,
            data: {
                id: null,
                header: '新增',
                substationName: "",
                commMgrIP: "",
                remark: "",
                isActive: true
            },
            methods: {
                saveSubstation: function (event) {
                    if (!SubstationList.mainTableElementId || !SubstationList.editFormId) return;
                    if (!$("#" + SubstationList.editFormId).valid()) return;
                    let data = {
                        id: this.id,
                        substationName: this.substationName,
                        commMgrIP: this.commMgrIP,
                        isActive: this.isActive,
                        powerSupplyLineId: SubstationList.ParentGuid,
                        remark: this.remark
                    }
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.TransformerSubstation_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    $('#' + SubstationList.mainTableElementId).bootstrapTable('refresh');
                                    $(SubstationList.editModalVue.$el).modal('hide');
                                }
                            }
                        }
                    });
                }
            }
        })
    },
    initEditFormValidate: function (editFormId) {
        if (!editFormId) return;
        SubstationList.editFormId = editFormId
        $("#" + editFormId).validate({
            rules: {
                substationName: {
                    required: true,
                },
                commMgrIP: {
                    //required: true,
                    validateip: true,
                }
            },
            messages: {
                substationName: {
                    required: "名称不能为空",
                },
                commMgrIP: {
                    //required: "IP不能为空",
                    validateIp: "IP地址无效",
                }
            }
        });

        $.validator.addMethod("validateip", function (value, element, params) {
            var reg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
            return this.optional(element) || reg.test(value);
        }, "ip地址错误！");
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + SubstationList.editFormId).validate().resetForm();
        $("#" + SubstationList.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    initEditModalValues: function (uniqueId) {
        if (!SubstationList.editModalVue || !SubstationList.mainTableElementId) return;
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + SubstationList.mainTableElementId).bootstrapTable('getRowByUniqueId', uniqueId);

        SubstationList.resetFormValidate()
        if (!rowData) {
            rowData = {
                id: null,
                substationName: "",
                commMgrIP: "",
                isActive: true,
                remark: null,
            }
            SubstationList.editModalVue.header = '新增';
        } else {
            SubstationList.editModalVue.header = '编辑';
        }
        SubstationList.editModalVue.id = rowData.id;
        SubstationList.editModalVue.substationName = rowData.substationName;
        SubstationList.editModalVue.commMgrIP = rowData.commMgrIP;
        SubstationList.editModalVue.isActive = rowData.isActive;
        SubstationList.editModalVue.remark = rowData.remark;
    },
    delStation: function () {
        if (!SubstationList.mainTableElementId) return;
        let row = $("#" + SubstationList.mainTableElementId).bootstrapTable('getSelections');
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
            url: AppServiceUrl.TransformerSubstation_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag)
                        $('#' + SubstationList.mainTableElementId).bootstrapTable('refresh');
                }
            },
        });
    }
}