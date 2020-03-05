$(document).ready(function () {
    substationTree.initTree(subTreeChanged);
    videoDevEquipmentList.initFunc();
});
function subTreeChanged(node) {
    setBasedata();
    videoDevEquipmentList.refreshTable();
}
function setBasedata() {
    let node = substationTree.getSelectSubstationNode();
    let c = {
        searchCondition: {
            devName: "",
            transformerSubstationId: node ? node.id : null,
            DevType: 0,
            IsOnlyActive: true,
            IsNeedChildren: false,
            IsVideoTerminal: false,
        },
        sorting: ""
    }

    isas.ajax({
        url: AppServiceUrl.VideoDev_FindVideoDevForSelect,
        data: JSON.stringify(c),
        isHideSuccessMsg: true,
        success: function (rst) {
            if (rst.result) {
                if (rst.result.flag) {
                    videoDevEquipmentList.editModalVue.devValues = rst.result.resultDatas
                }
            }
        }
    });

    isas.ajax({
        url: AppServiceUrl.EquipmentType_FindEquipmentTypeForSelect,
        data: JSON.stringify({}),
        isHideSuccessMsg: true,
        success: function (rst) {
            if (rst.result) {
                if (rst.result.flag) {
                    videoDevEquipmentList.editModalVue.devTypeValues = rst.result.resultDatas
                }
            }
        }
    });
}
var videoDevEquipmentList = {
    mainTableId: "VideoDevEquipmentTable",
    toolBarId: "VideoDevEquipmentTableToolbar",
    editFormId: "editVideoDevEquipmentForm",
    editModalId: "editVideoDevEquipmentModal",

    vueRowData: {
        id: null,
        dev: "",
        devEquipment: '',
        devVideoDevEquipment: '',
        devType: '',
        remark: "",
        isActive: true,
    },
    //初始化
    initFunc: function () {
        videoDevEquipmentList.initComponent();
        videoDevEquipmentList.initEditModalByVue();
    },
    //初始化任务单个按钮事件
    initComponent: function () {
        $("#searchVideoDevEquipmentBtn").click(function () {
            videoDevEquipmentList.refreshTable();
        });
        $("#addVideoDevEquipmentBtn").click(function () {
            videoDevEquipmentList.initEditFormValidate();
            videoDevEquipmentList.initEditModalValues("");
            $(videoDevEquipmentList.editModalVue.$el).modal('show');
        });
        $("#delVideoDevEquipmentBtn").click(function () {
            videoDevEquipmentList.delItem();
        });
        videoDevEquipmentList.initTable();
    },
    //初始化摄像头列表
    initTable: function () {
        if (!videoDevEquipmentList.mainTableId) return;
        isas.bootstrapTable({
            el: '#' + videoDevEquipmentList.mainTableId,
            toolBarEl: '#' + videoDevEquipmentList.toolBarId,
            url: AppServiceUrl.VideoDevEquipmentInfo_FindDatas,
            isInitData: false,
            singleSelect: false,
            pageList: [12, 18, 24],
            pageSize: 12,
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let node = substationTree.getSelectSubstationNode();
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        VideoDevName: $("#searchVideoDevEquipmentName").val(),
                        EquipmentInfoName: $("#searchDevEquipmentName").val(),
                        transformerSubstationId: node ? node.id : null,
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
                        let pageSize = $('#' + videoDevEquipmentList.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + videoDevEquipmentList.mainTableId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'equipmentInfo', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        if (value) {
                            return value.name;
                        }
                        return value;
                    }
                },
                {
                    field: 'videoDev', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '视频设备名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        ////console.log(value)
                        if (value) {
                            return value.devName;
                        }
                        return value;
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
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#" + videoDevEquipmentList.editModalId + "' onclick='videoDevEquipmentList.initEditModalValues(\"" + row.id + "\")'>";
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
        if (!videoDevEquipmentList.editModalId) return;
        videoDevEquipmentList.editModalVue = new Vue({
            el: '#' + videoDevEquipmentList.editModalId,
            data: {
                header: '',
                id: null,
                dev: "",
                devValues: [],
                devEquipment: '',
                devEquipmentValues: [],
                devVideoDevEquipment: '',
                devVideoDevEquipmentValues: [],
                devType: "",
                devTypeValues: [],
                remark: "",
                isActive: true,
                //ifShow:true,
            },
            created: function () {
            },
            watch: {
            },
            methods: {
                save: function (event) {
                    if (!videoDevEquipmentList.mainTableId || !videoDevEquipmentList.editFormId) return;
                    if (!$("#" + videoDevEquipmentList.editFormId).valid()) return;
                    let node = substationTree.getSelectSubstationNode();
                    let data = {
                        id: this.id,
                        EquipmentInfoId: this.devEquipment,
                        VideoDevId: this.devVideoDevEquipment,
                        TransformerSubstationId: node ? node.id : null,
                        remark: this.remark,
                        isActive: this.isActive,
                    };
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.VideoDevEquipmentInfo_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    videoDevEquipmentList.refreshTable();
                                    $(videoDevEquipmentList.editModalVue.$el).modal('hide');
                                }
                                layer.alert(rst.result.message);
                            }
                        }
                    });
                },
                //选择 NVR
                setDevAction: function (para) {
                    //if (para == "") {
                    //    return;
                    //}
                    let _this = this;
                    this.dev = para;
                    let node = substationTree.getSelectSubstationNode();

                    let e = {
                        searchCondition: {
                            devName: "",
                            transformerSubstationId: node ? node.id : null,
                            VideoDevId: para,
                            IsOnlyActive: true,
                            IsNeedChildren: false,
                            IsVideoTerminal: true,
                        },
                        sorting: ""
                    }

                    isas.ajax({
                        url: AppServiceUrl.VideoDev_FindVideoDevForSelect,
                        data: JSON.stringify(e),
                        isHideSuccessMsg: true,
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    _this.devVideoDevEquipmentValues = rst.result.resultDatas
                                }
                            }
                        }
                    });
                },
                //选择设备类型
                setdevTypeAction: function (para) {
                    if (para == null) {
                        return;
                    }
                    let _this = this;
                    let node = substationTree.getSelectSubstationNode();

                    let e = {
                        searchCondition: {
                            devName: "",
                            transformerSubstationId: node ? node.id : null,
                            equipmentTypeId: para,
                            IsOnlyActive: true,
                        },
                        sorting: ""
                    }

                    isas.ajax({
                        url: AppServiceUrl.EquipmentInfo_FindEquipmentInfoForSelect,
                        data: JSON.stringify(e),
                        isHideSuccessMsg: true,
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    _this.devEquipmentValues = rst.result.resultDatas
                                }
                            }
                        }
                    });
                },
                setdevEquipmentAction: function (para) {
                    this.devEquipment = para;
                },
                setdevVideoDevEquipmentAction: function (para) {
                    this.devVideoDevEquipment = para;
                }
            },
            mounted: function () {
            }
        }),
            videoDevEquipmentList.vueRowData = {
                id: null,
                dev: "",
                devEquipment: '',
                devVideoDevEquipment: '',
                remark: "",
                isActive: true,
            }
    },
    //编辑任务单的Form验证
    initEditFormValidate: function () {
        if (!videoDevEquipmentList.editFormId) return;
        $("#" + videoDevEquipmentList.editFormId).validate({
            ignore: ":hidden:not(select)",//解决无法校验select
            rules: {
                dev: {
                    required: true,
                },
                devEquipment: {
                    required: true,
                },
                devVideoDevEquipment: {
                    required: true,
                }
            },
            messages: {
                dev: {
                    required: "请选择 NVR/DVR",
                },
                devEquipment: {
                    required: "请选择设备",
                },
                devVideoDevEquipment: {
                    required: "请选择时评设备",
                }
            }
        });
    },
    //重置验证
    resetFormValidate: function () {
        ////console.log($("#" + videoDevEquipmentList.editFormId).validate())
        $("#" + videoDevEquipmentList.editFormId).validate().resetForm();
        $("#" + videoDevEquipmentList.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    //初始化编辑任务单各值
    initEditModalValues: function (uniqueId) {
        if (!videoDevEquipmentList.editModalVue || !videoDevEquipmentList.mainTableId) return;
        videoDevEquipmentList.resetFormValidate()
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + videoDevEquipmentList.mainTableId).bootstrapTable('getRowByUniqueId', uniqueId);
        if (!rowData) {
            rowData = videoDevEquipmentList.vueRowData;
            videoDevEquipmentList.editModalVue.header = '添加';
        } else {
            videoDevEquipmentList.editModalVue.header = '编辑';
        }
        videoDevEquipmentList.refreshRowData(rowData)
    },

    delItem: function () {
        let rows = videoDevEquipmentList.getSelectItems();
        if (!rows || rows.length == 0) {
            layer.alert("请选择要删除的行！");
            return;
        }
        let ids = new Array();
        rows.forEach(function (r) {
            ids.push(r.id);
        });
        isas.ajax({
            //请求地址
            url: AppServiceUrl.VideoDevEquipmentInfo_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        videoDevEquipmentList.refreshTable();
                    }
                    layer.alert(rst.result.message);
                } else if (!rst.success)
                    layer.alert(rst.error.message);
            },
        });
    },

    refreshTable: function () {
        $('#' + videoDevEquipmentList.mainTableId).bootstrapTable('refresh');
    },
    removeTableAll: function () {
        $('#' + videoDevEquipmentList.mainTableId).bootstrapTable('removeAll');
    },
    getSelecSingle: function () {
        if (!videoDevEquipmentList.mainTableId) return null;
        let rows = $("#" + videoDeviceList.mainTableId).bootstrapTable('getSelections');
        return !rows || rows.length == 0 ? null : rows[0];
    },
    getSelectItems: function () {
        if (!videoDevEquipmentList.mainTableId) return null;
        return $("#" + videoDevEquipmentList.mainTableId).bootstrapTable('getSelections');
    },
    refreshRowData: function (rowData) {
        videoDevEquipmentList.editModalVue.id = rowData.id;
        if (rowData.videoDev && rowData.equipmentInfo) {
            videoDevEquipmentList.editModalVue.dev = rowData.videoDev.videoDevId
            videoDevEquipmentList.editModalVue.devType = rowData.equipmentInfo.equipmentTypeId
        } else {
            videoDevEquipmentList.editModalVue.dev = null;
            videoDevEquipmentList.editModalVue.devType = null;
        }
        videoDevEquipmentList.editModalVue.devEquipment = rowData.equipmentInfoId;
        videoDevEquipmentList.editModalVue.devVideoDevEquipment = rowData.videoDevId;

        videoDevEquipmentList.editModalVue.remark = rowData.remark
        videoDevEquipmentList.editModalVue.isActive = rowData.isActive
        videoDevEquipmentList.editModalVue.setdevTypeAction(videoDevEquipmentList.editModalVue.devType)
        videoDevEquipmentList.editModalVue.setDevAction(videoDevEquipmentList.editModalVue.dev)

        ////console.log(videoDevEquipmentList.editModalVue)
    },
}