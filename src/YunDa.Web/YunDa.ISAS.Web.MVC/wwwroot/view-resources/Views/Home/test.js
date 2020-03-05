$(function () {
    manufacturePage.initVue();

    $('#testJsDatePicker').datepicker({
        todayBtn: "linked",
        format: 'yyyy-mm-dd',
        language: 'zh-CN',
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        minuteStep: 1
    });

    $("#mongoDBAddBtn").click(function () {
        isas.ajax({
            //请求地址
            url: "/api/services/isas/TestMongoDB/TestMongoDBAdd",
            //数据，json字符串
            data: "",
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        $("#mongoDBRstInfo").html(rst.result.message);
                    }
                }
            }
        });
    });
    $("#mongoDBSearchBtn").click(function () {
        isas.ajax({
            //请求地址
            url: "/api/services/isas/TestMongoDB/TestMongoDBSearch",
            //数据，json字符串
            data: "",
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        $("#mongoDBRstInfo").html(JSON.stringify(rst.result.resultDatas));
                    }
                }
            }
        });
    });
});

var manufacturePage = {
    initVue: function () {
        manufacturePage.vueTest = new Vue({
            el: '#testApp',
            data: {
                dateValue: "2019-01-03",
                timeValue: "8:00",
                timePlacement: 'bottom',
                timeAlign: 'left'
            },
            methods: {
                setDateValue: function (v) {
                    alert(v);
                },
                setTimeValue: function (v) {
                    alert(v);
                }
            }
        });
    },
}