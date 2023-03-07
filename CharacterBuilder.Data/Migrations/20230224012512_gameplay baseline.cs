using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharacterBuilder.Data.Migrations
{
    /// <inheritdoc />
    public partial class gameplaybaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameMasterId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUTC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_AspNetUsers_GameMasterId",
                        column: x => x.GameMasterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LowAttackRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HighAttackRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowThrownRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighThrownRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttackingStat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetStat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsTwoHanded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampaignPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignPlayers_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignPlayers_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    CampaignId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    MindScore = table.Column<int>(type: "int", nullable: false),
                    BodyScore = table.Column<int>(type: "int", nullable: false),
                    ResilienceScore = table.Column<int>(type: "int", nullable: false),
                    SoulScore = table.Column<int>(type: "int", nullable: false),
                    MovementScore = table.Column<int>(type: "int", nullable: false),
                    CurrentHp = table.Column<int>(type: "int", nullable: false),
                    CurrentTalentPoints = table.Column<int>(type: "int", nullable: false),
                    CurrentMovementPoints = table.Column<int>(type: "int", nullable: false),
                    WeaponProficiencies = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characters_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InventorySlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    ItemCount = table.Column<int>(type: "int", nullable: false),
                    WeaponId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventorySlots_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventorySlots_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventorySlots_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignPlayers_CampaignId",
                table: "CampaignPlayers",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignPlayers_PlayerId",
                table: "CampaignPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_GameMasterId",
                table: "Campaigns",
                column: "GameMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CampaignId",
                table: "Characters",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_OwnerId",
                table: "Characters",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySlots_CharacterId",
                table: "InventorySlots",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySlots_ItemId",
                table: "InventorySlots",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySlots_WeaponId",
                table: "InventorySlots",
                column: "WeaponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignPlayers");

            migrationBuilder.DropTable(
                name: "InventorySlots");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "Campaigns");
        }
    }
}
