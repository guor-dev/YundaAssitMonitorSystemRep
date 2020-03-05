$(document).ready(function () {
    substationTree.initTree(subTreeChanged);
    videoDeviceList.initListFunc();
    videoCameraList.initFunc();
    presetList.initListFunc();
    devLinkList.initListFunc();
    setPageStaticBaseData();
});
function subTreeChanged(node) {
    setPageActiveBasedata()
    videoDeviceList.refreshTable();
    videoCameraList.refreshTable();
}
function setPageActiveBasedata() {
    isas.ajax({
        url: AppServiceUrl.ManufacturerInfo_FindManufacturerInfoForSelect,
        data: JSON.stringify({}),
        isHideSuccessMsg: true,
        success: function (rst) {
            if (rst.result) {
                if (rst.result.flag) {
                    videoDeviceList.editModalVue.devManufacturerValues = rst.result.resultDatas
                    videoCameraList.editModalVue.videomanufacturerValues = rst.result.resultDatas
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
                    //console.log(rst.result.resultDatas)
                    devLinkList.editModalVue.devTypeValues = rst.result.resultDatas
                }
            }
        }
    });
}
function setPageStaticBaseData() {
    videoDeviceList.editModalVue.devTypeValues.push(
        { text: devType[0], value: 0, key: 0 },
    )
    videoCameraList.editModalVue.videoTypeValues.push(
        { text: devType[1], value: 1, key: 1 },
        { text: devType[2], value: 2, key: 2 },
        { text: devType[3], value: 3, key: 3 },
        { text: devType[99], value: 99, key: 99 },
    )

    videoCameraList.editModalVue.devNoValues.push(
        { text: 1, value: 1, key: 1 },
        { text: 2, value: 2, key: 2 },
    )
}

var devType = {
    "0": "硬盘录像机",
    "1": "枪机",
    "2": "球机",
    "3": "热成像",
    "99": "其他"
}
var CodeStreamTypeEnum = {
    "1": "主码流",
    "2": "子码流",
}

var videoDeviceList = {
    editModalVue: null,
    mainTableId: null,
    toolBarId: null,
    editModalId: null,
    editFormId: null,
    VideoDevRowData: {},
    //初始化任务单
    initListFunc: function () {
        videoDeviceList.mainTableId = "videoDeviceTable";
        videoDeviceList.toolBarId = "videoDeviceTableToolbar";
        videoDeviceList.editModalId = "editVideoDeviceModal";
        videoDeviceList.editFormId = "editVideoDeviceForm";

        videoDeviceList.initEditModalByVue();
        videoDeviceList.initComponent();
        //videoDeviceList.initEditFormValidate();
    },
    //初始化任务单个按钮事件
    initComponent: function () {
        $("#searchVideoDeviceBtn").click(function () {
            videoDeviceList.refreshTable();
            videoCameraList.refreshTable();
        });
        $("#addVideoDeviceBtn").click(function () {
            let subNode = substationTree.getSelectSubstationNode();
            if (!subNode) {
                layer.alert("请选择变电所！");
                return;
            }
            videoDeviceList.initEditFormValidate();
            videoDeviceList.refreshRowData(videoDeviceList.VideoDevRowData);
            videoDeviceList.initEditModalValues("");
            $('#' + videoDeviceList.editModalId).modal('show');
        });
        $("#delVideoDeviceBtn").click(function () {
            videoDeviceList.delete();
        });
        videoDeviceList.initTable();
    },
    //初始化任务单表
    initTable: function () {
        if (!videoDeviceList.mainTableId) return;
        let tableHeight = $("#" + videoDeviceList.mainTableId).parent().height();
        tableHeight = tableHeight < 200 ? 200 : tableHeight;
        isas.bootstrapTable({
            el: '#' + videoDeviceList.mainTableId,
            toolBarEl: '#' + videoDeviceList.toolBarId,
            url: AppServiceUrl.VideoDev_FindDatas,
            //height: tableHeight,
            isInitData: false,
            singleSelect: true,
            pageList: [2, 4, 6],
            pageSize: 2,
            onLoadSuccess: function () {
                //$("#" + videoDeviceList.mainTableId).bootstrapTable('hideLoading');
            },
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let node = substationTree.getSelectSubstationNode();
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        devName: $("#searchVideoDeviceName").val(),
                        transformerSubstationId: node ? node.id : null,
                        DevType: 0,
                        IsOnlyActive: true,
                        IsNeedChildren: false,
                        IsVideoTerminal: false,
                    },
                    sorting: "devName"
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
                        let pageSize = $('#' + videoDeviceList.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + videoDeviceList.mainTableId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'devName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'devType', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备类型', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        var regPos = /^\d+(\.\d+)?$/; //非负浮点数
                        var regNeg = /^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$/; //负浮点数
                        if (regPos.test(value) || regNeg.test(value)) {
                            return devType[parseInt(value)]
                        } else {
                            return value;
                        }
                    }
                },
                {
                    field: 'manufacturerInfoId', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备厂商', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: false,
                },
                {
                    field: 'manufacturerInfo', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备厂商', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        // //console.log("manufacture" + value)
                        if (value) {
                            if (value.manufacturerName) {
                                return value.manufacturerName
                            }
                        }
                        return value
                    }
                },

                {
                    field: 'ip', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: 'ip地址', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },

                {
                    field: 'port', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '端口号', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'devUserName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '登录名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                },
                {
                    field: 'devPassword', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '登录密码', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
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
                            //return dateFormar.dateFormar("yyyy-MM-dd", value)
                            let date = new Date(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        return null;
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
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#" + videoDeviceList.editModalId + "' onclick='videoDeviceList.initEditModalValues(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-pencil' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }
            ],
            onCheck: function (row, $element) {
                videoCameraList.refreshTable();
            },
        });
    },
    //实例化编辑任务单的Vue
    initEditModalByVue: function () {
        if (!videoDeviceList.editModalId) return;
        videoDeviceList.editModalVue = new Vue({
            el: '#' + videoDeviceList.editModalId,
            data: {
                header: '',
                id: null,
                devName: "",
                format: "",
                language: "",
                ip: '',
                installationDate: null,
                productionDate: null,
                typeValue: "",
                devManufacturerValues: [],
                devTypeValues: [],
                devmanufacturerValue: "",
                port: "",
                devUserName: "",
                devPassword: "",
                channelNo: '',
                devNo: '',
                isPTZ: '',
                devType: '',
                remark: "",
                isActive: true,
                devmanufacturerplaceholder: "请选择厂商",
                devTypeplaceholder: "请选择设备类型",
                installationDateStrplaceholder: "请选择时间",
                productionDateStrplaceholder: "请选择时间",
            },
            created: function () {
            },
            watch: {
            },
            methods: {
                save: function (event) {
                    if (!videoDeviceList.mainTableId || !videoDeviceList.editFormId) return;
                    if (!$("#" + videoDeviceList.editFormId).valid()) return;
                    let data = {
                        id: this.id,
                        seqNo: -1,
                        devName: this.devName,
                        DevType: parseInt(this.typeValue),
                        ManufacturerInfoId: this.devmanufacturerValue,
                        InstallationDate: this.dateInstallValue,
                        ProductionDate: this.dateProduceValue,
                        IP: this.ip,
                        Port: parseInt(this.port),
                        DevUserName: this.devUserName,
                        DevPassword: this.devPassword,
                        TransformerSubstationId: substationTree.getSelectSubstationNode().id,
                        remark: this.remark,
                        isActive: this.isActive,
                    }
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.VideoDev_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    videoDeviceList.refreshTable();
                                    $(videoDeviceList.editModalVue.$el).modal('hide');
                                }
                            }
                        }
                    });
                },

                setdevTypeAction: function (para) {
                    this.typeValue = para
                },
                setInstallDateValue: function (para) {
                    this.dateInstallValue = para
                },
                setProduceDateValue: function (para) {
                    this.dateProduceValue = para
                },
                setDevManufacturerAction: function (para) {
                    this.devmanufacturerValue = para
                    //console.log(para)
                }
            },
            mounted: function () {
            }
        })
        //声明行数据
        videoDeviceList.VideoDevRowData = {
            header: '',
            id: null,
            devName: "",
            format: "",
            language: "",
            ip: '',
            installationDate: null,
            productionDate: null,
            devManufacturerValues: [],
            devTypeValues: [],
            devType: '',
            devmanufacturerValue: "",
            substationValue: "",
            port: "",
            devUserName: "",
            devPassword: '',
            channelNo: '',
            devNo: '',
            isPTZ: '',
            remark: "",
            isActive: true,
            manufacturerInfo: {},
        }
    },
    //编辑任务单的Form验证
    initEditFormValidate: function () {
        if (!videoDeviceList.editFormId) return;
        $("#" + videoDeviceList.editFormId).validate({
            ignore: ":hidden:not(select)",//解决无法校验select
            rules: {
                devName: {
                    required: true,
                },
                devmanufacturerValue: {
                    required: true,
                },
                devType: {
                    required: true,
                },
                ip: {
                    required: true,
                },
                port: {
                    required: true,
                },
                devUserName: {
                    required: true,
                },
                devPassword: {
                    required: true,
                }
            },
            messages: {
                devName: {
                    required: "名称不能为空",
                },
                devmanufacturerValue: {
                    required: "请选择厂商",
                },
                devType: {
                    required: "请选择类型",
                },
                ip: {
                    required: "ip不能为空",
                    ipvalidator: "ip地址错误"
                },
                port: {
                    required: "端口不能为空",
                },
                devUserName: {
                    required: "用户名不能为空",
                },
                devPassword: {
                    required: "密码不能为空",
                }
            }
        });
        /*ip验证*/
        $.validator.addMethod("ipvalidator", function (value, element, params) {
            var reg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/
            return this.optional(element) || reg.test(value);
        }, "ip地址错误");
    },
    //重置验证
    resetFormValidate: function () {
        ////console.log($("#" + videoDeviceList.editFormId).validate())
        $("#" + videoDeviceList.editFormId).validate().resetForm();
        $("#" + videoDeviceList.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    //初始化编辑任务单各值
    initEditModalValues: function (uniqueId) {
        if (!videoDeviceList.editModalVue || !videoDeviceList.mainTableId) return;
        let subNode = substationTree.getSelectSubstationNode();
        videoDeviceList.resetFormValidate();
        if (!subNode) {
            layer.alert("请选择变电所！");
            return;
        }
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + videoDeviceList.mainTableId).bootstrapTable('getRowByUniqueId', uniqueId);
        if (!rowData) {
            rowData = videoDeviceList.VideoDevRowData;
            videoDeviceList.editModalVue.header = '添加';
        } else
            videoDeviceList.editModalVue.header = '编辑';

        videoDeviceList.refreshRowData(rowData);
    },
    delete: function () {
        let row = videoDeviceList.getSelectSingle();
        if (!row || row.length == 0) {
            layer.alert("请选择要删除的行！");
            return;
        }
        isas.ajax({
            type: "get",
            //请求地址
            url: AppServiceUrl.VideoDev_DeleteById,
            //数据，json字符串
            data: { id: row.id },
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        videoDeviceList.refreshTable();
                        videoCameraList.refreshTable();
                    }
                    layer.alert(rst.result.message);
                } else if (!rst.success)
                    layer.alert(rst.error.message);
            },
        });
    },
    getSelectSingle: function () {
        if (!videoDeviceList.mainTableId) return null;
        let rows = $("#" + videoDeviceList.mainTableId).bootstrapTable('getSelections');
        return !rows || rows.length == 0 ? null : rows[0];
    },
    refreshTable: function () {
        $('#' + videoDeviceList.mainTableId).bootstrapTable('refresh');
    },
    refreshRowData: function (rowData) {
        videoDeviceList.editModalVue.id = rowData.id;
        videoDeviceList.editModalVue.ip = rowData.ip;
        videoDeviceList.editModalVue.devName = rowData.devName;
        videoDeviceList.editModalVue.format = rowData.format;
        videoDeviceList.editModalVue.devName = rowData.devName;
        videoDeviceList.editModalVue.language = rowData.language;
        videoDeviceList.editModalVue.devmanufacturerValue = rowData.manufacturerInfoId;
        videoDeviceList.editModalVue.typeValue = rowData.devType;
        videoDeviceList.editModalVue.installationDate = (new Date(rowData.installationDate)).Format("yyyy-MM-dd");
        videoDeviceList.editModalVue.productionDate = (new Date(rowData.productionDate)).Format("yyyy-MM-dd");
        videoDeviceList.editModalVue.substationValue = substationTree.getSelectSubstationNode().id
        videoDeviceList.editModalVue.port = rowData.port
        videoDeviceList.editModalVue.devUserName = rowData.devUserName
        videoDeviceList.editModalVue.devPassword = rowData.devPassword
        videoDeviceList.editModalVue.channelNo = rowData.channelNo
        videoDeviceList.editModalVue.devNo = rowData.devNo
        videoDeviceList.editModalVue.isPTZ = rowData.isPTZ
        videoDeviceList.editModalVue.devType = rowData.devType
        videoDeviceList.editModalVue.remark = rowData.remark
        videoDeviceList.editModalVue.isActive = rowData.isActive
        //console.log(rowData)
        //console.log(videoDeviceList.editModalVue)
    }
}

var videoCameraList = {
    mainTableId: "cameraTable",
    toolBarId: "cameraTableToolbar",
    editModalId: "editCameraModal",
    editFormId: "editCameraForm",
    vueUsedChannel: [],
    vueRowData: {},
    //初始化
    initFunc: function () {
        videoCameraList.initEditModalByVue();
        videoCameraList.initComponent();
    },
    //初始化任务单个按钮事件
    initComponent: function () {
        $("#searchCameraBtn").click(function () {
            videoCameraList.refreshTable();
        });
        $("#addCameraBtn").click(function () {
            let selectItem = videoDeviceList.getSelectSingle();
            if (!selectItem) {
                layer.alert("请选择 NVR/DVR 设备！");
                return;
            }
            videoCameraList.initEditFormValidate();
            videoCameraList.initEditModalValues("");
            $(videoCameraList.editModalVue.$el).modal('show');
        });
        $("#delCameraBtn").click(function () {
            videoCameraList.delItem();
        });
        videoCameraList.initTable();
    },
    //初始化摄像头列表
    initTable: function () {
        if (!videoCameraList.mainTableId) return;
        isas.bootstrapTable({
            el: '#' + videoCameraList.mainTableId,
            toolBarEl: '#' + videoCameraList.toolBarId,
            url: AppServiceUrl.VideoDev_FindDatas,
            isInitData: false,
            singleSelect: false,
            pageList: [9, 18, 27],
            pageSize: 9,
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let selectItem = videoDeviceList.getSelectSingle();
                let c = {
                    pageIndex: params.offset / params.limit + 1, // 每页显示数据的开始行号
                    pageSize: params.limit, // 每页要显示的数据条数
                    searchCondition: {
                        devName: $("#searchCameraName").val(),
                        transformerSubstationId: substationTree.getSelectSubstationNode().id,
                        VideoDevId: selectItem == null ? selectItem : selectItem.id,
                        IsOnlyActive: false,
                        IsNeedChildren: false,
                        IsVideoTerminal: true,
                        IsNeedPresetPoint: false,
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
                        let pageSize = $('#' + videoDeviceList.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + videoDeviceList.mainTableId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'devName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'VideoDevId', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备对应NVRid', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: false
                },

                {
                    field: 'manufacturerInfo', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备厂商', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        ////console.log("manufacture" + value)
                        if (value) {
                            if (value.manufacturerName) {
                                return value.manufacturerName
                            }
                        }
                        return value
                    }
                },
                {
                    field: 'manufacturerInfoId', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备厂商', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: false,
                },
                {
                    field: 'channelNo', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '占用通道号', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },

                {
                    field: 'devNo', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '设备通道号', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'isPTZ', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '云台支持', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        if (value) {
                            return "支持"
                        }
                        return "不支持"
                    }
                },
                {
                    field: 'codeStreamType', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '播放码流', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) { // 单元格格式化函数
                        if (value) {
                            return CodeStreamTypeEnum[value]
                        }
                        return value
                    }
                },
                {
                    field: 'ip', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: 'ip地址', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: false
                },

                {
                    field: 'port', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '端口号', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: false
                },
                {
                    field: 'devUserName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '登录名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: false,
                },
                {
                    field: 'devPassword', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '登录密码', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: false,
                },

                {
                    field: 'presetPoints', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '预置点', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        //console.log(value)
                        return "<a onclick=presetList.presetclick(\"" + row.id + "\")>详情...</a>"
                    }
                },
                {
                    field: 'test', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '关联设备', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true,
                    formatter: function (value, row, index) {
                        return "<a onclick=devLinkList.devLinkclick(\"" + row.id + "\")>详情...</a>"
                    }
                },
                {
                    field: 'installationDate', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '安装时间', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: false,
                    formatter: function (value, row, index) {
                        if (value) {
                            //return   dateFormar.dateFormar("yyyy-MM-dd", value)
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
                    visible: false,
                    formatter: function (value, row, index) {
                        if (value) {
                            //return   dateFormar.dateFormar("yyyy-MM-dd", value)
                            let date = new Date(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        return null;
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
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#" + videoCameraList.editModalId + "' onclick='videoCameraList.initEditModalValues(\"" + row.id + "\")'>";
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
        if (!videoCameraList.editModalId) return;
        videoCameraList.editModalVue = new Vue({
            el: '#' + videoCameraList.editModalId,
            data: {
                header: '',
                id: null,
                devName: "",
                VideoDevId: "",
                videoTypeValues: [],
                videoType: '',
                videomanufacturerValue: "",
                videomanufacturerValues: [],
                channelNo: '',
                channelNoValues: [],
                devNo: '',
                devNoValues: [],
                isPTZ: false,
                installationDate: null,
                productionDate: null,
                ip: '',
                port: "",
                devUserName: "",
                devPassword: '',
                codeStreamType: 2,
                format: "",
                language: "",
                remark: "",
                isActive: true,
            },
            created: function () {
            },
            watch: {
            },
            methods: {
                saveItem: function (event) {
                    if (!videoCameraList.mainTableId || !videoCameraList.editFormId) return;
                    if (!$("#" + videoCameraList.editFormId).valid()) return;
                    let selectItem = videoDeviceList.getSelectSingle();
                    let data = {
                        id: this.id,
                        devName: this.devName,
                        VideoDevId: selectItem == null ? this.VideoDevId : selectItem.id,
                        DevType: parseInt(this.videoType),
                        ManufacturerInfoId: this.videomanufacturerValue,
                        InstallationDate: this.installationDate,
                        ProductionDate: this.productionDate,
                        IP: this.ip,
                        Port: parseInt(this.port),
                        DevUserName: this.devUserName,
                        DevPassword: this.devPassword,
                        TransformerSubstationId: substationTree.getSelectSubstationNode().id,
                        devNo: parseInt(this.devNo),
                        isPTZ: this.isPTZ,
                        channelNo: parseInt(this.channelNo),
                        codeStreamType: parseInt(this.codeStreamType),
                        remark: this.remark,
                        isActive: this.isActive,
                    }
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.VideoDev_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    videoCameraList.refreshTable();
                                    $(videoCameraList.editModalVue.$el).modal('hide');
                                }
                                layer.alert(rst.result.message);
                            }
                            videoCameraList.clearModalSelect();
                        }
                    });
                },
                closeModal: function () {
                    videoCameraList.clearModalSelect();
                },
                setvideoTypeAction: function (para) {
                    this.videoType = para;
                },
                setInstallDateValue: function (para) {
                    this.installationDate = para;
                },
                setProduceDateValue: function (para) {
                    this.productionDate = para;
                },
                setvideoManufacturerAction: function (para) {
                    this.videomanufacturerValue = para;
                },
                setChannelNoAction: function (para) {
                    this.channelNo = para;
                },
                setDevNoAction: function (para) {
                    this.devNo = para;
                },
                codestreamChange: function (para) {
                    this.codeStreamType = para;
                    //console.log(para)
                }
            },
            mounted: function () {
            }
        }),
            videoCameraList.vueRowData = {
                header: '',
                id: null,
                devName: "",
                format: "",
                language: "",
                ip: '',
                installationDate: null,
                productionDate: null,
                devManufacturerValues: [],
                devTypeValues: [],
                devType: '',
                devmanufacturerValue: "",
                substationValue: "",
                port: "",
                devUserName: "",
                devPassword: '',
                channelNo: '',
                devNo: '',
                isPTZ: false,
                remark: "",
                isActive: true,
                videoDevId: "",
                codeStreamType: 2,
            }
    },
    setVideoTerminalSelect: function (nvrId, dValue) {
        if (!nvrId) return;
        let c = {
            searchCondition: {
                VideoDevId: nvrId,
                IsNeedChildren: false
            }
        }
        isas.ajax({
            //请求地址
            url: "VideoDev/FindDatas",
            //数据，json字符串
            data: JSON.stringify(c),
            async: true,
            //请求成功
            success: function (rst) {
                if (rst.result && rst.result.flag)
                    videoCameraList.editModalVue.videoTerminals = rst.result.resultDatas;
            }
        });
    },

    //编辑任务单的Form验证
    initEditFormValidate: function () {
        if (!videoCameraList.editFormId) return;
        $("#" + videoCameraList.editFormId).validate({
            ignore: ":hidden:not(select)",//解决无法校验select
            rules: {
                devCameraName: {
                    required: true,
                },
                videomanufacturerValue: {
                    required: true,
                },
                videoType: {
                    required: true,
                },
                channelNo: {
                    required: true,
                },
                devNo: {
                    required: true,
                },
                ip: {
                    ipvalidator: true,
                },
            },
            messages: {
                devCameraName: {
                    required: "名称不能为空",
                },
                videomanufacturerValue: {
                    required: "请选择厂商",
                },
                videoType: {
                    required: "请选择类型",
                },
                channelNo: {
                    required: "请选择占用 NVR/DVR 通道",
                },
                devNo: {
                    required: "请选择摄像头通道",
                },
                ip: {
                    required: "ip地址格式错误",
                },
            }
        });
        /*ip验证*/
        $.validator.addMethod("ipvalidator", function (value, element, params) {
            var reg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/
            return this.optional(element) || reg.test(value);
        }, "ip地址错误");
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + videoCameraList.editFormId).validate().resetForm();
        $("#" + videoCameraList.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    //初始化编辑任务单各值
    initEditModalValues: function (uniqueId) {
        if (!videoCameraList.editModalVue || !videoCameraList.mainTableId) return;

        videoCameraList.resetFormValidate()

        var rowData = null;
        if (uniqueId)
            rowData = $('#' + videoCameraList.mainTableId).bootstrapTable('getRowByUniqueId', uniqueId);
        videoCameraList.vueUsedChannel = videoCameraList.getUsedChannel();

        if (!rowData) {
            rowData = videoCameraList.vueRowData;
            videoCameraList.editModalVue.header = '添加';
            videoCameraList.editModalVue.channelNoValues = videoCameraList.editModalVue.channelNoValues.filter(item => videoCameraList.vueUsedChannel.indexOf(item.key) == -1)
        } else {
            videoCameraList.editModalVue.header = '编辑';
            videoCameraList.editModalVue.channelNoValues = videoCameraList.editModalVue.channelNoValues.filter(item => videoCameraList.vueUsedChannel.indexOf(item.key) == -1 || item.value == rowData.channelNo)
        }

        videoCameraList.refreshRowData(rowData)
    },
    clearModalSelect: function () {
    },
    delItem: function () {
        let rows = videoCameraList.getSelectItems();
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
            url: AppServiceUrl.VideoDev_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        videoCameraList.refreshTable();
                    }
                    layer.alert(rst.result.message);
                } else if (!rst.success)
                    layer.alert(rst.error.message);
            },
        });
    },
    getSelectSingle: function () {
        if (!videoDeviceList.mainTableId) return null;
        let rows = $("#" + videoDeviceList.mainTableId).bootstrapTable('getSelections');
        return !rows || rows.length == 0 ? null : rows[0];
    },
    getSelectItems: function () {
        if (!videoCameraList.mainTableId) return null;
        return $("#" + videoCameraList.mainTableId).bootstrapTable('getSelections');
    },
    refreshTable: function () {
        $('#' + videoCameraList.mainTableId).bootstrapTable('refresh');
    },
    removeTableAll: function () {
        $('#' + videoCameraList.mainTableId).bootstrapTable('removeAll');
    },
    refreshRowData: function (rowData) {
        videoCameraList.editModalVue.id = rowData.id;
        videoCameraList.editModalVue.ip = rowData.ip;
        videoCameraList.editModalVue.devName = rowData.devName;
        videoCameraList.editModalVue.VideoDevId = rowData.videoDevId;
        videoCameraList.editModalVue.format = rowData.format;
        videoCameraList.editModalVue.devName = rowData.devName;
        videoCameraList.editModalVue.language = rowData.language;
        videoCameraList.editModalVue.installationDate = rowData.installationDate;
        videoCameraList.editModalVue.productionDate = rowData.productionDate;
        videoCameraList.editModalVue.videomanufacturerValue = rowData.manufacturerInfoId;
        videoCameraList.editModalVue.videoType = parseInt(rowData.devType);
        videoCameraList.editModalVue.installationDate = (new Date(rowData.installationDate)).Format("yyyy-MM-dd");
        videoCameraList.editModalVue.productionDate = (new Date(rowData.productionDate)).Format("yyyy-MM-dd");
        videoCameraList.editModalVue.substationValue = substationTree.getSelectSubstationNode().id
        videoCameraList.editModalVue.port = rowData.port
        videoCameraList.editModalVue.devUserName = rowData.devUserName
        videoCameraList.editModalVue.devPassword = rowData.devPassword
        videoCameraList.editModalVue.channelNo = rowData.channelNo
        videoCameraList.editModalVue.devNo = rowData.devNo
        videoCameraList.editModalVue.isPTZ = rowData.isPTZ
        videoCameraList.editModalVue.devType = rowData.devType
        videoCameraList.editModalVue.remark = rowData.remark
        videoCameraList.editModalVue.isActive = rowData.isActive
        videoCameraList.editModalVue.codeStreamType = rowData.codeStreamType
    },
    getUsedChannel: function () {
        videoCameraList.editModalVue.channelNoValues.length = 0;
        for (var i = 33; i < 65; i++) {
            videoCameraList.editModalVue.channelNoValues.push(
                { text: i, value: i, key: i },
            )
        }
        let selectItem = videoDeviceList.getSelectSingle();

        let data = {
            searchCondition: {
                devName: "",
                transformerSubstationId: substationTree.getSelectSubstationNode().id,
                VideoDevId: selectItem == null ? selectItem : selectItem.id,
                IsOnlyActive: false,
                IsNeedChildren: false,
                IsVideoTerminal: true,
            },
        }
        let arr = [];
        isas.ajax({
            //请求地址
            url: AppServiceUrl.VideoDev_FindDatas,
            //数据，json字符串
            data: JSON.stringify(data),
            isHideSuccessMsg: true,
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        rst.result.resultDatas.forEach(item => arr.push(item.channelNo))
                    }
                }
            }
        });
        return arr;
    }
}
//预置点
var presetList = {
    mainTableId: "presetTable",
    editModalId: "presetModal",
    toolBarId: "presettableToolbar",
    datas: [],
    //初始化任务单
    initListFunc: function () {
        presetList.initComponent();
    },
    //初始化任务单个按钮事件
    initComponent: function () {
        $("#searchPresetBtn").click(function () {
            let ids = []
            let key = $("#searchPresetName").val()
            presetList.datas.forEach(item => {
                if (key == "") {
                    ids.push(item.id)
                }
                else if (item.name.indexOf(key) > -1) {
                    ids.push(item.id)
                }
            })
            $('#' + presetList.mainTableId).bootstrapTable('filterBy', { id: ids });
        });
    },
    //初始化任务单表
    initTable: function () {
        $('#' + presetList.mainTableId).bootstrapTable('destroy');
        isas.bootstrapTable({
            el: '#' + presetList.mainTableId,
            toolBarEl: '#' + presetList.toolBarId,
            isInitData: false,
            singleSelect: true,
            pageList: [5, 10, 15],
            pageSize: 5,
            data: presetList.datas,
            search: false,
            theadClasses: 'thead-default',
            columns: [
                {
                    title: '行号',
                    align: 'center',
                    valign: 'middle',
                    width: 50,
                    searchable: false,
                    formatter: function (value, row, index) {
                        let pageSize = $('#' + videoDeviceList.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + videoDeviceList.mainTableId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'number', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '预置点号', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    searchable: true,
                    visible: true
                },
                {
                    field: 'name', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '预置点名称', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    searchable: true,
                    visible: true
                },
                {
                    field: 'isActive',
                    title: '状态',
                    align: 'center',
                    valign: 'middle',
                    searchable: false,
                    width: 80,
                    formatter: function (value, row, index) {
                        var text = '-';
                        if (value) {
                            text = "<span class='text-success' style='font-size:16px;'><i class='fa fa-check'></i></span>";
                        } else {
                            text = "<span class='text-danger' style='font-size:16px;'><i class='fa fa-close'></i></span>";
                        }
                        return text;
                    },
                },
            ],
        });
    },
    presetclick: function (param) {
        //console.log(param);
        //value = [{ number: 1, name: "test", id: 1 }]//测试数据
        if (param) {
            //presetList.datas = value
            isas.ajax({
                //请求地址
                url: AppServiceUrl.PresetPoint_FindDatas,
                //数据，json字符串
                data: JSON.stringify({
                    pageIndex: 1, // 每页显示数据的开始行号
                    pageSize: 300, // 每页要显示的数据条数
                    searchCondition: {
                        VideoDevId:param,
                        IsOnlyActive: false,
                    }
                }),
                isHideSuccessMsg: true,
                //请求成功
                success: function (rst) {
                    if (rst.result) {
                        if (rst.result.flag) {
                            //let arr = devLinkList.datas.filter(item => item.id != id);
                            //console.log(rst.result.resultDatas)
                            presetList.datas = rst.result.resultDatas;
                            presetList.initTable();
                        }
                    }
                },
            });
        }
        $('#' + presetList.editModalId).modal('show');
    }
}
//关联设备
var devLinkList = {
    mainTableId: "devLinkTable",
    editModalId: "devLinkModal",
    toolBarId: "devLinktableToolbar",
    editModalVue: "devLinkVue",
    datas: [],
    //初始化任务单
    initListFunc: function () {
        devLinkList.initEditModalByVue();
        devLinkList.initComponent();
    },
    //初始化任务单个按钮事件
    initComponent: function () {
        $("#searchdevLinkBtn").click(function () {
            let ids = []
            let key = $("#searchdevLinkName").val()
            devLinkList.datas.forEach(item => {
                if (key == "") {
                    ids.push(item.id)
                }
                else if (item.equipmentInfo.name.indexOf(key) > -1) {
                    ids.push(item.id)
                }
            })
            $('#' + devLinkList.mainTableId).bootstrapTable('filterBy', { id: ids });
        });
    },
    //初始化任务单表
    initTable: function () {
        $('#' + devLinkList.mainTableId).bootstrapTable('destroy');
        isas.bootstrapTable({
            el: '#' + devLinkList.mainTableId,
            toolBarEl: '#' + devLinkList.toolBarId,
            toolbarAlign: 'right',
            url: "",
            isInitData: false,
            singleSelect: true,
            pageList: [5, 10, 15],
            pageSize: 5,
            theadClasses: 'thead-default',
            data: devLinkList.datas,
            columns: [
                {
                    title: '行号',
                    align: 'center',
                    valign: 'middle',
                    width: 50,
                    formatter: function (value, row, index) {
                        let pageSize = $('#' + videoDeviceList.mainTableId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + videoDeviceList.mainTableId).bootstrapTable("getOptions").pageNumber;
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
                    field: 'isActive',
                    title: '状态',
                    align: 'center',
                    valign: 'middle',
                    width: 80,
                    visible: true,
                    formatter: function (value, row, index) {
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
                    title: '操作',
                    align: 'center',
                    valign: 'middle',
                    width: 80,
                    formatter: function (value, row, index) {
                        let btnHtml = "<button class='btn-link' title='修改' ' onclick='devLinkList.devLinkdelete(\"" + row.id + "\")'>";
                        btnHtml += "<i class='fa fa-trash' style='font-size:18px;'></i>";
                        btnHtml += "</button>";
                        return btnHtml;
                    }
                }
            ],
        });
    },
    //refreshTable: function () {
    //    $('#' + devLinkList.mainTableId).bootstrapTable('refresh');
    //},
    devLinkdelete: function (id) {
        let ids = new Array();
        ids.push(id);
        isas.ajax({
            //请求地址
            url: AppServiceUrl.VideoDevEquipmentInfo_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            isHideSuccessMsg: true,
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        let arr = devLinkList.datas.filter(item => item.id != id);
                        devLinkList.datas = arr;
                        devLinkList.initTable();
                    }
                }
            },
        });
    },
    devLinkclick: function (value) {
        devLinkList.editModalVue.videoDevId = value;
        devLinkList.getRemoteData();
        $('#' + devLinkList.editModalId).modal('show');
    },
    getRemoteData: function () {
        let node = substationTree.getSelectSubstationNode();
        let c = {
            pageIndex: 1, // 每页显示数据的开始行号
            pageSize: 300, // 每页要显示的数据条数
            searchCondition: {
                videoDevId: devLinkList.editModalVue.videoDevId,
                transformerSubstationId: node ? node.id : null,
            },
        }
        isas.ajax({
            //请求地址
            url: AppServiceUrl.VideoDevEquipmentInfo_FindDatas,
            //数据，json字符串
            data: JSON.stringify(c),
            isHideSuccessMsg: true,
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        devLinkList.datas = rst.result.resultDatas;
                        devLinkList.initTable();
                    }
                }
            }
        });
    },
    initEditModalByVue: function () {
        devLinkList.editModalVue = new Vue({
            el: '#' + devLinkList.editModalId,
            data: {
                id: null,
                devType: "",
                devTypeValues: [],
                devEquipment: '',
                devEquipmentValues: [],
                videoDevId: '',
            },
            mounted: function () {
            },
            methods: {
                save: function (event) {
                    let node = substationTree.getSelectSubstationNode();
                    let data = {
                        id: this.id,
                        EquipmentInfoId: this.devEquipment,
                        VideoDevId: this.videoDevId,
                        TransformerSubstationId: node ? node.id : null,
                        isActive: true,
                    };
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.VideoDevEquipmentInfo_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        isHideSuccessMsg: true,
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    devLinkList.getRemoteData();
                                }
                            }
                        }
                    });
                },
                //选择设备类型
                setdevTypeAction: function (para) {
                    if (para == "") {
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
            },
        })
    },
}