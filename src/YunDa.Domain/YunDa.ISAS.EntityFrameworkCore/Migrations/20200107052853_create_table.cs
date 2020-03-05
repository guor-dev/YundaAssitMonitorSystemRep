using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace YunDa.ISAS.Migrations
{
    public partial class create_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gi_equipment_type",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gi_equipment_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "gi_manufacturer_info",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    DeleterUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ManufacturerName = table.Column<string>(maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 29, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 50, nullable: true),
                    ManufacturerAddress = table.Column<string>(maxLength: 200, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gi_manufacturer_info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "gi_power_supply_line",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    LineName = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gi_power_supply_line", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_function",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    SeqNo = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    LoadUrl = table.Column<string>(maxLength: 200, nullable: true),
                    Icon = table.Column<string>(maxLength: 40, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    SysFunctionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_function", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    RoleName = table.Column<string>(maxLength: 50, nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sys_user",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false),
                    RealName = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 29, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "gi_transformer_substation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    SubstationName = table.Column<string>(maxLength: 100, nullable: false),
                    CommMgrIP = table.Column<string>(maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    PowerSupplyLineId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gi_transformer_substation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_gi_transformer_substation_gi_power_supply_line_PowerSupplyLi~",
                        column: x => x.PowerSupplyLineId,
                        principalTable: "gi_power_supply_line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sys_role_function",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    SysFunctionId = table.Column<Guid>(nullable: true),
                    SysRoleId = table.Column<Guid>(nullable: true),
                    IsEdit = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_role_function", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sys_role_function_sys_function_SysFunctionId",
                        column: x => x.SysFunctionId,
                        principalTable: "sys_function",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sys_role_function_sys_role_SysRoleId",
                        column: x => x.SysRoleId,
                        principalTable: "sys_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sys_user_role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    SysUserId = table.Column<Guid>(nullable: true),
                    SysRoleId = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_user_role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sys_user_role_sys_role_SysRoleId",
                        column: x => x.SysRoleId,
                        principalTable: "sys_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sys_user_role_sys_user_SysRoleId",
                        column: x => x.SysRoleId,
                        principalTable: "sys_user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gi_equipment_info",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Code = table.Column<string>(nullable: true),
                    InstallationDate = table.Column<DateTime>(nullable: true),
                    ProductionDate = table.Column<DateTime>(nullable: true),
                    EquipmentTypeId = table.Column<Guid>(nullable: true),
                    ManufacturerInfoId = table.Column<Guid>(nullable: true),
                    TransformerSubstationId = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gi_equipment_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_gi_equipment_info_gi_equipment_type_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "gi_equipment_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gi_equipment_info_gi_manufacturer_info_ManufacturerInfoId",
                        column: x => x.ManufacturerInfoId,
                        principalTable: "gi_manufacturer_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gi_equipment_info_gi_transformer_substation_TransformerSubst~",
                        column: x => x.TransformerSubstationId,
                        principalTable: "gi_transformer_substation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vs_inspection_card",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    CardName = table.Column<string>(maxLength: 100, nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    TransformerSubstationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vs_inspection_card", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vs_inspection_card_gi_transformer_substation_TransformerSubs~",
                        column: x => x.TransformerSubstationId,
                        principalTable: "gi_transformer_substation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vs_video_dev",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    SeqNo = table.Column<int>(nullable: true),
                    DevName = table.Column<string>(maxLength: 100, nullable: false),
                    DevType = table.Column<int>(nullable: false),
                    ManufacturerInfoId = table.Column<Guid>(nullable: true),
                    InstallationDate = table.Column<DateTime>(nullable: true),
                    ProductionDate = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    IP = table.Column<string>(maxLength: 20, nullable: true),
                    Port = table.Column<int>(nullable: true),
                    DevUserName = table.Column<string>(maxLength: 30, nullable: true),
                    DevPassword = table.Column<string>(maxLength: 30, nullable: true),
                    ChannelNo = table.Column<int>(nullable: true),
                    DevNo = table.Column<int>(nullable: true),
                    IsPTZ = table.Column<bool>(nullable: false),
                    CodeStreamType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    TransformerSubstationId = table.Column<Guid>(nullable: true),
                    VideoDevId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vs_video_dev", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vs_video_dev_gi_manufacturer_info_ManufacturerInfoId",
                        column: x => x.ManufacturerInfoId,
                        principalTable: "gi_manufacturer_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vs_video_dev_gi_transformer_substation_TransformerSubstation~",
                        column: x => x.TransformerSubstationId,
                        principalTable: "gi_transformer_substation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vs_inspection_plan_task",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    SeqNo = table.Column<int>(nullable: false),
                    PlanTaskName = table.Column<string>(maxLength: 100, nullable: true),
                    ExecutionWeek = table.Column<int>(nullable: false),
                    ExecutionTime = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    InspectionCardId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vs_inspection_plan_task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vs_inspection_plan_task_vs_inspection_card_InspectionCardId",
                        column: x => x.InspectionCardId,
                        principalTable: "vs_inspection_card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vs_preset_point",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    VideoDevId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vs_preset_point", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vs_preset_point_vs_video_dev_VideoDevId",
                        column: x => x.VideoDevId,
                        principalTable: "vs_video_dev",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vs_video_dev_equipment_info",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    DeleterUserId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EquipmentInfoId = table.Column<Guid>(nullable: true),
                    VideoDevId = table.Column<Guid>(nullable: true),
                    TransformerSubstationId = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vs_video_dev_equipment_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vs_video_dev_equipment_info_gi_equipment_info_EquipmentInfoId",
                        column: x => x.EquipmentInfoId,
                        principalTable: "gi_equipment_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vs_video_dev_equipment_info_gi_transformer_substation_Transf~",
                        column: x => x.TransformerSubstationId,
                        principalTable: "gi_transformer_substation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vs_video_dev_equipment_info_vs_video_dev_VideoDevId",
                        column: x => x.VideoDevId,
                        principalTable: "vs_video_dev",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vs_inspection_Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatorUserId = table.Column<Guid>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<Guid>(nullable: true),
                    SeqNo = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    ProcessAction = table.Column<int>(nullable: false),
                    ProcessDuration = table.Column<int>(nullable: false),
                    IsImageRecognition = table.Column<bool>(nullable: false),
                    InspectionCardId = table.Column<Guid>(nullable: true),
                    VideoDevId = table.Column<Guid>(nullable: true),
                    PresetPointId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vs_inspection_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vs_inspection_Item_vs_inspection_card_InspectionCardId",
                        column: x => x.InspectionCardId,
                        principalTable: "vs_inspection_card",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vs_inspection_Item_vs_preset_point_PresetPointId",
                        column: x => x.PresetPointId,
                        principalTable: "vs_preset_point",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vs_inspection_Item_vs_video_dev_VideoDevId",
                        column: x => x.VideoDevId,
                        principalTable: "vs_video_dev",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gi_equipment_info_EquipmentTypeId",
                table: "gi_equipment_info",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_gi_equipment_info_ManufacturerInfoId",
                table: "gi_equipment_info",
                column: "ManufacturerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_gi_equipment_info_TransformerSubstationId",
                table: "gi_equipment_info",
                column: "TransformerSubstationId");

            migrationBuilder.CreateIndex(
                name: "IX_gi_transformer_substation_PowerSupplyLineId",
                table: "gi_transformer_substation",
                column: "PowerSupplyLineId");

            migrationBuilder.CreateIndex(
                name: "IX_sys_role_function_SysFunctionId",
                table: "sys_role_function",
                column: "SysFunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_sys_role_function_SysRoleId",
                table: "sys_role_function",
                column: "SysRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_sys_user_role_SysRoleId",
                table: "sys_user_role",
                column: "SysRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_inspection_card_TransformerSubstationId",
                table: "vs_inspection_card",
                column: "TransformerSubstationId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_inspection_Item_InspectionCardId",
                table: "vs_inspection_Item",
                column: "InspectionCardId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_inspection_Item_PresetPointId",
                table: "vs_inspection_Item",
                column: "PresetPointId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_inspection_Item_VideoDevId",
                table: "vs_inspection_Item",
                column: "VideoDevId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_inspection_plan_task_InspectionCardId",
                table: "vs_inspection_plan_task",
                column: "InspectionCardId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_preset_point_VideoDevId",
                table: "vs_preset_point",
                column: "VideoDevId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_video_dev_ManufacturerInfoId",
                table: "vs_video_dev",
                column: "ManufacturerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_video_dev_TransformerSubstationId",
                table: "vs_video_dev",
                column: "TransformerSubstationId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_video_dev_equipment_info_EquipmentInfoId",
                table: "vs_video_dev_equipment_info",
                column: "EquipmentInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_video_dev_equipment_info_TransformerSubstationId",
                table: "vs_video_dev_equipment_info",
                column: "TransformerSubstationId");

            migrationBuilder.CreateIndex(
                name: "IX_vs_video_dev_equipment_info_VideoDevId",
                table: "vs_video_dev_equipment_info",
                column: "VideoDevId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_role_function");

            migrationBuilder.DropTable(
                name: "sys_user_role");

            migrationBuilder.DropTable(
                name: "vs_inspection_Item");

            migrationBuilder.DropTable(
                name: "vs_inspection_plan_task");

            migrationBuilder.DropTable(
                name: "vs_video_dev_equipment_info");

            migrationBuilder.DropTable(
                name: "sys_function");

            migrationBuilder.DropTable(
                name: "sys_role");

            migrationBuilder.DropTable(
                name: "sys_user");

            migrationBuilder.DropTable(
                name: "vs_preset_point");

            migrationBuilder.DropTable(
                name: "vs_inspection_card");

            migrationBuilder.DropTable(
                name: "gi_equipment_info");

            migrationBuilder.DropTable(
                name: "vs_video_dev");

            migrationBuilder.DropTable(
                name: "gi_equipment_type");

            migrationBuilder.DropTable(
                name: "gi_manufacturer_info");

            migrationBuilder.DropTable(
                name: "gi_transformer_substation");

            migrationBuilder.DropTable(
                name: "gi_power_supply_line");
        }
    }
}