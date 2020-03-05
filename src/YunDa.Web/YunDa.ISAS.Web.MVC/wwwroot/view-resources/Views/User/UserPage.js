$(function () {
    userPage.initEditModalByVue("editModal");
    userPage.initEditFormValidate("editForm");
    $("#searchBtn").click(function () {
        $('#' + userPage.mainTableElementId).bootstrapTable('refresh');
    });
    $("#addBtn").click(function () {
        userPage.initEditModalValues("");
    });
    $("#delBtn").click(function () {
        userPage.delUser();
    });
    userPage.initUserTable("tableUser", "tableUserToolbar");
});

var userPage = {
    mainTableElementId: null,
    editModalVue: null,
    editFormId: null,
    initUserTable: function (tableId, toolBarId) {
        userPage.mainTableElementId = tableId;
        if (!userPage.mainTableElementId) return;
        //let tableHeight = document.body.clientHeight - 10;
        //tableHeight = tableHeight < 200 ? 200 : tableHeight;
        isas.bootstrapTable({
            el: '#' + userPage.mainTableElementId,
            toolBarEl: '#' + toolBarId,
            url: AppServiceUrl.User_FindDatas,
            //height: tableHeight,
            toolbarAlign: 'left',
            showToggle: 'true',
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let c = {
                    "pageIndex": params.offset / params.limit + 1, // 每页显示数据的开始行号
                    "pageSize": params.limit, // 每页要显示的数据条数
                    "searchCondition": {
                        "userName": $("#searchUserName").val()
                    },
                    "sorting": "userName"
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
                        //let pageSize = $('#' + userPage.mainTableElementId).bootstrapTable("getOptions").pageSize;
                        //let pageNumber = $('#' + userPage.mainTableElementId).bootstrapTable("getOptions").pageNumber;
                        return value;
                    }
                },
                {
                    title: '行号',
                    align: 'center',
                    valign: 'middle',
                    width: 50,
                    formatter: function (value, row, index) {
                        let pageSize = $('#' + userPage.mainTableElementId).bootstrapTable("getOptions").pageSize;
                        let pageNumber = $('#' + userPage.mainTableElementId).bootstrapTable("getOptions").pageNumber;
                        return (pageNumber - 1) * pageSize + 1 + index;
                    }
                },
                {
                    field: 'userName', // 要显示数据的字段名称，可以理解为json对象里的key
                    title: '用户名', // 表格表头显示文字
                    align: 'center', // 左右居中
                    valign: 'middle', // 上下居中
                    visible: true
                },
                {
                    field: 'realName',
                    title: '真实姓名',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'phoneNumber',
                    title: '电话号码',
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'emailAddress',
                    title: '邮件地址',
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
                , {
                    title: "操作",
                    align: 'center',
                    valign: 'middle',
                    width: 80, // 定义列的宽度，单位为像素px
                    formatter: function (value, row, index) {
                        let btnHtml = "<button class='btn-link' title='修改' data-toggle='modal' data-target='#editModal' onclick='userPage.initEditModalValues(\"" + row.id + "\")'>";
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
        userPage.editModalVue = new Vue({
            el: '#' + editModalId,
            data: {
                id: null,
                header: '编辑用户',
                userName: "",
                password: "",
                confirmPassword: "",
                realName: "",
                phoneNumber: "",
                emailAddress: "",
                isActive: true
            },
            watch: {
                userName: function (newValue, oldValue) {
                    if (this.realName == oldValue)
                        this.realName = newValue;
                }
            },
            methods: {
                saveUser: function (event) {
                    if (!userPage.mainTableElementId || !userPage.editFormId) return;
                    if (!$("#" + userPage.editFormId).valid()) return;
                    let data = {
                        id: this.id,
                        userName: this.userName,
                        password: this.password,
                        realName: this.realName,
                        phoneNumber: this.phoneNumber,
                        emailAddress: this.emailAddress,
                        isActive: this.isActive
                    }
                    isas.ajax({
                        //请求地址
                        url: AppServiceUrl.User_CreateOrUpdate,
                        //数据，json字符串
                        data: JSON.stringify(data),
                        //请求成功
                        success: function (rst) {
                            if (rst.result) {
                                if (rst.result.flag) {
                                    $('#' + userPage.mainTableElementId).bootstrapTable('refresh');
                                    $(userPage.editModalVue.$el).modal('hide');
                                }
                                layer.alert(rst.result.message);
                            }
                        }
                    });
                }
            }
        })
    },
    initEditFormValidate: function (editFormId) {
        if (!editFormId) return;
        userPage.editFormId = editFormId
        $("#" + editFormId).validate({
            rules: {
                userName: {
                    required: true,
                },
                password: {
                    required: true,
                },
                confirmPassword: {
                    required: true,
                    //rangelength: [6, 12],
                    /*重复密码需要与原密码相同的要求*/
                    equalTo: "#password"
                },
                realName: {
                    required: true,
                },
                phoneNumber: {
                    required: false,
                    phone: true,
                },
                emailAddress: {
                    email: true,
                },
            },
            messages: {
                userName: {
                    required: "登录名不能为空",
                },
                password: {
                    required: "密码不能为空",
                },
                confirmPassword: {
                    required: "确认密码不能为空",
                    //rangelength: "重复密码长度在6-12位之间",
                    equalTo: "确认密码与密码不一致"
                },
                realName: {
                    required: "真实姓名不能为空"
                },
                phoneNumber: {
                    phone: "请输入有效手机号"
                },
                emailAddress: {
                    email: "请输入有效邮箱"
                }
            }
        });
        /*手机号格式验证*/
        $.validator.addMethod("phone", function (value, element, params) {
            var reg = /^1[34578]\d{9}$/;
            return this.optional(element) || reg.test(value);
        }, "phone error");
    },
    initEditModalValues: function (uniqueId) {
        if (!userPage.editModalVue || !userPage.mainTableElementId) return;
        var rowData = null;
        if (uniqueId)
            rowData = $('#' + userPage.mainTableElementId).bootstrapTable('getRowByUniqueId', uniqueId);

        userPage.resetFormValidate();
        if (!rowData) {
            rowData = {
                id: null,
                userName: "",
                password: "",
                realName: "",
                phoneNumber: "",
                emailAddress: "",
                isActive: true
            }
            userPage.editModalVue.header = '添加用户';
        } else
            userPage.editModalVue.header = '编辑用户';
        userPage.editModalVue.id = rowData.id;
        userPage.editModalVue.userName = rowData.userName;
        userPage.editModalVue.password = rowData.password;
        userPage.editModalVue.confirmPassword = rowData.password;
        userPage.editModalVue.realName = rowData.realName;
        userPage.editModalVue.phoneNumber = rowData.phoneNumber;
        userPage.editModalVue.emailAddress = rowData.emailAddress;
        userPage.editModalVue.isActive = rowData.isActive;
    },
    //重置验证
    resetFormValidate: function () {
        $("#" + userPage.editFormId).validate().resetForm();
        $("#" + userPage.editFormId).find('.form-group').removeClass('has-success').removeClass('has-error');
    },
    delUser: function () {
        if (!userPage.mainTableElementId) return;
        let row = $("#" + userPage.mainTableElementId).bootstrapTable('getSelections');
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
            url: AppServiceUrl.User_DeleteByIds,
            //数据，json字符串
            data: JSON.stringify(ids),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        $('#' + userPage.mainTableElementId).bootstrapTable('refresh');
                    }
                    layer.alert(rst.result.message);
                } else if (!rst.success)
                    layer.alert(rst.error.message);
            },
        });
    }
}