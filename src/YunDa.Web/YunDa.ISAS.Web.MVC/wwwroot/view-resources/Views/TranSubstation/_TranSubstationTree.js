var substationTree = {
    treeId: null,
    selectNode: null,
    treeChanged: null,
    initTree: function (treeChangedFun) {
        substationTree.treeId = "lineTree";
        substationTree.treeChanged = treeChangedFun;
        $('#' + substationTree.treeId).jstree({ core: { data: null } });
        let condtion = {
            sorting: "lineName",
            searchCondition: {
                isNeedChildren: true,
                isOnlyActive: true
            }
        }
        isas.ajax({
            isHideSuccessMsg: true,
            //请求地址
            url: AppServiceUrl.PowerSupplyLine_FindDatas,
            //数据，json字符串
            data: JSON.stringify(condtion),
            //请求成功
            success: function (rst) {
                if (rst.result) {
                    if (rst.result.flag) {
                        let rstData = rst.result.resultDatas;
                        if (!rstData || rstData.length == 0) return;
                        let len = rstData.length;
                        var treeData = [];
                        for (let i = 0; i < len; i++) {
                            let rstLineData = rstData[i];
                            let lineNode = {
                                id: rstLineData.id,
                                text: rstLineData.lineName,
                                type: "line",
                                state: {
                                    opened: i == 0
                                }
                            };
                            let lineNodeChildren = null;
                            if (rstLineData.transformerSubstations && rstLineData.transformerSubstations.length > 0) {
                                lineNodeChildren = [];
                                let sublen = rstLineData.transformerSubstations.length;
                                for (let j = 0; j < sublen; j++) {
                                    let rstSubData = rstLineData.transformerSubstations[j];
                                    lineNodeChildren.push({
                                        id: rstSubData.id,
                                        text: rstSubData.substationName,
                                        type: "substation",
                                        //icon: 'none',
                                        state: {
                                            selected: i == 0 && j == 0
                                        }
                                    });
                                }
                            };
                            lineNode.children = lineNodeChildren;
                            treeData.push(lineNode);
                        };
                        substationTree.setTree(treeData);
                    }
                }
            },
        });
    },
    setTree: function (treeData) {
        let tree = $('#' + substationTree.treeId);
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
                line: {
                    icon: 'fa fa-home',
                },
                substation: {
                    icon: "fa fa-cubes",
                }
            }
        });
        tree.on("changed.jstree", function (e, data) {
            let node = data.instance.get_selected(true)[0];
            if (!node || (substationTree.selectNode && substationTree.selectNode.id == node.id)) return;
            substationTree.selectNode = null;
            if (node.type == "substation") {
                substationTree.selectNode = node;
                if (substationTree.treeChanged)
                    substationTree.treeChanged(node);
            }
        });
    },
    getSelectSubstationNode: function () {
        return substationTree.selectNode;
    }
}