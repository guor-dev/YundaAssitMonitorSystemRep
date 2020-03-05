$(document).ready(function () {
    setPageBaseData();
    //初始化树形结构
    substationTree.initTree(subTreeChanged);
    //初始化巡检单
    inspectionCard.initInspectionCard();
    //初始化巡检任务项
    inspectionItem.initInspectionItem();
    //初始化任务计划
    inspectionPlanTask.initPlanTask();
});
function setPageBaseData() {
    isas.ajax({
        isHideSuccessMsg: true,
        //请求地址
        url: AppServiceUrl.InspectionItem_FindProcessActionsForSelect,
        //数据，json字符串
        data: "",
        async: false,
        isHideLoad: true,
        //请求成功
        success: function (rst) {
            if (rst.result) {
                if (rst.result.flag) {
                    inspectionItem.baseProcessActions = rst.result.resultDatas;
                }
            }
        }
    });
}
var baseTransformerSubstationId = "";
function subTreeChanged(node) {
    if (!node) return;
    baseTransformerSubstationId = node.id;
    //刷新巡检任务单列表
    inspectionCard.refreshTable();
    isas.ajax({
        isHideSuccessMsg: true,
        //请求地址
        url: AppServiceUrl.EquipmentType_FindEquipmentTypeForSelect,
        //数据，json字符串
        data: JSON.stringify({}),
        async: true,
        //请求成功
        success: function (rst) {
            if (rst.result) {
                if (rst.result.flag) {
                    inspectionItem.baseEquipmentTypes = rst.result.resultDatas;
                }
            }
        }
    });
    //获取变电所下的所有硬盘录像机摄像头
    c = {
        searchCondition: {
            transformerSubstationId: baseTransformerSubstationId,
            isVideoTerminal: true,
            isNeedChildren: false
        },
        sorting: "devName"
    }
    isas.ajax({
        isHideSuccessMsg: true,
        //请求地址
        url: AppServiceUrl.VideoDev_FindVideoDevForSelect,
        //数据，json字符串
        data: JSON.stringify(c),
        async: true,
        //请求成功
        success: function (rst) {
            if (rst.result) {
                if (rst.result.flag) {
                    inspectionItem.baseVideoTerminals = rst.result.resultDatas;
                }
            }
        }
    });
}

var inspectionCard = {
    editModalVue: null,
    mainTableId: null,
    toolBarId: null,
    editModalId: null,
    editFormId: null,
    //初始化任务单
    initInspectionCard: function () {
        inspectionCard.mainTableId = "cardTable";
        inspectionCard.toolBarId = "cardTableToolbar";
        inspectionCard.editModalId = "editCardModal";
        inspectionCard.editFormId = "editCardForm";
        inspectionCard.initComponent();
        inspectionCard.initEditModalByVue();
        inspectionCard.initEditFormValidate();
    },
    //初始化任务单个按钮事件
    initComponent: function () {
        $("#searchCardBtn").click(function () {
            inspectionCard.refreshTable();
        });
        $("#addCardBtn").click(function () {
            let subNode = substationTree.getSelectSubstationNode();
            if (!subNode) {
                layer.alert("请选择变电所！");
                return;
            }
            inspectionCard.initEditModalValues("");
            $(inspectionCard.editModalVue.$el).modal('show');
        });
        $("#delCardBtn").click(function () {
            inspectionCard.delCard();
        });
        $("#cardChildrenTab").on("show.bs.tab", function (e) {
            let tabTarget = $(e.target).attr('href');
            if (tabTarget == "#inspectionItem") {
                inspectionItem.refreshTable();
            } else {
                inspectionPlanTask.refreshData();
            }
        }).on("hide.bs.tab", function (e) {
            let tabTarget = $(e.target).attr('href');
            if (tabTarget == "#inspectionItem") {
                inspectionItem.removeAllData();
            } else {
                inspectionPlanTask.removeAllData();
            }
        })
        inspectionCard.initTable();
    },
    //初始化任务单表
    initTable: function () {
        if (!inspectionCard.mainTableId) return;
        //let tableHeight = $("#" + inspectionCard.mainTableId).parent().height();
        //tableHeight = tableHeight < 200 ? 200 : tableHeight;
        //$("#" + inspectionCard.mainTableId).bootstrapTable();
        isas.bootstrapTable({
            el: '#' + inspectionCard.mainTableId,
            toolBarEl: '#' + inspectionCard.toolBarId,
            url: AppServiceUrl.InspectionCard_FindDatas,
            //height: tableHeight,
            isInitData: false,
            singleSelect: true,
            pageList: [4, 8, 12],
            pageSize: 4,
            onLoadSuccess: function () {
                //$("#" + inspectionCard.mainTableId).bootstrapTable('hideLoading');
            },
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let node = substationTree.getSelectSubstationNode();
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        cardName: $("#searchCardName").val(),
                        transformerSubstationId: node ? node.id : null,
                        IsNeedChildren: true
                    },
                    sorting: "cardName"
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
                        let pageSize = $('#' + inspectionCard.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + inspectionCard.mainTableId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'transformerSubstation', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '变电所', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) { // 单元格格式化函数
                        return value ? value.substationName : "-";
                    }
                },
                {
                    field: 'cardName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '巡检任务单', // 表格表头显示文字
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
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#editCardModal' onclick='inspectionCard.initEditModalValues(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-pencil' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }
            ],
            onCheck: function (row, $element) {
                inspectionCard.refreshCardChildren();
            },
            onUncheck: function (row, $element) {
                inspectionCard.cleanCardChildren();
            }
        });
    },
    //实例化编辑任务单的Vue
    initEditModalByVue: function () {
        if (!inspectionCard.editModalId) return;
        inspectionCard.editModalVue = new Vue({
            el: '#' + inspectionCard.editModalId,
            data: {
                header: '编辑视频巡检清单',
                id: null,
                cardName: "",
                remark: "",
                isActive: true,
                powerSupplyLineId: null,
                transformerSubstationId: null,
                transformerSubstationName: null
            },
            watch: {
            },
            methods: {
                saveCard: function (event) {
                    if (!inspectionCard.mainTableId || !inspectionCard.editFormId) return;
                    if (!$("#" + inspectionCard.editFormId).valid()) return;
                    let data = {
                        id: this.id,
                        cardName: this.cardName,
                        remark: this.remark,
                        isActive: this.isActive,
                        transformerSubstationId: this.transformerSubstationId,
                    }
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.InspectionCard_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    inspectionCard.refreshTable();
                                    $(inspectionCard.editModalVue.$el).modal('hide');
                                }
                            }
                        }
                    });
                }
            }
        });
    },
    //编辑任务单的Form验证
    initEditFormValidate: function () {
        if (!inspectionCard.editFormId) return;
        $("#" + inspectionCard.editFormId).validate({
            rules: {
                cardName: {
                    required: true,
                }
            },
            messages: {
                cardName: {
                    required: "视频巡检任务单名称不能为空",
                },
                transformerSubstationId: {
                    required: "请选择变电所不能为空",
                }
            }
        });
    },
    //初始化编辑任务单各值
    initEditModalValues: function (uniqueId) {
        if (!inspectionCard.editModalVue || !inspectionCard.mainTableId) return;
        let subNode = substationTree.getSelectSubstationNode();

        if (!subNode) {
            layer.alert("请选择变电所！");
            return;
        }
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + inspectionCard.mainTableId).bootstrapTable('getRowByUniqueId', uniqueId);
        if (!rowData) {
            rowData = {
                id: null,
                cardName: "",
                remark: "",
                isActive: true,
                powerSupplyLineId: null,
            }
            inspectionCard.editModalVue.header = '添加巡检任务单';
        } else
            inspectionCard.editModalVue.header = '编辑巡检任务单';

        //设置记录单其他参数
        inspectionCard.editModalVue.id = rowData.id;
        inspectionCard.editModalVue.cardName = rowData.cardName;
        inspectionCard.editModalVue.remark = rowData.remark;
        inspectionCard.editModalVue.isActive = rowData.isActive;
        //设置选中的变电所
        inspectionCard.editModalVue.transformerSubstationId = subNode.id;
        inspectionCard.editModalVue.transformerSubstationName = subNode.text;
    },
    delCard: function () {
        let row = inspectionCard.getSelectCardSingle();
        if (!row || row.length == 0) {
            layer.alert("请选择要删除的行！");
            return;
        }
        isas.ajax({
            confirm: true,
            type: "get",
            //请求地址
            url: AppServiceUrl.InspectionCard_DeleteById,
            //数据，json字符串
            data: { id: row.id },
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        inspectionCard.refreshTable();
                    }
                }
            },
        });
    },
    getSelectCard: function () {
        if (!inspectionCard.mainTableId) return null;
        return $("#" + inspectionCard.mainTableId).bootstrapTable('getSelections');
    },
    getSelectCardSingle: function () {
        if (!inspectionCard.mainTableId) return null;
        let rows = $("#" + inspectionCard.mainTableId).bootstrapTable('getSelections');
        return !rows || rows.length == 0 ? null : rows[0];
    },
    refreshTable: function () {
        $('#' + inspectionCard.mainTableId).bootstrapTable('refresh');
        inspectionCard.cleanCardChildren();
    },
    refreshCardChildren: function () {
        var $tabs = $('#cardChildrenTab').children('li');
        $tabs.each(function () {
            $tab = $(this);
            if ($tab.hasClass('active')) {
                let tabTarget = $tab.find('a').attr('href');
                if (tabTarget == "#inspectionItem") {
                    inspectionItem.refreshTable();
                } else {
                    inspectionPlanTask.refreshData();
                }
            }
        });
    },
    cleanCardChildren: function () {
        inspectionItem.removeAllData();
        inspectionPlanTask.removeAllData();
    }
}
var inspectionItem = {
    editModalVue: null,
    mainTableId: null,
    toolBarId: null,
    editModalId: null,
    editFormId: null,
    baseProcessActions: [],
    baseEquipmentTypes: [],
    baseVideoTerminals: [],
    tableHeight: 200,
    //初始化任务单
    initInspectionItem: function () {
        inspectionItem.mainTableId = "itemTable";
        inspectionItem.toolBarId = "itemTableToolbar";
        inspectionItem.editModalId = "editItemModal";
        inspectionItem.editFormId = "editItemForm";
        inspectionItem.initEditModalByVue();
        inspectionItem.initEditFormValidate();
        inspectionItem.initComponent();
    },
    //初始化任务单个按钮事件
    initComponent: function () {
        $("#searchItemBtn").click(function () {
            inspectionItem.refreshTable();
        });
        $("#addItemBtn").click(function () {
            let selecCard = inspectionCard.getSelectCardSingle();
            if (!selecCard) {
                layer.alert("请选择巡检任务单！");
                return;
            }
            inspectionItem.initEditModalValues("");
            $(inspectionItem.editModalVue.$el).modal('show');
        });
        $("#delItemBtn").click(function () {
            inspectionItem.delItem();
        });
        inspectionItem.initTable();
    },
    //初始化任务单表
    initTable: function () {
        if (!inspectionItem.mainTableId) return;
        //inspectionItem.tableHeight = $("#cardChildrenDiv").height() - 70;
        //inspectionItem.tableHeight = inspectionItem.tableHeight < 200 ? 200 : inspectionItem.tableHeight;
        isas.bootstrapTable({
            el: '#' + inspectionItem.mainTableId,
            toolBarEl: '#' + inspectionItem.toolBarId,
            url: AppServiceUrl.InspectionItem_FindDatas,
            //height: inspectionItem.tableHeight,
            isInitData: false,
            singleSelect: false,
            pageList: [4, 8, 12],
            pageSize: 4,
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let card = inspectionCard.getSelectCardSingle();
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        itemName: $("#searchItemName").val(),
                        InspectionCardId: card ? card.id : null,
                        IsOnlyActive: false
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
                        let pageSize = $('#' + inspectionItem.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + inspectionItem.mainTableId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'seqNo', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '序号', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    width: 50,
                    visible: true,
                },
                {
                    field: 'itemName',
                    title: '任务项',
                    align: 'center',
                    valign: 'middle',
                    visible: true
                },
                {
                    field: 'videoDev',
                    title: '摄像头',
                    align: 'center',
                    valign: 'middle',
                    visible: true,
                    formatter: function (value, row, index) {
                        return value ? value.devName : "-";
                    }
                },
                {
                    field: 'presetPoint',
                    title: '预置点',
                    align: 'center',
                    valign: 'middle',
                    visible: true,
                    formatter: function (value, row, index) {
                        return value ? value.name : "-";
                    }
                },
                {
                    field: 'processAction',
                    title: '巡视动作',
                    align: 'center',
                    valign: 'middle',
                    visible: true,
                    formatter: function (value, row, index) { // 单元格格式化函数
                        let text = "-";
                        inspectionItem.baseProcessActions.forEach(function (a) {
                            if (value == a.value) {
                                text = a.text;
                            }
                        });
                        return text;
                    }
                },
                {
                    field: 'processDuration', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '持续时间', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) { // 单元格格式化函数
                        //value： 该列的字段值；
                        //row： 这一行的数据对象；
                        // index： 行号，第几行，从0开始计算
                        var text = '-';
                        if (value) {
                            text = value + "秒";
                        }
                        return text;
                    }
                },
                {
                    field: 'isImageRecognition',
                    title: '图像识别',
                    align: 'center',      //设置单元格数据的左右对齐方式， 可选择的值有：’left’, ‘right’, ‘center’
                    valign: 'middle',    //设置单元格数据的上下对齐方式， 可选择的值有：’top’, ‘middle’, ‘bottom’
                    width: 80,
                    formatter: function (value, row, index) { // 单元格格式化函数
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
                    }
                },
                {
                    field: 'isActive',
                    title: '状态',
                    align: 'center',      //设置单元格数据的左右对齐方式， 可选择的值有：’left’, ‘right’, ‘center’
                    valign: 'middle',    //设置单元格数据的上下对齐方式， 可选择的值有：’top’, ‘middle’, ‘bottom’
                    width: 80,
                    formatter: function (value, row, index) { // 单元格格式化函数
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
                    }
                },
                {
                    field: 'remark',
                    title: '备注',
                    align: 'center',
                    valign: 'middle',
                    visible: true,
                    formatter: function (value, row, index) {
                        return value ? value : "-";
                    }
                },
                {
                    title: "操作",
                    align: 'center',
                    valign: 'middle',
                    width: 80, // 定义列的宽度，单位为像素px
                    formatter: function (value, row, index) {
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#editItemModal' onclick='inspectionItem.initEditModalValues(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-pencil' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }
            ]
        });
    },
    //实例化编辑任务单的Vue
    initEditModalByVue: function () {
        if (!inspectionItem.editModalId) return;
        inspectionItem.editModalVue = new Vue({
            el: '#' + inspectionItem.editModalId,
            data: {
                header: '编辑视频巡检项',
                id: null,
                seqNo: -1,
                itemName: "",
                processAction: "",
                processDuration: 5,
                isImageRecognition: false,
                remark: "",
                isActive: true,
                inspectionCardId: null,
                inspectionCardName: "",
                equipmentTypeId: null,
                equipmentInfoId: null,
                videoDevId: null,
                presetPointId: null,
                processActions: [],
                equipmentTypes: [],
                equipmentInfos: [],
                videoTerminals: [],
                presetPoints: [],
                placeholderForProcessAction: "请选择巡视动作",
                placeholderForEquipmentType: "请选择设备类型",
                placeholderForEquipmentInfo: "请选择设备",
                placeholderForVideoDevId: "请选择摄像头",
                placeholderForPresetPoint: "请选择预置点"
            },
            created: function () {
            },
            watch: {
                equipmentTypeId: function (newValue, oldValue) {
                    if (newValue != oldValue) {
                        //若变更硬盘录像机，则更新摄像头下拉下拉选项内容
                        this.equipmentInfoId = null;
                        this.setEquipmentInfoSelect(newValue);
                        //$("#equipmentTypeId").val(newValue);
                    }
                },
                equipmentInfoId: function (newValue, oldValue) {
                    if (newValue != oldValue) {
                        //若变更硬盘录像机，则更新摄像头下拉下拉选项内容
                        this.videoDevId = null;
                        this.presetPointId = null;
                        this.setVideoTerminalSelect(newValue);
                        //$("#equipmentInfoId").val(newValue);
                    }
                },
                videoDevId: function (newValue, oldValue) {
                    if (newValue != oldValue)
                        //若变更摄像头，则更新预置点下拉下拉选项内容
                        this.setPresetPointSelect(newValue);
                },
            },
            methods: {
                saveItem: function (event) {
                    if (!inspectionItem.mainTableId || !inspectionItem.editFormId) return;
                    if (!$("#" + inspectionItem.editFormId).valid()) return;
                    let data = {
                        id: this.id,
                        seqNo: this.seqNo,
                        itemName: this.itemName,
                        processAction: this.processAction,
                        processDuration: this.processDuration,
                        isImageRecognition: this.isImageRecognition,
                        remark: this.remark,
                        isActive: this.isActive,
                        inspectionCardId: this.inspectionCardId,
                        videoDevId: this.videoDevId,
                        presetPointId: this.presetPointId
                    }
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.InspectionItem_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    inspectionItem.refreshTable();
                                    $(inspectionItem.editModalVue.$el).modal('hide');
                                }
                            }
                            inspectionItem.clearModalSelect();
                        }
                    });
                },
                closeModal: function () {
                    inspectionItem.clearModalSelect();
                },
                setProcessAction: function (v) {
                    this.processAction = v ? parseInt(v) : null;
                },
                setEquipmentTypeId: function (v) {
                    this.equipmentTypeId = v ? v : null;
                },
                setEquipmentInfoId: function (v) {
                    this.equipmentInfoId = v ? v : null;
                },
                setVideoDevId: function (v) {
                    this.videoDevId = v ? v : null;
                },
                setPresetPointId: function (v) {
                    this.presetPointId = v ? v : null;
                },
                setEquipmentInfoSelect: function (typeId) {
                    if (!typeId) {
                        inspectionItem.editModalVue.equipmentInfos = [];
                        return;
                    }
                    let c = {
                        searchCondition: {
                            equipmentTypeId: typeId,
                            transformerSubstationId: baseTransformerSubstationId
                        }
                    }
                    isas.ajax({
                        isHideSuccessMsg: true,
                        //请求地址
                        url: AppServiceUrl.EquipmentInfo_FindEquipmentInfoForSelect,
                        //数据，json字符串
                        data: JSON.stringify(c),
                        async: true,
                        //请求成功
                        success: function (rst) {
                            if (rst.result && rst.result.flag)
                                inspectionItem.editModalVue.equipmentInfos = rst.result.resultDatas;
                        }
                    });
                },
                setVideoTerminalSelect: function (eId) {
                    if (!eId) {
                        inspectionItem.editModalVue.videoTerminals = inspectionItem.baseVideoTerminals;
                        return;
                    }
                    let c = {
                        searchCondition: {
                            equipmentInfoId: eId,
                            transformerSubstationId: baseTransformerSubstationId
                        }
                    }
                    isas.ajax({
                        isHideSuccessMsg: true,
                        //请求地址
                        url: AppServiceUrl.VideoDevEquipmentInfo_FindVideoTerminalForSelect,
                        //数据，json字符串
                        data: JSON.stringify(c),
                        async: true,
                        //请求成功
                        success: function (rst) {
                            if (rst.result && rst.result.flag)
                                inspectionItem.editModalVue.videoTerminals = rst.result.resultDatas;
                        }
                    });
                },
                setPresetPointSelect: function (vTerId) {
                    if (!vTerId) {
                        inspectionItem.editModalVue.presetPoints = [];
                        return;
                    }
                    let c = {
                        searchCondition: {
                            VideoDevId: vTerId,
                            IsNeedChildren: false
                        }
                    }
                    isas.ajax({
                        isHideSuccessMsg: true,
                        //请求地址
                        url: AppServiceUrl.PresetPoint_FindPresetPointsForSelect,
                        //数据，json字符串
                        data: JSON.stringify(c),
                        async: true,
                        //请求成功
                        success: function (rst) {
                            if (rst.result && rst.result.flag)
                                inspectionItem.editModalVue.presetPoints = rst.result.resultDatas;
                            //inspectionItem.editModalVue.presetPointId = dValue;
                            //$("#presetPointId").val(dValue);
                        }
                    });
                }
            },
            mounted: function () {
            }
        })
    },
    //编辑任务单的Form验证
    initEditFormValidate: function () {
        if (!inspectionItem.editFormId) return;
        $("#" + inspectionItem.editFormId).validate({
            ignore: ":hidden:not(select)",//解决无法校验select
            rules: {
                seqNo: {
                    required: true,
                },
                itemName: {
                    required: true,
                },
                processAction: {
                    required: true,
                },
                processDuration: {
                    required: true,
                },
                videoDevId: {
                    required: true,
                }
            },
            messages: {
                seqNo: {
                    required: "请输入巡检序号，用于巡检运行顺序",
                },
                itemName: {
                    required: "请输入巡检项名称",
                },
                processAction: {
                    required: "请选择巡检活动",
                },
                processAction: {
                    required: "请输入任务持续时间",
                },
                videoDevId: {
                    required: "请选择摄像头",
                }
            }
        });
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + inspectionItem.editFormId).validate().resetForm();
        $("#" + inspectionItem.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    //初始化编辑任务单各值
    initEditModalValues: function (uniqueId) {
        inspectionItem.resetFormValidate();
        if (!inspectionItem.editModalVue || !inspectionItem.mainTableId) return;
        let card = inspectionCard.getSelectCardSingle();
        if (!card) {
            layer.alert("请选择巡检任务单！");
            return;
        }
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + inspectionItem.mainTableId).bootstrapTable('getRowByUniqueId', uniqueId);
        if (!rowData) {
            rowData = {
                id: null,
                seqNo: "",
                itemName: "",
                processAction: null,
                processDuration: 5,
                isImageRecognition: false,
                remark: "",
                isActive: true,
                inspectionCardId: card.id,
                inspectionCard: {
                    id: card.id,//任务项所属任务单ID
                    cardName: card.cardName//任务项所属任务单
                },
                videoDevId: null,//摄像头ID
                presetPointId: null,
            }
            inspectionItem.editModalVue.header = '添加巡检任务项';
        } else
            inspectionItem.editModalVue.header = '编辑巡检任务项';
        inspectionItem.editModalVue.processActions = inspectionItem.baseProcessActions;
        inspectionItem.editModalVue.equipmentTypes = inspectionItem.baseEquipmentTypes;
        inspectionItem.editModalVue.videoTerminals = inspectionItem.baseVideoTerminals;

        //设置记录单其他参数
        inspectionItem.editModalVue.id = rowData.id;
        inspectionItem.editModalVue.seqNo = rowData.seqNo;
        inspectionItem.editModalVue.itemName = rowData.itemName;
        inspectionItem.editModalVue.processDuration = rowData.processDuration;
        inspectionItem.editModalVue.isImageRecognition = rowData.isImageRecognition;
        inspectionItem.editModalVue.isActive = rowData.isActive;
        inspectionItem.editModalVue.remark = rowData.remark;

        inspectionItem.editModalVue.processAction = rowData.processAction;
        inspectionItem.editModalVue.videoDevId = rowData.videoDevId;
        inspectionItem.editModalVue.presetPointId = rowData.presetPointId;

        //设置选中的变电所
        inspectionItem.editModalVue.inspectionCardId = rowData.inspectionCard.id;
        inspectionItem.editModalVue.inspectionCardName = rowData.inspectionCard.cardName;
    },
    clearModalSelect: function () {
        inspectionItem.editModalVue.processAction = "";
        inspectionItem.editModalVue.equipmentTypeId = null;
        inspectionItem.editModalVue.equipmentInfoId = null;
        inspectionItem.editModalVue.videoDevId = null;
        inspectionItem.editModalVue.presetPointId = null;
        inspectionItem.editModalVue.processActions = [];
        inspectionItem.editModalVue.videoNVRs = [];
        inspectionItem.editModalVue.equipmentTypes = [];
        inspectionItem.editModalVue.equipmentInfos = [];
        inspectionItem.editModalVue.videoTerminals = [];
        inspectionItem.editModalVue.presetPoints = [];
    },
    delItem: function () {
        let rows = inspectionItem.getSelectItems();
        if (!rows || rows.length == 0) {
            layer.alert("请选择要删除的行！");
            return;
        }
        let ids = new Array();
        rows.forEach(function (r) {
            ids.push(r.id);
        });
        isas.ajax({
            confirm: true,
            //请求地址
            url: AppServiceUrl.InspectionItem_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        inspectionItem.refreshTable();
                    }
                }
            },
        });
    },
    getSelectItems: function () {
        if (!inspectionItem.mainTableId) return null;
        return $("#" + inspectionItem.mainTableId).bootstrapTable('getSelections');
    },
    refreshTable: function () {
        let card = inspectionCard.getSelectCardSingle();
        if (!card) return;
        $('#' + inspectionItem.mainTableId).bootstrapTable('refresh');
    },
    removeAllData: function () {
        $('#' + inspectionItem.mainTableId).bootstrapTable('removeAll');
    }
}

var inspectionPlanTask = {
    planTaskVue: null,
    vueId: "",
    editFormId: "",
    initPlanTask: function () {
        inspectionPlanTask.vueId = "planTaskVueDiv";
        inspectionPlanTask.editFormId = "editPlanTaskForm";

        inspectionPlanTask.initVue();
        inspectionPlanTask.initEditFormValidate();
        inspectionPlanTask.initComponent();
    },
    initComponent: function () {
        let tHeight = $("#cardChildrenDiv").height() - 70;
        tHeight = tHeight < 200 ? 200 : tHeight;
        $("#" + inspectionPlanTask.vueId).height(tHeight);
    },
    initVue: function () {
        if (!inspectionPlanTask.vueId) return;
        inspectionPlanTask.planTaskVue = new Vue({
            el: '#' + inspectionPlanTask.vueId,
            data: {
                id: null,
                seqNo: "",
                planTaskName: "",
                executionWeekText: "",
                executionWeek: "",
                executionTime: "",
                remark: "",
                isActive: true,
                //inspectionCardId: null,
                weekDatas: [
                    { value: 1, text: "星期一", isActive: false },
                    { value: 2, text: "星期二", isActive: false },
                    { value: 3, text: "星期三", isActive: false },
                    { value: 4, text: "星期四", isActive: false },
                    { value: 5, text: "星期五", isActive: false },
                    { value: 6, text: "星期六", isActive: false },
                    { value: 7, text: "星期日", isActive: false },
                    //{ value: "", text: "清空", isActive: false },
                ],
                planTaskDatas: new Array(),
                timePlacement: 'top',
                timeAlign: 'left',
                selectPlanTaskIds: new Array(),
            },
            methods: {
                weekBtnClick: function (v, t) {
                    this.executionWeek = v;
                    this.executionWeekText = v ? t : "";
                    let i = 0;
                    this.weekDatas.forEach(function (w) {
                        let temp = {
                            value: w.value,
                            text: w.text,
                            isActive: w.value == v
                        }
                        inspectionPlanTask.planTaskVue.weekDatas.splice(i, 1, temp);
                        i++;
                    });
                },
                setTimeValue: function (v) {
                    this.executionTime = v;
                },
                savePlanTask: function (event) {
                    if (!inspectionPlanTask.editFormId) return;
                    if (!$("#" + inspectionPlanTask.editFormId).valid()) return;
                    let selecCard = inspectionCard.getSelectCardSingle();
                    if (!selecCard) {
                        layer.alert("请选择巡检任务单！");
                        return;
                    }
                    if (this.planTaskDatas && this.planTaskDatas.length > 1 && this.executionTime) {
                        let len = this.planTaskDatas.length;
                        for (let i = 0; i < len; i++) {
                            let task = this.planTaskDatas[i];
                            if (task.executionWeek == this.executionWeek) {
                                let startTimeStrs = task.executionTime.split(':');
                                let endTimeStrs = this.executionTime.split(':');
                                let secondDiff = (parseInt(endTimeStrs[0]) * 3600 + parseInt(endTimeStrs[1]) * 60) - (parseInt(startTimeStrs[0]) * 3600 + parseInt(startTimeStrs[1]) * 60);//计算出相差秒
                                if (Math.abs(secondDiff) < 30 * 60) {
                                    layer.alert("同一天巡检任务计划时间过短，未超过30分钟！请重新设置！");
                                    return;
                                }
                            }
                        }
                    }
                    let data = {
                        id: this.id,
                        planTaskName: this.planTaskName,
                        executionWeek: this.executionWeek,
                        executionTime: this.executionTime,
                        remark: this.remark,
                        isActive: this.isActive,
                        InspectionCardId: selecCard.id,
                    }
                    isas.ajax({
                        isHideSuccessMsg: true,
                        //请求地址
                        url: AppServiceUrl.InspectionPlanTask_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    inspectionPlanTask.planTaskVue.planTaskName = "";
                                    inspectionPlanTask.planTaskVue.executionWeek = "";
                                    inspectionPlanTask.planTaskVue.executionWeekText = "";
                                    inspectionPlanTask.planTaskVue.executionTime = "";
                                    inspectionPlanTask.planTaskVue.remark = "";
                                    inspectionPlanTask.planTaskVue.weekDatas.forEach(function (w) {
                                        w.isActive = false;
                                    });
                                    inspectionPlanTask.refreshData();
                                }
                            }
                        }
                    });
                },
                delPlanTask: function (id) {
                    if (!id) {
                        layer.alert("请选择要删除的任务计划！");
                        return;
                    }
                    //alert((typeof (id) == "string") + "|||" + id);
                    isas.ajax({
                        confirm: true,
                        type: "get",
                        //请求地址
                        url: AppServiceUrl.InspectionPlanTask_DeleteById,
                        //数据，json字符串
                        data: { id: id },
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag)
                                    inspectionPlanTask.refreshData();
                            }
                        }
                    });
                },
                selectPlanTask: function (id) {
                    if (!this.selectPlanTaskIds) {
                        this.selectPlanTaskIds = new Array();
                        this.selectPlanTaskIds.push(id);
                        return;
                    }
                    let len = this.selectPlanTaskIds.length;
                    let indexA = -1;
                    for (let i = 0; i < len; i++) {
                        if (this.selectPlanTaskIds[i] == id) {
                            indexA = i;
                            break;
                        }
                    }
                    if (indexA == -1) {
                        this.selectPlanTaskIds.push(id);
                        $("#" + id).find(".thumbtack_blackboard").css("color", "#ed5565");
                        $("#" + id).css("border", "2px solid #ed5565");
                    } else {
                        this.selectPlanTaskIds.splice(indexA, 1);
                        $("#" + id).find(".thumbtack_blackboard").css("color", "#269BDE");
                        $("#" + id).css("border", "0px solid transparent");
                    }
                },
                copyPlanTask: function () {
                    if (!this.selectPlanTaskIds || this.selectPlanTaskIds.length == 0) {
                        layer.alert("请选择要复制的任务计划！");
                        return;
                    }
                    //alert((typeof (id) == "string") + "|||" + id);
                    isas.ajax({
                        confirm: true,
                        //数据，json字符串
                        data: JSON.stringify(this.selectPlanTaskIds),
                        //请求地址
                        url: AppServiceUrl.InspectionPlanTask_CopyTaskByIds,
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    inspectionPlanTask.refreshData();
                                }
                            }
                        }
                    });
                }
            }
        });
    },
    //编辑任务单的Form验证
    initEditFormValidate: function () {
        if (!inspectionPlanTask.editFormId) return;
        $("#" + inspectionPlanTask.editFormId).validate({
            rules: {
                executionWeekText: {
                    required: true,
                },
                executionTime: {
                    required: true,
                }
            },
            messages: {
                executionWeekText: {
                    required: "星期不能为空",
                },
                executionTime: {
                    required: "时间不能为空",
                }
            }
        });
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + inspectionPlanTask.editFormId).validate().resetForm();
        $("#" + inspectionPlanTask.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    refreshData: function () {
        inspectionPlanTask.planTaskVue.selectPlanTaskIds.forEach(function (id) {
            $("#" + id).find(".thumbtack_blackboard").css("color", "#269BDE");
            $("#" + id).css("border", "0px solid transparent");
        });
        let selecCard = inspectionCard.getSelectCardSingle();
        if (!selecCard) {
            //layer.alert("请选择巡检任务单！");
            return;
        }
        inspectionPlanTask.planTaskVue.selectPlanTaskIds = new Array();
        let c = {
            searchCondition: {
                inspectionCardId: selecCard.id,
                IsNeedChildren: false
            },
            sorting: "executionWeek"
        }
        isas.ajax({
            isHideSuccessMsg: true,
            //请求地址
            url: AppServiceUrl.InspectionPlanTask_FindDatas,
            //数据，json字符串
            data: JSON.stringify(c),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag)
                        inspectionPlanTask.planTaskVue.planTaskDatas = rst.result.resultDatas;
                }
            }
        });
    },
    removeAllData: function () {
        inspectionPlanTask.planTaskVue.planTaskDatas = [];
    }
}