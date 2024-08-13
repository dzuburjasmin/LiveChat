using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveChat.Migrations
{
    /// <inheritdoc />
    public partial class userTableInheritedTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "USERS",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "USERS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "USERS",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "USERS",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "USERS",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "USERS",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "USERS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "USERS",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "USERS");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "USERS");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "USERS",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "USERS",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
