using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Changes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdaterId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: "69DB714F-9576-45BA-B5B7-F00649BE01DE",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1f00920c-134d-4c66-92ca-e0143341c64f", "AQAAAAIAAYagAAAAEFJbb1tb9lIBbikCjVTifur/w1RL5fnw9qU+9ebLrRZe5wONlAqFstzfuLa3lOksig==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.UpdateData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: "69DB714F-9576-45BA-B5B7-F00649BE01DE",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e72382c3-4659-4881-9a3a-1ba9a952331a", "AQAAAAIAAYagAAAAEKKDj/6XvtX7+b+zvu5/xCk7GuNOxAqAM4yawC8pDE7BWm6sxsiELUUAblPT3x7vDw==" });
        }
    }
}
