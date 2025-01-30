using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GP.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.EnsureSchema(
                name: "idt");

            migrationBuilder.EnsureSchema(
                name: "lng");

            migrationBuilder.EnsureSchema(
                name: "log");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "idt",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    UserType = table.Column<byte>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Uuid = table.Column<string>(type: "text", nullable: true),
                    OtpSentCount = table.Column<int>(type: "integer", nullable: false),
                    LastAccessFailedAttempt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedById = table.Column<string>(type: "character varying(128)", nullable: true),
                    ModifiedById = table.Column<string>(type: "character varying(128)", nullable: true),
                    CanAccessOutOfDomain = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppDomains",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Domain = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_AppDomains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDomains_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppDomains_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppOptions",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(128)", nullable: false),
                    IsOtpRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IsPinRequired = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_AppOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppOptions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOptions_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppOptions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailConfirmationRequests",
                schema: "idt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                    table.PrimaryKey("PK_EmailConfirmationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailConfirmationRequests_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailConfirmationRequests_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmailConfirmationRequests_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Keys",
                schema: "lng",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Label = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("PK_Keys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Keys_Keys_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "lng",
                        principalTable: "Keys",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Keys_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Keys_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "lng",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
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
                    table.PrimaryKey("PK_Languages", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Languages_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Languages_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetRequests",
                schema: "idt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                    table.PrimaryKey("PK_PasswordResetRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetRequests_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PasswordResetRequests_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PasswordResetRequests_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionCategories",
                schema: "idt",
                columns: table => new
                {
                    Label = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    VisibleLabel = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_PermissionCategories", x => x.Label);
                    table.ForeignKey(
                        name: "FK_PermissionCategories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissionCategories_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "idt",
                columns: table => new
                {
                    Label = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    VisibleLabel = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsDirective = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_Permissions", x => x.Label);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Permissions_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "idt",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Uuid = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    CreatedById = table.Column<string>(type: "character varying(128)", nullable: true),
                    ModifiedById = table.Column<string>(type: "character varying(128)", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Roles_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SmsConfirmationRequests",
                schema: "idt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    ConfirmationStatus = table.Column<int>(type: "integer", nullable: false),
                    SmsRequestTypeEnum = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SmsConfirmationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsConfirmationRequests_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmsConfirmationRequests_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmsConfirmationRequests_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmsSenderServiceLogs",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "character varying(128)", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    SmsText = table.Column<string>(type: "text", nullable: false),
                    MessageId = table.Column<string>(type: "text", nullable: false),
                    ResponseCode = table.Column<string>(type: "text", nullable: false),
                    ResponseText = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_SmsSenderServiceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmsSenderServiceLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmsSenderServiceLogs_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmsSenderServiceLogs_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "idt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "character varying(128)", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserJwts",
                schema: "idt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RevokeDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: true),
                    ReasonRevoked = table.Column<string>(type: "text", nullable: true),
                    ImpersonatorId = table.Column<string>(type: "character varying(128)", nullable: true),
                    Domain = table.Column<string>(type: "text", nullable: true),
                    RememberMe = table.Column<string>(type: "text", nullable: true),
                    Application = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "character varying(128)", nullable: false),
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
                    table.PrimaryKey("PK_UserJwts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserJwts_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserJwts_Users_ImpersonatorId",
                        column: x => x.ImpersonatorId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserJwts_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserJwts_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "idt",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "character varying(128)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "idt",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(128)", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageKeys",
                schema: "lng",
                columns: table => new
                {
                    LanguageId = table.Column<string>(type: "character varying(32)", nullable: false),
                    KeyId = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_LanguageKeys", x => new { x.LanguageId, x.KeyId });
                    table.ForeignKey(
                        name: "FK_LanguageKeys_Keys_KeyId",
                        column: x => x.KeyId,
                        principalSchema: "lng",
                        principalTable: "Keys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageKeys_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "lng",
                        principalTable: "Languages",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageKeys_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LanguageKeys_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LanguageKeyValues",
                schema: "lng",
                columns: table => new
                {
                    LanguageCode = table.Column<string>(type: "character varying(32)", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_LanguageKeyValues", x => new { x.LanguageCode, x.Key });
                    table.ForeignKey(
                        name: "FK_LanguageKeyValues_Languages_LanguageCode",
                        column: x => x.LanguageCode,
                        principalSchema: "lng",
                        principalTable: "Languages",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageKeyValues_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LanguageKeyValues_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermissionCategoryPermissions",
                schema: "idt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<string>(type: "character varying(32)", nullable: true),
                    PermissionId = table.Column<string>(type: "character varying(32)", nullable: true),
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
                    table.PrimaryKey("PK_PermissionCategoryPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionCategoryPermissions_PermissionCategories_Category~",
                        column: x => x.CategoryId,
                        principalSchema: "idt",
                        principalTable: "PermissionCategories",
                        principalColumn: "Label");
                    table.ForeignKey(
                        name: "FK_PermissionCategoryPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "idt",
                        principalTable: "Permissions",
                        principalColumn: "Label");
                    table.ForeignKey(
                        name: "FK_PermissionCategoryPermissions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PermissionCategoryPermissions_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                schema: "idt",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(128)", nullable: false),
                    PermissionId = table.Column<string>(type: "character varying(32)", nullable: false),
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
                    table.PrimaryKey("PK_UserPermissions", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "idt",
                        principalTable: "Permissions",
                        principalColumn: "Label",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "idt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "character varying(128)", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "idt",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "idt",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RoleId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
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
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "idt",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissionCategories",
                schema: "idt",
                columns: table => new
                {
                    PermissionCategoryPermissionId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
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
                    table.PrimaryKey("PK_RolePermissionCategories", x => new { x.RoleId, x.PermissionCategoryPermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissionCategories_PermissionCategoryPermissions_Perm~",
                        column: x => x.PermissionCategoryPermissionId,
                        principalSchema: "idt",
                        principalTable: "PermissionCategoryPermissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionCategories_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "idt",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissionCategories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RolePermissionCategories_Users_ModifiedById",
                        column: x => x.ModifiedById,
                        principalSchema: "idt",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDomains_CreatedById",
                schema: "app",
                table: "AppDomains",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AppDomains_ModifiedById",
                schema: "app",
                table: "AppDomains",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOptions_CreatedById",
                schema: "app",
                table: "AppOptions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOptions_ModifiedById",
                schema: "app",
                table: "AppOptions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AppOptions_UserId",
                schema: "app",
                table: "AppOptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmationRequests_CreatedById",
                schema: "idt",
                table: "EmailConfirmationRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmationRequests_ModifiedById",
                schema: "idt",
                table: "EmailConfirmationRequests",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmationRequests_UserId",
                schema: "idt",
                table: "EmailConfirmationRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Keys_CreatedById",
                schema: "lng",
                table: "Keys",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Keys_ModifiedById",
                schema: "lng",
                table: "Keys",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Keys_ParentId",
                schema: "lng",
                table: "Keys",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageKeys_CreatedById",
                schema: "lng",
                table: "LanguageKeys",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageKeys_KeyId",
                schema: "lng",
                table: "LanguageKeys",
                column: "KeyId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageKeys_ModifiedById",
                schema: "lng",
                table: "LanguageKeys",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageKeyValues_CreatedById",
                schema: "lng",
                table: "LanguageKeyValues",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageKeyValues_ModifiedById",
                schema: "lng",
                table: "LanguageKeyValues",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_CreatedById",
                schema: "lng",
                table: "Languages",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ModifiedById",
                schema: "lng",
                table: "Languages",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetRequests_CreatedById",
                schema: "idt",
                table: "PasswordResetRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetRequests_ModifiedById",
                schema: "idt",
                table: "PasswordResetRequests",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetRequests_UserId",
                schema: "idt",
                table: "PasswordResetRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionCategories_CreatedById",
                schema: "idt",
                table: "PermissionCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionCategories_ModifiedById",
                schema: "idt",
                table: "PermissionCategories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionCategoryPermissions_CategoryId",
                schema: "idt",
                table: "PermissionCategoryPermissions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionCategoryPermissions_CreatedById",
                schema: "idt",
                table: "PermissionCategoryPermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionCategoryPermissions_ModifiedById",
                schema: "idt",
                table: "PermissionCategoryPermissions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionCategoryPermissions_PermissionId",
                schema: "idt",
                table: "PermissionCategoryPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CreatedById",
                schema: "idt",
                table: "Permissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ModifiedById",
                schema: "idt",
                table: "Permissions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "idt",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionCategories_CreatedById",
                schema: "idt",
                table: "RolePermissionCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionCategories_ModifiedById",
                schema: "idt",
                table: "RolePermissionCategories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissionCategories_PermissionCategoryPermissionId",
                schema: "idt",
                table: "RolePermissionCategories",
                column: "PermissionCategoryPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedById",
                schema: "idt",
                table: "Roles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ModifiedById",
                schema: "idt",
                table: "Roles",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "idt",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmsConfirmationRequests_CreatedById",
                schema: "idt",
                table: "SmsConfirmationRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmsConfirmationRequests_ModifiedById",
                schema: "idt",
                table: "SmsConfirmationRequests",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmsConfirmationRequests_UserId",
                schema: "idt",
                table: "SmsConfirmationRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsSenderServiceLogs_CreatedById",
                schema: "log",
                table: "SmsSenderServiceLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmsSenderServiceLogs_ModifiedById",
                schema: "log",
                table: "SmsSenderServiceLogs",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmsSenderServiceLogs_UserId",
                schema: "log",
                table: "SmsSenderServiceLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "idt",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJwts_CreatedById",
                schema: "idt",
                table: "UserJwts",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserJwts_ImpersonatorId",
                schema: "idt",
                table: "UserJwts",
                column: "ImpersonatorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJwts_ModifiedById",
                schema: "idt",
                table: "UserJwts",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserJwts_Token",
                schema: "idt",
                table: "UserJwts",
                column: "Token");

            migrationBuilder.CreateIndex(
                name: "IX_UserJwts_UserId",
                schema: "idt",
                table: "UserJwts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "idt",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_CreatedById",
                schema: "idt",
                table: "UserPermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_ModifiedById",
                schema: "idt",
                table: "UserPermissions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                schema: "idt",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_CreatedById",
                schema: "idt",
                table: "UserRoles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ModifiedById",
                schema: "idt",
                table: "UserRoles",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "idt",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "idt",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                schema: "idt",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifiedById",
                schema: "idt",
                table: "Users",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "idt",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDomains",
                schema: "app");

            migrationBuilder.DropTable(
                name: "AppOptions",
                schema: "app");

            migrationBuilder.DropTable(
                name: "EmailConfirmationRequests",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "LanguageKeys",
                schema: "lng");

            migrationBuilder.DropTable(
                name: "LanguageKeyValues",
                schema: "lng");

            migrationBuilder.DropTable(
                name: "PasswordResetRequests",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "RolePermissionCategories",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "SmsConfirmationRequests",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "SmsSenderServiceLogs",
                schema: "log");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "UserJwts",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "UserPermissions",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "Keys",
                schema: "lng");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "lng");

            migrationBuilder.DropTable(
                name: "PermissionCategoryPermissions",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "PermissionCategories",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "idt");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "idt");
        }
    }
}
