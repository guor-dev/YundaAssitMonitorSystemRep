$(function () {
    sysFunctionList.initEditModalByVue("editSysFunctionModal");
    sysFunctionTree.initTree(subTreeChanged);
    sysFunctionList.initEditFormValidate();
    $("#searchSysFunctionBtn").click(function () {
        sysFunctionList.refreshTable();
    });
    $("#addSysFunctionBtn").click(function () {
        sysFunctionList.initEditModalValues("");
    });
    $("#delSysFunctionBtn").click(function () {
        sysFunctionList.delFoo();
    });
    sysFunctionList.initTable("tableSysFunction", "tableToolbar");
});
function subTreeChanged(node) {
    sysFunctionList.parentNodeId = node.id;
    sysFunctionList.refreshTable();
}
var sysFunctionList = {
    mainTableElementId: null,
    editModalVue: null,
    editFormId: "editForm",
    parentNodeId: null,
    initTable: function (tableId, toolBarId) {
        sysFunctionList.mainTableElementId = tableId;
        if (!sysFunctionList.mainTableElementId) return;
        isas.bootstrapTable({
            el: '#' + sysFunctionList.mainTableElementId,
            toolBarEl: '#' + toolBarId,
            url: AppServiceUrl.Function_FindDatas,
            toolbarAlign: 'left',
            showToggle: 'true',
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        name: $("#searchSysFunctionName").val(),
                        code: $("#searchSysFunctionCode").val(),
                        id: sysFunctionList.parentNodeId,
                    },
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
                        return value;
                    }
                },
                {
                    title: '行号',
                    align: 'center',
                    valign: 'middle',
                    width: 50,
                    formatter: function (value, row, index) {
                        let pageSize = $('#' + sysFunctionList.mainTableElementId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + sysFunctionList.mainTableElementId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'name', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '功能名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'code',
                    title: '功能编码',
                    align: 'center',
                    valign: 'middle'
                },
                //{
                //    field: 'type',
                //    title: '功能类别',
                //    align: 'center',
                //    valign: 'middle',
                //    formatter: function (value, row, index) {
                //        let type = sysFunctionList.editModalVue.types.find(item => parseInt(item.value) == value);
                //        if (type) {
                //            return type.text;
                //        }
                //        return "错误"
                //    }
                //},
                {
                    field: 'loadUrl',
                    title: '功能加载链接',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return value;
                    }
                },
                {
                    field: 'icon',
                    title: '功能显示图标',
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
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#editSysFunctionModal' onclick='sysFunctionList.initEditModalValues(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-pencil' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }
            ]
        });
    },
    initEditModalByVue: function (editModalId) {
        if (!editModalId) return;
        sysFunctionList.editModalVue = new Vue({
            el: '#' + editModalId,
            data: {
                id: null,
                header: '编辑',
                name: "",
                code: "",
                type: "",
                types: [],
                loadUrl: "",
                icon: "",
                remark: "",
                isActive: true
            },
            created: function () {
                let _this = this;

                isas.ajax({
                    //请求地址
                    url: AppServiceUrl.Function_FindTypesForSelect,
                    //数据，json字符串
                    data: null,
                    isHideSuccessMsg: true,

                    //请求成功
                    success: function (rst) {
                        if (rst.result) {
                            if (rst.result.flag) {
                                _this.types = rst.result.resultDatas;
                            }
                        }
                    }
                })
            },
            methods: {
                setType: function (param) {
                    console.log(param);
                    this.type = param;
                },
                save: function (event) {
                    if (!sysFunctionList.mainTableElementId || !sysFunctionList.editFormId) return;
                    if (!$("#" + sysFunctionList.editFormId).valid()) return;
                    let data = {
                        //id: this.id,
                        name: this.name,
                        code: this.code,
                        type: parseInt(this.type),
                        loadUrl: this.loadUrl,
                        icon: this.icon,
                        remark: this.remark,
                        isActive: this.isActive,
                        sysFunctionId:sysFunctionList.parentNodeId,
                    }
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.Function_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    $(sysFunctionList.editModalVue.$el).modal('hide');
                                    sysFunctionTree.initTree(subTreeChanged);
                                    sysFunctionList.refreshTable();
                                }
                            }
                        }
                    })
                }
            }
        })
    },
    initEditFormValidate: function () {
        if (!sysFunctionList.editFormId) return;
        $("#" + sysFunctionList.editFormId).validate({
            ignore: ":hidden:not(select)",//解决无法校验select
            rules: {
                name: {
                    required: true,
                },
                code: {
                    required: true,
                },
                type: {
                    required: true,
                },
            },
            messages: {
                name: {
                    required: "名称不能为空",
                },
                code: {
                    required: "编码不能为空",
                },
                type: {
                    required: "请选择功能类别",
                },
            }
        });
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + sysFunctionList.editFormId).validate().resetForm();
        $("#" + sysFunctionList.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    initEditModalValues: function (uniqueId) {
        if (!sysFunctionList.editModalVue || !sysFunctionList.mainTableElementId) return;
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + sysFunctionList.mainTableElementId).bootstrapTable('getRowByUniqueId', uniqueId);
        sysFunctionList.resetFormValidate()
        if (!rowData) {
            rowData = {
                id: null,
                name: "",
                code: "",
                type: "",
                loadUrl: "",
                icon: "",
                remark: "",
                isActive: true
            }
            sysFunctionList.editModalVue.header = '添加';
        } else
            sysFunctionList.editModalVue.header = '编辑';

        sysFunctionList.editModalVue.id = rowData.id;
        sysFunctionList.editModalVue.name = rowData.name;
        sysFunctionList.editModalVue.code = rowData.code;
        sysFunctionList.editModalVue.type = rowData.type;
        sysFunctionList.editModalVue.loadUrl = rowData.loadUrl;
        sysFunctionList.editModalVue.icon = rowData.icon;
        sysFunctionList.editModalVue.remark = rowData.remark;
        sysFunctionList.editModalVue.isActive = rowData.isActive;
    },
    delFoo: function () {
        if (!sysFunctionList.mainTableElementId) return;
        let row = $("#" + sysFunctionList.mainTableElementId).bootstrapTable('getSelections');
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
            url: AppServiceUrl.Function_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag)
                        sysFunctionTree.initTree(subTreeChanged);
                        sysFunctionList.refreshTable();
                }
            },
        });
    },
    refreshTable: function () {
        $('#' + sysFunctionList.mainTableElementId).bootstrapTable('refresh');
    }
}