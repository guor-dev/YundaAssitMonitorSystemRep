Vue.component("vue-datepicker", {
    props: ['default_value', 'format', 'language'],
    template: '<div class="input-group date">'
        + '<input type="text" class="form-control datetimepicker" placeholder="请选择时间" readonly="readonly" v-model="default_value">'
        + '<span class="input-group-addon">'
        + '<i class="fa fa-calendar"></i>'
        + '</span>'
        + '</div>',
    mounted: function () {
        let _this = this;
        let id = $(_this.$el).attr('id');
        let name = $(_this.$el).attr('name');
        $(_this.$el).find("input").attr('id', id);
        $(_this.$el).find("input").attr('name', name);
        $(_this.$el).attr('id', '');
        $(_this.$el).attr('name', '');
        $(_this.$el).datepicker({
            todayBtn: "linked",
            format: _this.format ? _this.format : 'yyyy-mm-dd',
            language: _this.language ? _this.language : 'zh-CN',
            keyboardNavigation: false,
            forceParse: false,
            calendarWeeks: true,
            autoclose: true,
            minuteStep: 1
        }).find("input").change(function () {
            _this.$emit('change', this.value);
        });
    }
});