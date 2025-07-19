using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedFieldToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "PasswordHash" },
                values: new object[] { new DateTime(2025, 7, 19, 13, 14, 3, 541, DateTimeKind.Utc).AddTicks(9582), "$2a$12$Ftxjkz2aPKUi8zUU69qOFORJPfAeQ8RSNkp8rYxjx9/BR/YoqKL6S" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "PasswordHash" },
                values: new object[] { new DateTime(2025, 7, 15, 18, 29, 30, 101, DateTimeKind.Utc).AddTicks(1299), "$2a$12$Qk9vayFSwJ1pJGbwgS7RIeE1jLrCQqmJgRYv5V9mhxOwHKyEcxkbq" });
        }
    }
}
