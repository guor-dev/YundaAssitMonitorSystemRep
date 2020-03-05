$(document).ready(function () {
    substationTree.initTree(subTreeChanged);
    equipmentInfoList.initListFunc();
    getBasedata();
});
function subTreeChanged(node) {
    equipmentInfoList.refreshTable();
}
function getBasedata(vueBasedata) {
    isas.ajax({
        isHideSuccessMsg: true,
        //请求地址
        url: AppServiceUrl.EquipmentType_FindEquipmentTypeForSelect,
        //数据，json字符串
        data: JSON.stringify(
            {
                //searchCondition: { isOnlyActive: true }
            }),
        //请求成功
        success: function (rst) {
            if (rst.result) {
                if (rst.result.flag) {
                    equipmentInfoList.editModalVue.typeValues = rst.result.resultDatas;
                }
            }
        }
    });
    //请求厂商
    isas.ajax({
        isHideSuccessMsg: true,
        //请求地址
        url: AppServiceUrl.ManufacturerInfo_FindManufacturerInfoForSelect,
        //数据，json字符串
        data: JSON.stringify(
            {
                //searchCondition: { isOnlyActive: true }
            }
        ),
        //请求成功
        success: function (rst) {
            if (rst.result) {
                if (rst.result.flag) {
                    equipmentInfoList.editModalVue.manufacturerValues = rst.result.resultDatas;
                }
            }
        }
    });
}

var equipmentInfoList = {
    editModalVue: null,
    mainTableId: null,
    toolBarId: null,
    editModalId: null,
    editFormId: null,

    initListFunc: function () {
        equipmentInfoList.mainTableId = "equipmentInfoTable";
        equipmentInfoList.toolBarId = "equipmentInfoTableToolbar";
        equipmentInfoList.editModalId = "editEquipmentInfoModal";
        equipmentInfoList.editFormId = "editequipmentInfoForm";
        equipmentInfoList.initComponent();
        equipmentInfoList.initEditModalByVue();
    },
    initComponent: function () {
        $("#searchEquipmentInfoBtn").click(function () {
            equipmentInfoList.refreshTable();
        });
        $("#addEquipmentInfoBtn").click(function () {
            let subNode = substationTree.getSelectSubstationNode();
            if (!subNode) {
                layer.alert("请选择变电所！");
                return;
            }
            equipmentInfoList.initEditFormValidate();
            equipmentInfoList.initEditModalValues("");
        });
        $("#delEquipmentInfoBtn").click(function () {
            equipmentInfoList.delCard();
        });
        equipmentInfoList.initTable();
    },
    initTable: function () {
        if (!equipmentInfoList.mainTableId) return;
        let tableHeight = $("#" + equipmentInfoList.mainTableId).parent().height();
        tableHeight = tableHeight < 200 ? 200 : tableHeight;
        isas.bootstrapTable({
            el: '#' + equipmentInfoList.mainTableId,
            toolBarEl: '#' + equipmentInfoList.toolBarId,
            url: AppServiceUrl.EquipmentInfo_FindDatas,
            //height: tableHeight,
            isInitData: false,
            singleSelect: true,
            pageList: [8, 12, 16],
            pageSize: 8,
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let node = substationTree.getSelectSubstationNode();
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        Name: $("#searchEquipmentInfoName").val(),
                        transformerSubstationId: node ? node.id : null,
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
                        let pageSize = $('#' + equipmentInfoList.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + equipmentInfoList.mainTableId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'name', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'code', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备编码', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'installationDate', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '安装时间', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        if (value) {
                            let date = new Date(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        return null;
                    }
                },
                {
                    field: 'productionDate', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '出厂时间', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        if (value) {
                            let date = new Date(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        return null;
                    }
                },
                {
                    field: 'equipmentType', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备类型', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        if (value) {
                            return value.name;
                        }
                    }
                },
                {
                    field: 'manufacturerInfo', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备厂商', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        if (value) {
                            return value.manufacturerName;
                        }
                    }
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
                    field: 'remark', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '备注', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    title: "操作",
                    align: 'center',
                    valign: 'middle',
                    width: 80, // 定义列的宽度，单位为像素px
                    formatter: function (value, row, index) {
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#" + equipmentInfoList.editModalId + "' onclick='equipmentInfoList.initEditModalValues(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-pencil' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }
            ],
        });
    },
    //实例化编辑任务单的Vue
    initEditModalByVue: function () {
        if (!equipmentInfoList.editModalId) return;
        equipmentInfoList.editModalVue = new Vue({
            el: '#' + equipmentInfoList.editModalId,
            data: {
                header: '',
                id: null,
                equipmentInfoName: "",
                equipmentInfoCode: "",
                format: "",
                language: "",
                installDateValue: null,
                produceDateValue: null,
                typeValue: null,
                manufacturerValue: null,
                typeValues: [],
                manufacturerValues: [],
                remark: "",
                isActive: true,
            },
            created: function () {
            },
            watch: {
            },
            methods: {
                getEqTypeMFInfo: function () {
                    this.typeValues = equipmentInfoList.vueBasedata.typeValues;
                    this.manufacturerValues = equipmentInfoList.vueBasedata.manufacturerValues;
                },
                save: function (event) {
                    if (!equipmentInfoList.mainTableId || !equipmentInfoList.editFormId) return;
                    if (!$("#" + equipmentInfoList.editFormId).valid()) return;
                    let node = substationTree.getSelectSubstationNode();
                    if (node == null) return;
                    let data = {
                        id: this.id,
                        //seqNo: this.seqNo,
                        name: this.equipmentInfoName,
                        code: this.equipmentInfoCode,
                        installationDate: this.installDateValue,
                        productionDate: this.produceDateValue,
                        equipmentTypeId: this.typeValue,
                        manufacturerInfoId: this.manufacturerValue,
                        transformerSubstationId: node.id,
                        remark: this.remark,
                        isActive: this.isActive,
                    }
                    //console.log(data)
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.EquipmentInfo_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    equipmentInfoList.refreshTable();
                                    $(equipmentInfoList.editModalVue.$el).modal('hide');
                                }
                            }
                        }
                    });
                },
                closeModal: function () {
                    equipmentInfoList.clearModalSelect();
                },
                setInstallDateValue: function (para) {
                    this.installDateValue = para
                },
                setProduceDateValue: function (para) {
                    this.produceDateValue = para
                },
                setTypeAction: function (para) {
                    this.typeValue = para
                },
                setManufacturerAction: function (para) {
                    this.manufacturerValue = para
                },
            },
            mounted: function () {
            }
        })
    },
    //编辑任务单的Form验证
    initEditFormValidate: function () {
        if (!equipmentInfoList.editFormId) return;
        $("#" + equipmentInfoList.editFormId).validate({
            ignore: ":hidden:not(select)",//解决无法校验select
            rules: {
                name: {
                    required: true,
                },
                typeValue: {
                    required: true,
                }
            },
            messages: {
                name: {
                    required: "设备名称不能为空",
                },
                typeValue: {
                    required: "请选择设备类型",
                }
            }
        });
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + equipmentInfoList.editFormId).validate().resetForm();
        $("#" + equipmentInfoList.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    //初始化编辑任务单各值
    initEditModalValues: function (uniqueId) {
        if (!equipmentInfoList.editModalVue || !equipmentInfoList.mainTableId) return;
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + equipmentInfoList.mainTableId).bootstrapTable('getRowByUniqueId', uniqueId);
        equipmentInfoList.resetFormValidate();
        //"yyyy-MM-DD"
        if (!rowData) {
            rowData = {
                id: null,
                name: "",
                code: "",
                installationDate: null,
                productionDate: null,
                equipmentType: { id: null },
                manufacturerInfo: { id: null },
                remark: "",
                isActive: true,
            }
            equipmentInfoList.editModalVue.header = '添加';
        } else
            equipmentInfoList.editModalVue.header = '编辑';

        //console.log(rowData)
        //设置记录单其他参数
        equipmentInfoList.editModalVue.id = rowData.id;
        equipmentInfoList.editModalVue.equipmentInfoName = rowData.name;
        equipmentInfoList.editModalVue.equipmentInfoCode = rowData.code;

        equipmentInfoList.editModalVue.installDateValue =(new Date(rowData.installationDate)).Format("yyyy-MM-dd");
        equipmentInfoList.editModalVue.produceDateValue = (new Date(rowData.productionDate)).Format("yyyy-MM-dd");
        equipmentInfoList.editModalVue.typeValue = rowData.equipmentType.id;
        equipmentInfoList.editModalVue.manufacturerValue = rowData.manufacturerInfo.id;
        equipmentInfoList.editModalVue.remark = rowData.remark;
        equipmentInfoList.editModalVue.isActive = rowData.isActive;
    },
    delCard: function () {
        let row = equipmentInfoList.getSelectItem();
        if (!row || row.length == 0) {
            layer.alert("请选择要删除的行！");
            return;
        };
        let arrIds = new Array();
        row.forEach(r => arrIds.push(r.id));
        isas.ajax({
            //请求地址
            url: AppServiceUrl.EquipmentInfo_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(arrIds),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        equipmentInfoList.refreshTable();
                    }
                    layer.alert(rst.result.message);
                } else if (!rst.success)
                    layer.alert(rst.error.message);
            },
        });
    },
    refreshTable: function () {
        $('#' + equipmentInfoList.mainTableId).bootstrapTable('refresh');
    },
    getSelectItem: () => $("#" + equipmentInfoList.mainTableId).bootstrapTable('getSelections'),
}