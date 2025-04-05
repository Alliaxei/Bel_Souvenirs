using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bel_Souvenirs.Migrations
{
    /// <inheritdoc />
    public partial class productQuantity1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productQuantity",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "productQuantity",
                table: "Products",
                type: "integer",
                nullable: true);
        }
    }
}
