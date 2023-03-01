using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CharacterBuilder.Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_item_creatorlink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Weapons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_CreatedById",
                table: "Weapons",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreatedById",
                table: "Items",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_CreatedById",
                table: "Items",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_AspNetUsers_CreatedById",
                table: "Weapons",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_CreatedById",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_AspNetUsers_CreatedById",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Weapons_CreatedById",
                table: "Weapons");

            migrationBuilder.DropIndex(
                name: "IX_Items_CreatedById",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Items");
        }
    }
}
