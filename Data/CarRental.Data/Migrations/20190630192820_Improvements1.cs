using Microsoft.EntityFrameworkCore.Migrations;

namespace CarRental.Data.Migrations
{
    public partial class Improvements1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PickUpLocationId",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnLocationId",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PickUpLocationId",
                table: "Orders",
                column: "PickUpLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReturnLocationId",
                table: "Orders",
                column: "ReturnLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Locations_PickUpLocationId",
                table: "Orders",
                column: "PickUpLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Locations_ReturnLocationId",
                table: "Orders",
                column: "ReturnLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Locations_PickUpLocationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Locations_ReturnLocationId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PickUpLocationId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ReturnLocationId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PickUpLocationId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReturnLocationId",
                table: "Orders");
        }
    }
}
