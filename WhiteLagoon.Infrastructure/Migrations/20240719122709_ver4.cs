using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WhiteLagoon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ver4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "CreateDate", "Description", "ImageUrl", "Name", "Occupancy", "Price", "Sqft", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 19, 15, 27, 9, 170, DateTimeKind.Local).AddTicks(5395), "250m2 Full deniz manzarali 3 odali", "https://placehold.co/600x400", "Royal Villa", 4, 500.0, 250, null },
                    { 2, new DateTime(2024, 7, 19, 15, 27, 9, 170, DateTimeKind.Local).AddTicks(5414), "350m2 Full deniz manzarali 3 odali", "https://placehold.co/600x400", "Premium Pool Villa", 4, 600.0, 350, null },
                    { 3, new DateTime(2024, 7, 19, 15, 27, 9, 170, DateTimeKind.Local).AddTicks(5417), "400m2 Full deniz manzarali 3 odali", "https://placehold.co/600x400", "Luxury Villa", 4, 700.0, 400, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
