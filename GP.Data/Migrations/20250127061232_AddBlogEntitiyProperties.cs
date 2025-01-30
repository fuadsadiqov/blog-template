using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogEntitiyProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                schema: "cmn",
                table: "Blogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                schema: "cmn",
                table: "Blogs");
        }
    }
}
