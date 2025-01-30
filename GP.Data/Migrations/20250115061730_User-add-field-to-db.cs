using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Data.Migrations
{
    /// <inheritdoc />
    public partial class Useraddfieldtodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullNameAz",
                schema: "idt",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullNameEn",
                schema: "idt",
                table: "Users",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullNameAz",
                schema: "idt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FullNameEn",
                schema: "idt",
                table: "Users");
        }
    }
}
