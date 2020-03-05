$.validator.setDefaults({
    unhighlight: function (element) {
        $(element).closest('.form-group').removeClass('has-success').removeClass('has-error');
    },
    highlight: function (element) {
        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
    },
    success: function (element) {
        $(element).closest('.form-group').removeClass('has-error').addClass('has-success');
    },
    errorElement: "span",
    errorPlacement: function (error, element) {
        //error.html("<i class='fa fa-warning label- danger'></i>" + error.html());
        if (element.is(":radio") || element.is(":checkbox")) {
            error.appendTo(element.parent().parent().parent());
        } else {
            error.addClass("pull-right");
            error.css("padding-right", "25px");
            error.appendTo(element.parent().parent());
        }
    },
    errorClass: "help-block m-b-none",
    validClass: "help-block m-b-none"
});
isas = {
    ajax: function (options) {
        if (!options || !options.url) return;
        if (options.confirm) {
            layer.confirm(options.confirm.content ? options.confirm.content : '您确定该操作么？', {
                icon: options.confirm.icon ? options.confirm.icon : 3,
                btn: options.confirm.btn ? options.confirm.btn : ['确定', '取消'], //按钮
                //shade: true,
                shade: [0.3, 'gray'],//显示遮罩
                title: options.confirm.title ? title : "<i class='fa fa-warning label- danger'></i>&nbsp;&nbsp;提示"
            }, function () {
                isas.ajaxFun(options);
            });
        } else
            isas.ajaxFun(options);
    },
    ajaxFun: function (options) {
        var loadIndex = '';
        $.ajax({
            //请求方式
            type: options.type ? options.type : "post",
            //请求的媒体类型
            contentType: options.contentType ? options.contentType : "application/json;charset=UTF-8",
            //请求地址
            url: options.url,
            //数据，json字符串
            data: options.data,
            dataType: options.dataType ? options.dataType : "json",
            async: options.async ? true : !options.async == false,
            beforeSend: function (requestContext) {
                if (!options.isHideLoad)
                    loadIndex = layer.load(1, {
                        shade: [0.3, 'gray'] //0.1透明度的灰色背景
                    });
                if (options.beforeSend)
                    options.beforeSend(requestContext);
            },
            //请求成功
            success: function (rst) {
                if (options.success)
                    options.success(rst);
                if (rst.result) {
                    if (!rst.result.flag || !options.isHideSuccessMsg) {
                        layer.alert(rst.result.message);
                    }
                } else if (!rst.success)
                    layer.alert(rst.error.message);
            },
            complete: function (rst) {
                if (options.complete)
                    options.complete(rst);
                if (!options.isHideLoad)
                    layer.close(loadIndex);
            },
            //请求失败，包含具体的错误信息
            error: function (e) {
                if (options.error)
                    options.error(e);
                if (e.responseJSON && !e.responseJSON.success)
                    layer.alert(e.responseJSON.error.message);
            }
        });
    },
    bootstrapTable: function (options) {
        if (!options || !options.el) return;
        let pSize = options.pageSize ? options.pageSize : 18;
        $(options.el).bootstrapTable({
            height: options.height ? options.height : '',
            //请求服务端数据 Start
            method: 'post',
            contentType: "application/json",
            url: options.url ? options.url : "",// 获取表格数据的url
            dataType: "json",
            ajaxOptions: options.ajaxOptions,
            isInitData: options.isInitData == undefined ? true : options.isInitData == true,
            totalField: "totalCount",
            dataField: "resultDatas",//这是返回的json数组的key.默认是"rows".这里只有前后端约定好就行
            //请求服务端数据 End
            //url为空的情况下，加载固定数据 Start
            data: options.data ? options.data : "",
            //url为空的情况下，加载固定数据 End
            sidePagination: options.url ? 'server' : 'client', //设置为服务器端分页     客户端：client
            cache: false, // 设置为 false 禁用 AJAX 数据缓存， 默认为true
            striped: true,  //表格显示条纹，默认为false
            singleSelect: options.singleSelect,//设置True 将禁止多选
            clickToSelect: true,//设置true 将在点击行时，自动选择rediobox 和 checkbox
            pagination: true,
            paginationLoop: false,
            showExtendedPagination: true,
            pageList: options.pageList ? options.pageList : [pSize, pSize * 2, pSize * 3], // 如果设置了分页，设置可供选择的页面数据条数。设置为All 则显示所有记录。
            pageSize: pSize, // 页面数据条数
            pageNumber: 1, // 初始化加载第一页，默认第一页
            iconSize: 'outline',
            paginationVAlign: 'bottom',
            uniqueId: options.uniqueId ? options.uniqueId : 'id',
            toolbar: options.toolBarEl != undefined ? options.toolBarEl : false,
            toolbarAlign: options.toolbarAlign ? options.toolbarAlign : 'left',
            search: options.search ? options.search : false,
            showRefresh: options.showRefresh ? options.showRefresh : false,
            //showColumns: true,
            //showPaginationSwitch: true,
            //showFullscreen: true,
            detailView: options.detailView != undefined ? options.detailView : false,//是否显示父子表
            showToggle: options.showToggle != undefined ? options.showToggle : true,
            icons: {
                refresh: 'fa fa-refresh',
                //columns: 'fa fa-th-list',
                //fullscreen: 'fa fa-arrows-alt',
                detailOpen: 'fa fa-plus',
                detailClose: 'fa fa-minus',
                toggleOff: 'fa fa-toggle-off',
                toggleOn: 'fa fa-toggle-on'
            },
            //classes: "table table-striped table-bordered table-hover table-condensed",
            buttonsClass: "btn btn-outline btn-primary",
            theadClasses: options.theadClasses ? options.theadClasses : "thead-blue",
            showHeader: true,//是否显示列头。
            trimOnSearch: true,//设置为 true 将自动去掉搜索字符的前后空格。
            rowStyle: function (row, index) {
                let style = null;
                if (options.rowStyle)
                    style = options.rowStyle;
                else
                    style = {
                        // css: { 'height': '10px', 'font-size': 'small', 'classes':'danger'}
                    };
                return style;
            },
            queryParams: function (params) { // 请求服务器数据时发送的参数，可以在这里添加额外的查询参数，返回false则终止请求
                let c = "";
                if (options.queryParams)
                    c = options.queryParams(params)
                else
                    c = {
                        "pageIndex": params.offset / params.limit + 1, // 每页显示数据的开始行号
                        "pageSize": params.limit, // 每页要显示的数据条数
                    }
                return c
            },
            columns: options.columns,
            responseHandler: function (res) {
                //console.log("responseHandler");
                if (options.responseHandler)
                    return options.responseHandler(res);
                else if (res.success)
                    return res.result
                else
                    return {
                        totalCount: 0,
                        resultDatas: []
                    };
            },
            onLoadSuccess: function (result) {  //加载成功时执行
                //console.log("onLoadSuccess");
                if (options.onLoadSuccess)
                    return options.onLoadSuccess(result);
            },
            onLoadError: function () {  //加载失败时执行
                //console.log("onLoadError");
                if (options.onLoadError)
                    return options.onLoadError();
            },
            onClickRow: function (row, $element) {
                if (options.onClickRow)
                    return options.onClickRow(row, $element);
            },
            onCheck: function (row, $element) {
                if (options.onCheck)
                    return options.onCheck(row, $element);
            },
            onUncheck: function (row, $element) {
                if (options.onUncheck)
                    return options.onUncheck(row, $element);
            },
            onExpandRow: function (index, row, $detail) {
                if (options.onExpandRow)
                    return options.onExpandRow(index, row, $detail);
            }
        });
        if (options.search && options.searchPlaceholder)
            $(options.el).parent().parent().parent().find(".search-input").attr("placeholder", options.searchPlaceholder);
    }
}

Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "H+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "S+": this.getMilliseconds()
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(String(o[k]).length)));
        }
    }
    return fmt;
};