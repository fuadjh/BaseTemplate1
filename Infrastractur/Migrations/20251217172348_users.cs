using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LmsUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profile_FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Profile_LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Profile_NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Profile_FatherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Profile_AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Email_Address = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Mobile_Number = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    MobileConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LmsUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UX_LmsUser_Email",
                table: "LmsUsers",
                column: "Email_Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_LmsUser_Mobile",
                table: "LmsUsers",
                column: "Mobile_Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_LmsUser_NationalCode",
                table: "LmsUsers",
                column: "Profile_NationalCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LmsUsers");
        }
    }
}
