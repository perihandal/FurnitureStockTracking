using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class pricetablesconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PriceHistories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceDefinitionId1",
                table: "PriceHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistories_PriceDefinitionId1",
                table: "PriceHistories",
                column: "PriceDefinitionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceHistories_PriceDefinitions_PriceDefinitionId1",
                table: "PriceHistories",
                column: "PriceDefinitionId1",
                principalTable: "PriceDefinitions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceHistories_PriceDefinitions_PriceDefinitionId1",
                table: "PriceHistories");

            migrationBuilder.DropIndex(
                name: "IX_PriceHistories_PriceDefinitionId1",
                table: "PriceHistories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PriceHistories");

            migrationBuilder.DropColumn(
                name: "PriceDefinitionId1",
                table: "PriceHistories");
        }
    }
}
