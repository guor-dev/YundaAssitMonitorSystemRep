Vue.component("vue-clockpicker", {
    props: ['default_value', 'time_placement', 'time_align'],
    template: '<div class="input-group clockpicker">'
        + '<input type="text" class="form-control" placeholder="请选择时间" readonly="readonly" v-model="default_value">'
        + '<span class="input-group-addon">'
        + '<i class="fa fa-clock-o"></i>'
        + '</span>'
        + '</div>',
    created: function () {
    },
    mounted: function () {
        let _this = this;
        let id = $(_this.$el).attr('id');
        let name = $(_this.$el).attr('name');
        $(_this.$el).find("input").attr('id', id);
        $(_this.$el).find("input").attr('name', name);
        $(_this.$el).attr('id', '');
        $(_this.$el).attr('name', '');
        $(_this.$el).clockpicker({
            placement: this.time_placement ? this.time_placement : 'bottom',
            align: this.time_align ? this.time_align : 'left',
            autoclose: true,
            afterDone: function () {
            }
        }).find("input").change(function () {
            _this.$emit('change', this.value);
        });
    }
});