using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bel_Souvenirs.Migrations
{
    /// <inheritdoc />
    public partial class productQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "productQuantity",
                table: "Products",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productQuantity",
                table: "Products");
        }
    }
}
