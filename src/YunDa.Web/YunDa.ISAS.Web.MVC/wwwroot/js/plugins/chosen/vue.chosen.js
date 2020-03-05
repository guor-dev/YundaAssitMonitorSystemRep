Vue.component("vue-chosen", {
    props: ['default_value', 'options', 'placeholder'],
    watch: {
        options: function (newValue, oldValue) {
            this.$nextTick(function () {
                $(this.$el).val(this.default_value);
                $(this.$el).trigger("chosen:updated");
            });
        },
        default_value: function (newValue, oldValue) {
            if (newValue !== oldValue) {
                this.$nextTick(function () {
                    $(this.$el).val(newValue);
                    $(this.$el).trigger("chosen:updated");
                })
            }
        }
    },
    computed: {
    },
    methods: {
    },
    template: '<select style=""><option></option><option v-for="item in options" :value="item.value" :key="item.key">{{ item.text }}</option></select>',
    created: function () {
        //this.hashId = Math.floor(Math.random() * Math.pow(10, 8)).toString(16);
    },
    beformounted: function () {
    },
    mounted: function () {
        var _this = this;
        $(_this.$el).val(_this.default_value);
        $(_this.$el).
            chosen({
                width: '100%',
                disable_search: false,
                search_contains: true,
                allow_single_deselect: true,
                inherit_select_classes: true,
                no_results_text: 'No Results... ...',
                placeholder_text_single: _this.placeholder
            }).
            change(function () {
                _this.$emit('change', this.value);
            });
    }
});