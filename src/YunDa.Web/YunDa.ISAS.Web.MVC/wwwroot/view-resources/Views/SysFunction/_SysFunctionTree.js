var sysFunctionTree = {
    treeId: "functionTree",
    selectNode: null,
    treeChanged: null,
    types: [],
    initTree: function (treeChangedFun) {
        IsInited = true;
        sysFunctionTree.treeChanged = treeChangedFun;
        $('#' + sysFunctionTree.treeId).jstree({ core: { data: null } });
        isas.ajax({
            isHideSuccessMsg: true,
            //请求地址
            url: AppServiceUrl.Function_FindFunctionTypeForTree,
            //数据，json字符串
            data: JSON.stringify({}),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        let rstData = rst.result.resultDatas;
                        if (!rstData || rstData.length == 0) return;
                        sysFunctionTree.setTree(rstData);
                        //console.log(rstData)
                    }
                }
            },
        });
    },
    setTree: function (treeData) {
        let tree = $('#' + sysFunctionTree.treeId);
        tree.data('jstree', false).empty();
        tree.unbind() // tree解绑
        tree.jstree({
            core: {
                data: treeData
            },
            plugins: ['types'],
            types: {
                default: {
                },
                0: {
                    icon: 'fa fa-home',
                },
                1: {
                    icon: "fa fa-cubes",
                }
            }
        });
        tree.on("changed.jstree", function (e, data) {
            let node = data.instance.get_selected(true)[0];
            if (!node || (sysFunctionTree.selectNode && sysFunctionTree.selectNode.id == node.id)) return;
            //sysFunctionTree.selectNode = null;
            //sysFunctionTree.selectNode = node;
            //if (sysFunctionTree.treeChanged)
            //    sysFunctionTree.treeChanged(node);
        });
        tree.on("select_node.jstree", function (e, data) {
            let node = data.node;
            sysFunctionTree.treeChanged(node);
        });
    },
    getSelectsysFunctionNode: function () {
        return sysFunctionTree.selectNode;
    }
}