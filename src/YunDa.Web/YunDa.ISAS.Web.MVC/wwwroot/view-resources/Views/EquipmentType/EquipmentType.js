$(document).ready(function () {
    equipmentTypeList.initListFunc();
});
var equipmentTypeList = {
    editModalVue: null,
    mainTableId: null,
    toolBarId: null,
    editModalId: null,
    editFormId: null,
    //初始化
    initListFunc: function () {
        equipmentTypeList.mainTableId = "equipmentTypeTable";
        equipmentTypeList.toolBarId = "equipmentTypeTableToolbar";
        equipmentTypeList.editModalId = "editEquipmentTypeModal";
        equipmentTypeList.editFormId = "editEquipmentTypeForm";
        equipmentTypeList.initComponent();
        equipmentTypeList.initEditModalByVue();
        equipmentTypeList.initEditFormValidate();
    },
    //初始化任务单个按钮事件
    initComponent: function () {
        $("#searchEquipmentTypeBtn").click(function () {
            equipmentTypeList.refreshTable();
        });
        $("#addEquipmentTypeBtn").click(function () {
            equipmentTypeList.initEditModalValues("");
            $(equipmentTypeList.editModalVue.$el).modal('show');
        });
        $("#delEquipmentTypeBtn").click(function () {
            equipmentTypeList.delFunc();
        });
        equipmentTypeList.initTable();
        equipmentTypeList.refreshTable();
    },
    //初始化清单
    initTable: function () {
        if (!equipmentTypeList.mainTableId) return;
        let tableHeight = $("#" + equipmentTypeList.mainTableId).parent().height();
        tableHeight = tableHeight < 200 ? 200 : tableHeight;
        isas.bootstrapTable({
            el: '#' + equipmentTypeList.mainTableId,
            toolBarEl: '#' + equipmentTypeList.toolBarId,
            url: AppServiceUrl.EquipmentType_FindDatas,
            //height: tableHeight,
            isInitData: false,
            //singleSelect: true,
            pageList: [16, 22, 28],
            pageSize: 16,
            onLoadSuccess: function () {
                //$("#" + equipmentTypeList.mainTableId).bootstrapTable('hideLoading');
            },
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        name: $("#searchEquipmentTypeName").val(),
                    },
                    sorting: "name"
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
                        let pageSize = $('#' + equipmentTypeList.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + equipmentTypeList.mainTableId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'name', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '类型名称', // 表格表头显示文字
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
                    title: "操作",
                    align: 'center',
                    valign: 'middle',
                    width: 80, // 定义列的宽度，单位为像素px
                    formatter: function (value, row, index) {
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#editEquipmentTypeModal' onclick='equipmentTypeList.initEditModalValues(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-pencil' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }
            ],
        });
    },
    //实例化Vue
    initEditModalByVue: function () {
        if (!equipmentTypeList.editModalId) return;
        equipmentTypeList.editModalVue = new Vue({
            el: '#' + equipmentTypeList.editModalId,
            data: {
                header: '添加',
                id: null,
                equipmentTypeName: "",
                isActive: true,
            },
            created: function () {
            },
            watch: {
            },
            methods: {
                save: function (event) {
                    if (!equipmentTypeList.mainTableId || !equipmentTypeList.editFormId) return;
                    if (!$("#" + equipmentTypeList.editFormId).valid()) return;
                    let data = {
                        id: this.id,
                        name: this.equipmentTypeName,
                        isActive: this.isActive,
                    };
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.EquipmentType_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    equipmentTypeList.refreshTable();
                                    $(equipmentTypeList.editModalVue.$el).modal('hide');
                                }
                            }
                        }
                    });
                },
                closeModal: function () {
                },
                setEquipmentTypeName: function (para) {
                    this.equipmentTypeName = para
                },
            },
            mounted: function () {
            },
            created: function () {
            },
        })
    },
    //编辑表单的Form验证
    initEditFormValidate: function () {
        if (!equipmentTypeList.editFormId) return;
        $("#" + equipmentTypeList.editFormId).validate({
            rules: {
                equipmentTypeName: {
                    required: true,
                }
            },
            messages: {
                equipmentTypeName: {
                    required: "设备类型名称不能为空",
                }
            }
        });
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + equipmentTypeList.editFormId).validate().resetForm();
        $("#" + equipmentTypeList.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    //初始化编辑表单各值
    initEditModalValues: function (uniqueId) {
        if (!equipmentTypeList.editModalVue || !equipmentTypeList.mainTableId) return;

        var rowData = null;
        if (uniqueId)
            rowData = $('#' + equipmentTypeList.mainTableId).bootstrapTable('getRowByUniqueId', uniqueId);

        equipmentTypeList.resetFormValidate();
        if (!rowData) {
            rowData = {
                id: null,
                equipmentTypeName: "",
                isActive: true,
            }
            equipmentTypeList.editModalVue.header = '添加';
        } else
            equipmentTypeList.editModalVue.header = '编辑';

        //设置记录单其他参数
        equipmentTypeList.editModalVue.id = rowData.id;
        equipmentTypeList.editModalVue.equipmentTypeName = rowData.name;
        equipmentTypeList.editModalVue.isActive = rowData.isActive;
    },
    delFunc: function () {
        let row = equipmentTypeList.getSelectItem();
        if (!row || row.length == 0) {
            layer.alert("请选择要删除的行！");
            return;
        }
        let arrIds = new Array();
        row.forEach(r => arrIds.push(r.id));
        isas.ajax({
            type: "post",
            //请求地址
            url: AppServiceUrl.EquipmentType_DeleteByIds,
            //数据，json字符串
            //data: JSON.stringify(isSingleRow ? { id: row[0].id } : arrIds),
            data: JSON.stringify(arrIds),

            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        equipmentTypeList.refreshTable();
                        layer.alert(rst.result.message);
                    }
                } else if (!rst.success)
                    layer.alert(rst.error.message);
            },
        });
    },
    refreshTable: function () {
        $('#' + equipmentTypeList.mainTableId).bootstrapTable('refresh');
    },
    getSelectItem: () => $("#" + equipmentTypeList.mainTableId).bootstrapTable('getSelections'),
}