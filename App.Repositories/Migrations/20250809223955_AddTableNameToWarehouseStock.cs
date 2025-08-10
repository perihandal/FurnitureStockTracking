using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddTableNameToWarehouseStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseStocs_StockCards_StockCardId",
                table: "WarehouseStocs");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseStocs_Warehouses_WarehouseId",
                table: "WarehouseStocs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseStocs",
                table: "WarehouseStocs");

            migrationBuilder.RenameTable(
                name: "WarehouseStocs",
                newName: "WarehouseStocks");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseStocs_WarehouseId",
                table: "WarehouseStocks",
                newName: "IX_WarehouseStocks_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseStocs_StockCardId",
                table: "WarehouseStocks",
                newName: "IX_WarehouseStocks_StockCardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseStocks",
                table: "WarehouseStocks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseStocks_StockCards_StockCardId",
                table: "WarehouseStocks",
                column: "StockCardId",
                principalTable: "StockCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseStocks_Warehouses_WarehouseId",
                table: "WarehouseStocks",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseStocks_StockCards_StockCardId",
                table: "WarehouseStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseStocks_Warehouses_WarehouseId",
                table: "WarehouseStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseStocks",
                table: "WarehouseStocks");

            migrationBuilder.RenameTable(
                name: "WarehouseStocks",
                newName: "WarehouseStocs");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseStocks_WarehouseId",
                table: "WarehouseStocs",
                newName: "IX_WarehouseStocs_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseStocks_StockCardId",
                table: "WarehouseStocs",
                newName: "IX_WarehouseStocs_StockCardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseStocs",
                table: "WarehouseStocs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseStocs_StockCards_StockCardId",
                table: "WarehouseStocs",
                column: "StockCardId",
                principalTable: "StockCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseStocs_Warehouses_WarehouseId",
                table: "WarehouseStocs",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
