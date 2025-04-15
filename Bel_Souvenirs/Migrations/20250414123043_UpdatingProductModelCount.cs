using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bel_Souvenirs.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingProductModelCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmountOfOrders",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfOrders",
                table: "Products");
        }
    }
}
