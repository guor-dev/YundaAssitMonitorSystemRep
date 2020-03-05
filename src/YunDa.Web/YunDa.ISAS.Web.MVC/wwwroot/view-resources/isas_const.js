var BaseUrl = "/api/services/isas/";
var AppServiceUrl = {
    //获取用户数据
    User_FindDatas: BaseUrl + "User/FindDatas",
    //添加或修改用户信息
    User_CreateOrUpdate: BaseUrl + "User/CreateOrUpdate",
    //根据ID删除用户，多条删除
    User_DeleteByIds: BaseUrl + "User/DeleteByIds",

    //获取厂商数据
    ManufacturerInfo_FindDatas: BaseUrl + "ManufacturerInfo/FindDatas",
    //添加或修改厂商
    ManufacturerInfo_CreateOrUpdate: BaseUrl + "ManufacturerInfo/CreateOrUpdate",
    //根据ID删除厂商，多条删除
    ManufacturerInfo_DeleteByIds: BaseUrl + "ManufacturerInfo/DeleteByIds",
    //根据ID删除厂商，多条软删除
    ManufacturerInfo_SoftDeleteByIds: BaseUrl + "ManufacturerInfo/SoftDeleteByIds",
    //根据ID恢复厂商，多条恢复
    ManufacturerInfo_RecoverByIds: BaseUrl + "ManufacturerInfo/RecoverByIds",
    //获取厂商选择数据
    ManufacturerInfo_FindManufacturerInfoForSelect: BaseUrl + "ManufacturerInfo/FindManufacturerInfoForSelect",

    //获取线路数据
    PowerSupplyLine_FindDatas: BaseUrl + "PowerSupplyLine/FindDatas",
    //添加或修改线路
    PowerSupplyLine_CreateOrUpdate: BaseUrl + "PowerSupplyLine/CreateOrUpdate",
    //根据ID删除线路，多条删除
    PowerSupplyLine_DeleteByIds: BaseUrl + "PowerSupplyLine/DeleteByIds",

    //获取变电所数据
    TransformerSubstation_FindDatas: BaseUrl + "TransformerSubstation/FindDatas",
    //添加或修改变电所
    TransformerSubstation_CreateOrUpdate: BaseUrl + "TransformerSubstation/CreateOrUpdate",
    //根据ID删除变电所，多条删除
    TransformerSubstation_DeleteByIds: BaseUrl + "TransformerSubstation/DeleteByIds",

    //获取摄像头预置点数据
    PresetPoint_FindDatas: BaseUrl + "PresetPoint/FindDatas",
    //获取摄像头预置下拉框列表数据
    PresetPoint_FindPresetPointsForSelect: BaseUrl + "PresetPoint/FindPresetPointsForSelect",

    //获取巡检任务单
    InspectionCard_FindDatas: BaseUrl + "InspectionCard/FindDatas",
    //添加或修改巡检任务单
    InspectionCard_CreateOrUpdate: BaseUrl + "InspectionCard/CreateOrUpdate",
    //根据ID删除巡检任务单，单条删除
    InspectionCard_DeleteById: BaseUrl + "InspectionCard/DeleteById",
    //获取巡检任务项
    InspectionItem_FindDatas: BaseUrl + "InspectionItem/FindDatas",
    //巡检项 巡检过程中执行的活动的下拉框选择数据
    InspectionItem_FindProcessActionsForSelect: BaseUrl + "InspectionItem/FindProcessActionsForSelect",
    //添加或修改巡检任务项
    InspectionItem_CreateOrUpdate: BaseUrl + "InspectionItem/CreateOrUpdate",
    //根据ID删除巡检任务项，多条删除
    InspectionItem_DeleteByIds: BaseUrl + "InspectionItem/DeleteByIds",
    //获取巡检计划任务数据
    InspectionPlanTask_FindDatas: BaseUrl + "InspectionPlanTask/FindDatas",
    //添加或修改巡检计划任务
    InspectionPlanTask_CreateOrUpdate: BaseUrl + "InspectionPlanTask/CreateOrUpdate",
    //根据ID删除巡检任务单，单条删除
    InspectionPlanTask_DeleteById: BaseUrl + "InspectionPlanTask/DeleteById",
    InspectionPlanTask_CopyTaskByIds: BaseUrl + "InspectionPlanTask/CopyTaskByIds",

    //获取设备信息
    EquipmentInfo_FindDatas: BaseUrl + "EquipmentInfo/FindDatas",
    EquipmentInfo_DeleteById: BaseUrl + "EquipmentInfo/DeleteById",
    EquipmentInfo_DeleteByIds: BaseUrl + "EquipmentInfo/DeleteByIds",
    EquipmentInfo_CreateOrUpdate: BaseUrl + "EquipmentInfo/CreateOrUpdate",
    EquipmentInfo_FindEquipmentInfoForSelect: BaseUrl + "EquipmentInfo/FindEquipmentInfoForSelect",
    EquipmentInfo_FindEquipmentById: BaseUrl + "EquipmentInfo/FindEquipmentById",

    //获取设备类型信息
    EquipmentType_FindDatas: BaseUrl + "EquipmentType/FindDatas",
    EquipmentType_DeleteById: BaseUrl + "EquipmentType/DeleteById",
    EquipmentType_DeleteByIds: BaseUrl + "EquipmentType/DeleteByIds",
    EquipmentType_CreateOrUpdate: BaseUrl + "EquipmentType/CreateOrUpdate",
    EquipmentType_FindEquipmentTypeForSelect: BaseUrl + "EquipmentType/FindEquipmentTypeForSelect",

    //获取视频设备信息
    VideoDev_FindDatas: BaseUrl + "VideoDev/FindDatas",
    VideoDev_DeleteById: BaseUrl + "VideoDev/DeleteById",
    VideoDev_DeleteByIds: BaseUrl + "VideoDev/DeleteByIds",
    VideoDev_CreateOrUpdate: BaseUrl + "VideoDev/CreateOrUpdate",
    VideoDev_FindDevTypeForSelect: BaseUrl + "VideoDev/FindDevTypeForSelect",
    VideoDev_FindVideoDevForSelect: BaseUrl + "VideoDev/FindVideoDevForSelect",
    VideoDev_FindVideoDevById: BaseUrl + "VideoDev/FindVideoDevById",

    //获取监控位置信息
    VideoDevEquipmentInfo_FindDatas: BaseUrl + "VideoDevEquipmentInfo/FindDatas",
    VideoDevEquipmentInfo_DeleteByIds: BaseUrl + "VideoDevEquipmentInfo/DeleteByIds",
    VideoDevEquipmentInfo_CreateOrUpdate: BaseUrl + "VideoDevEquipmentInfo/CreateOrUpdate",
    VideoDevEquipmentInfo_FindVideoTerminalForSelect: BaseUrl + "VideoDevEquipmentInfo/FindVideoTerminalForSelect",

    //获取系统功能信息
    Function_FindFunctionTypeForTree: BaseUrl + "Function/FindFunctionTypeForTree",
    Function_FindTypesForSelect: BaseUrl + "Function/FindTypesForSelect",
    Function_FindDatas: BaseUrl + "Function/FindDatas",
    Function_CreateOrUpdate: BaseUrl + "Function/CreateOrUpdate",
    Function_DeleteByIds: BaseUrl + "Function/DeleteByIds",



}