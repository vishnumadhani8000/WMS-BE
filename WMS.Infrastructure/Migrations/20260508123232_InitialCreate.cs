using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LicenceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_drivers_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_drivers_users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_drivers_users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "states",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_states", x => x.Id);
                    table.ForeignKey(
                        name: "FK_states_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_states_users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_states_users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PlateNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CapacityKg = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vehicles_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicles_users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vehicles_users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StateId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cities_states_StateId",
                        column: x => x.StateId,
                        principalTable: "states",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cities_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cities_users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cities_users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shipments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DriverId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TotalWeightKg = table.Column<decimal>(type: "numeric(12,3)", nullable: false),
                    DispatchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shipments_drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shipments_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shipments_users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shipments_users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shipments_vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_addresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    StateId = table.Column<long>(type: "bigint", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    AddressLine = table.Column<string>(type: "text", nullable: false),
                    Landmark = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Pincode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_addresses_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_addresses_states_StateId",
                        column: x => x.StateId,
                        principalTable: "states",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_addresses_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AddressId = table.Column<long>(type: "bigint", nullable: false),
                    ShipmentId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    TotalWeightKg = table.Column<decimal>(type: "numeric(12,3)", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orders_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_orders_shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_user_addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "user_addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitWeightKg = table.Column<decimal>(type: "numeric(10,3)", nullable: false),
                    LineWeightKg = table.Column<decimal>(type: "numeric(12,3)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_items_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_items_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "cities_deleted_at_idx",
                table: "cities",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "cities_state_name_uidx",
                table: "cities",
                columns: new[] { "StateId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cities_CreatedBy",
                table: "cities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_cities_DeletedBy",
                table: "cities",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_cities_UpdatedBy",
                table: "cities",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "drivers_available_idx",
                table: "drivers",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "drivers_deleted_at_idx",
                table: "drivers",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_CreatedBy",
                table: "drivers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_DeletedBy",
                table: "drivers",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_drivers_UpdatedBy",
                table: "drivers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "order_items_order_id_idx",
                table: "order_items",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "order_items_product_id_idx",
                table: "order_items",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_CityId",
                table: "orders",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_CreatedBy",
                table: "orders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_orders_DeletedBy",
                table: "orders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_orders_UpdatedBy",
                table: "orders",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "orders_address_id_idx",
                table: "orders",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "orders_deleted_at_idx",
                table: "orders",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "orders_shipment_id_idx",
                table: "orders",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "orders_status_idx",
                table: "orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "orders_user_id_idx",
                table: "orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_products_CreatedBy",
                table: "products",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_products_DeletedBy",
                table: "products",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_products_UpdatedBy",
                table: "products",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "products_deleted_at_idx",
                table: "products",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "products_name_idx",
                table: "products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_shipments_CreatedBy",
                table: "shipments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_shipments_DeletedBy",
                table: "shipments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_shipments_UpdatedBy",
                table: "shipments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "shipments_deleted_at_idx",
                table: "shipments",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "shipments_driver_id_idx",
                table: "shipments",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "shipments_status_idx",
                table: "shipments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "shipments_vehicle_id_idx",
                table: "shipments",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_states_CreatedBy",
                table: "states",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_states_DeletedBy",
                table: "states",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_states_UpdatedBy",
                table: "states",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "states_deleted_at_idx",
                table: "states",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "states_name_uidx",
                table: "states",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_addresses_city_id_idx",
                table: "user_addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "user_addresses_deleted_at_idx",
                table: "user_addresses",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "user_addresses_state_id_idx",
                table: "user_addresses",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "user_addresses_user_id_idx",
                table: "user_addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "users_deleted_at_idx",
                table: "users",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "users_email_uidx",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_role_idx",
                table: "users",
                column: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_CreatedBy",
                table: "vehicles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_DeletedBy",
                table: "vehicles",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_vehicles_UpdatedBy",
                table: "vehicles",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "vehicles_available_idx",
                table: "vehicles",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "vehicles_deleted_at_idx",
                table: "vehicles",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "vehicles_plate_uidx",
                table: "vehicles",
                column: "PlateNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "shipments");

            migrationBuilder.DropTable(
                name: "user_addresses");

            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "states");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
