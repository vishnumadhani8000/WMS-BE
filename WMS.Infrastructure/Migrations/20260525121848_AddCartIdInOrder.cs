using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCartIdInOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CartId",
                table: "orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "orders_cart_id_idx",
                table: "orders",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_carts_CartId",
                table: "orders",
                column: "CartId",
                principalTable: "carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_carts_CartId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "orders_cart_id_idx",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "orders");
        }
    }
}
