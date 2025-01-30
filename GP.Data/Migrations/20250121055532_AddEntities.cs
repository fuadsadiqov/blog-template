using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cmn");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "cmn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Uuid = table.Column<string>(type: "text", nullable: true),
                    CreatedById = table.Column<string>(type: "character varying(128)", nullable: true),
                    ModifiedById = table.Column<string>(type: "character varying(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "cmn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                schema: "cmn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CoverImage = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "cmn",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogTags",
                schema: "cmn",
                columns: table => new
                {
                    BlogId = table.Column<Guid>(type: "uuid", maxLength: 128, nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", maxLength: 128, nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTags", x => new { x.BlogId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BlogTags_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalSchema: "cmn",
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "cmn",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CategoryId",
                schema: "cmn",
                table: "Blogs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTags_TagId",
                schema: "cmn",
                table: "BlogTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CreatedById",
                schema: "cmn",
                table: "Categories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ModifiedById",
                schema: "cmn",
                table: "Categories",
                column: "ModifiedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogTags",
                schema: "cmn");

            migrationBuilder.DropTable(
                name: "Blogs",
                schema: "cmn");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "cmn");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "cmn");
        }
    }
}
