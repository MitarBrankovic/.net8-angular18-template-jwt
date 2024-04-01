using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateBackend.Infrastructure.Data.Migrations;
/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserRefreshTokens",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRefreshTokens", x => x.Id);
            });
        
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PictureFolderName = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserRefreshTokens");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
