using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCheckoutInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalWeightKg",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "WeightKg",
                table: "cart_items");

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckOut",
                table: "carts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCheckOut",
                table: "carts");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWeightKg",
                table: "carts",
                type: "numeric(10,3)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightKg",
                table: "cart_items",
                type: "numeric(10,3)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
