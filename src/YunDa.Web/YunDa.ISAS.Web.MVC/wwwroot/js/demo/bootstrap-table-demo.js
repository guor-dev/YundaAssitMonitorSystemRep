(function (document, window, $) {
    'use strict';

    // Example Bootstrap Table Events
    // ------------------------------
    (function () {
        $('#exampleTableEvents').bootstrapTable({
            url: "/js/demo/bootstrap_table_test.json",
            search: true,
            pagination: true,
            showRefresh: true,
            showToggle: true,
            showColumns: true,
            //showPaginationSwitch: true,
            showFullscreen: true,
            detailView: true,
            iconSize: 'outline',
            toolbar: '#exampleTableEventsToolbar',
            icons: {
                //refresh: 'glyphicon-repeat',
                //toggleOn: 'glyphicon-list-alt',
                //toggleOff: 'glyphicon-list-alt',
                //columns: 'glyphicon-list'

                //paginationSwitchDown: 'fa fa-caret-down',
                //paginationSwitchUp: 'fa fa-caret-up',
                refresh: 'fa fa-refresh',
                toggleOff: 'fa fa-toggle-off',
                toggleOn: 'fa fa-toggle-on',
                columns: 'fa fa-th-list',
                fullscreen: 'fa fa-arrows-alt',
                detailOpen: 'fa fa-plus',
                detailClose: 'fa fa-minus'
            }
        });
        var $result = $('#examplebtTableEventsResult');

        $('#exampleTableEvents').on('all.bs.table', function (e, name, args) {
            console.log('Event:', name, ', data:', args);
        })
            .on('click-row.bs.table', function (e, row, $element) {
                $result.text('Event: click-row.bs.table');
            })
            .on('dbl-click-row.bs.table', function (e, row, $element) {
                $result.text('Event: dbl-click-row.bs.table');
            })
            .on('sort.bs.table', function (e, name, order) {
                $result.text('Event: sort.bs.table');
            })
            .on('check.bs.table', function (e, row) {
                $result.text('Event: check.bs.table');
            })
            .on('uncheck.bs.table', function (e, row) {
                $result.text('Event: uncheck.bs.table');
            })
            .on('check-all.bs.table', function (e) {
                $result.text('Event: check-all.bs.table');
            })
            .on('uncheck-all.bs.table', function (e) {
                $result.text('Event: uncheck-all.bs.table');
            })
            .on('load-success.bs.table', function (e, data) {
                $result.text('Event: load-success.bs.table');
            })
            .on('load-error.bs.table', function (e, status) {
                $result.text('Event: load-error.bs.table');
            })
            .on('column-switch.bs.table', function (e, field, checked) {
                $result.text('Event: column-switch.bs.table');
            })
            .on('page-change.bs.table', function (e, size, number) {
                $result.text('Event: page-change.bs.table');
            })
            .on('search.bs.table', function (e, text) {
                $result.text('Event: search.bs.table');
            });
    })();
})(document, window, jQuery);