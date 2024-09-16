using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestDataPopulator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Idioma");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    ParserType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsImplementedForLearning = table.Column<bool>(type: "bit", nullable: false),
                    IsImplementedForUI = table.Column<bool>(type: "bit", nullable: false),
                    IsImplementedForTranslation = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SourceURI = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Book_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Verb",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Conjugator = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Infinitive = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Core1 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Core2 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Core3 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Core4 = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Gerund = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PastParticiple = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verb_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Word",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextLowerCase = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Romanization = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Word", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Word_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LanguageUser",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalWordsRead = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LanguageUser_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageUser_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Idioma",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSetting",
                schema: "Idioma",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetting", x => new { x.UserId, x.Key });
                    table.ForeignKey(
                        name: "FK_UserSetting_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Idioma",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookStat",
                schema: "Idioma",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookStat", x => new { x.BookId, x.Key });
                    table.ForeignKey(
                        name: "FK_BookStat_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "Idioma",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookTag",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookTag_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "Idioma",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookTag_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Idioma",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
                    OriginalText = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Page_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "Idioma",
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordRank",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DifficultyScore = table.Column<decimal>(type: "numeric(8,2)", nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordRank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordRank_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordRank_Word_WordId",
                        column: x => x.WordId,
                        principalSchema: "Idioma",
                        principalTable: "Word",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WordTranslation",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VerbId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartOfSpeech = table.Column<int>(type: "int", nullable: false),
                    Ordinal = table.Column<int>(type: "int", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordTranslation_Language_LanguageToId",
                        column: x => x.LanguageToId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordTranslation_Verb_VerbId",
                        column: x => x.VerbId,
                        principalSchema: "Idioma",
                        principalTable: "Verb",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WordTranslation_Word_WordId",
                        column: x => x.WordId,
                        principalSchema: "Idioma",
                        principalTable: "Word",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookUserStat",
                schema: "Idioma",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    ValueString = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ValueNumeric = table.Column<decimal>(type: "numeric(10,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookUserStat", x => new { x.BookId, x.LanguageUserId, x.Key });
                    table.ForeignKey(
                        name: "FK_BookUserStat_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "Idioma",
                        principalTable: "Book",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookUserStat_LanguageUser_LanguageUserId",
                        column: x => x.LanguageUserId,
                        principalSchema: "Idioma",
                        principalTable: "LanguageUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordUser",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    StatusChanged = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Translation = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordUser_LanguageUser_LanguageUserId",
                        column: x => x.LanguageUserId,
                        principalSchema: "Idioma",
                        principalTable: "LanguageUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordUser_Word_WordId",
                        column: x => x.WordId,
                        principalSchema: "Idioma",
                        principalTable: "Word",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookUser",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookUser_Book_BookId",
                        column: x => x.BookId,
                        principalSchema: "Idioma",
                        principalTable: "Book",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookUser_LanguageUser_LanguageUserId",
                        column: x => x.LanguageUserId,
                        principalSchema: "Idioma",
                        principalTable: "LanguageUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookUser_Page_CurrentPageId",
                        column: x => x.CurrentPageId,
                        principalSchema: "Idioma",
                        principalTable: "Page",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Paragraph",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paragraph", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paragraph_Page_PageId",
                        column: x => x.PageId,
                        principalSchema: "Idioma",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBreadCrumb",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBreadCrumb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBreadCrumb_Page_PageId",
                        column: x => x.PageId,
                        principalSchema: "Idioma",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBreadCrumb_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Idioma",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashCard",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WordUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    NextReview = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCard_WordUser_WordUserId",
                        column: x => x.WordUserId,
                        principalSchema: "Idioma",
                        principalTable: "WordUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageUser",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReadDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageUser_BookUser_BookUserId",
                        column: x => x.BookUserId,
                        principalSchema: "Idioma",
                        principalTable: "BookUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageUser_Page_PageId",
                        column: x => x.PageId,
                        principalSchema: "Idioma",
                        principalTable: "Page",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParagraphTranslation",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParagraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TranslationText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParagraphTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParagraphTranslation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Idioma",
                        principalTable: "Language",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParagraphTranslation_Paragraph_ParagraphId",
                        column: x => x.ParagraphId,
                        principalSchema: "Idioma",
                        principalTable: "Paragraph",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sentence",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParagraphId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sentence_Paragraph_ParagraphId",
                        column: x => x.ParagraphId,
                        principalSchema: "Idioma",
                        principalTable: "Paragraph",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashCardAttempt",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlashCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AttemptedWhen = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCardAttempt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCardAttempt_FlashCard_FlashCardId",
                        column: x => x.FlashCardId,
                        principalSchema: "Idioma",
                        principalTable: "FlashCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashCardParagraphTranslationBridge",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlashCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParagraphTranslationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCardParagraphTranslationBridge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashCardParagraphTranslationBridge_FlashCard_FlashCardId",
                        column: x => x.FlashCardId,
                        principalSchema: "Idioma",
                        principalTable: "FlashCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashCardParagraphTranslationBridge_ParagraphTranslation_ParagraphTranslationId",
                        column: x => x.ParagraphTranslationId,
                        principalSchema: "Idioma",
                        principalTable: "ParagraphTranslation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Token",
                schema: "Idioma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SentenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Display = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Ordinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Token_Sentence_SentenceId",
                        column: x => x.SentenceId,
                        principalSchema: "Idioma",
                        principalTable: "Sentence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Token_Word_WordId",
                        column: x => x.WordId,
                        principalSchema: "Idioma",
                        principalTable: "Word",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Book_LanguageId",
                schema: "Idioma",
                table: "Book",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTag_BookId",
                schema: "Idioma",
                table: "BookTag",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTag_UserId_BookId_Tag",
                schema: "Idioma",
                table: "BookTag",
                columns: new[] { "UserId", "BookId", "Tag" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_BookId_LanguageUserId",
                schema: "Idioma",
                table: "BookUser",
                columns: new[] { "BookId", "LanguageUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_CurrentPageId",
                schema: "Idioma",
                table: "BookUser",
                column: "CurrentPageId");

            migrationBuilder.CreateIndex(
                name: "IX_BookUser_LanguageUserId",
                schema: "Idioma",
                table: "BookUser",
                column: "LanguageUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookUserStat_LanguageUserId",
                schema: "Idioma",
                table: "BookUserStat",
                column: "LanguageUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCard_WordUserId",
                schema: "Idioma",
                table: "FlashCard",
                column: "WordUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardAttempt_FlashCardId",
                schema: "Idioma",
                table: "FlashCardAttempt",
                column: "FlashCardId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardParagraphTranslationBridge_FlashCardId",
                schema: "Idioma",
                table: "FlashCardParagraphTranslationBridge",
                column: "FlashCardId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCardParagraphTranslationBridge_ParagraphTranslationId_FlashCardId",
                schema: "Idioma",
                table: "FlashCardParagraphTranslationBridge",
                columns: new[] { "ParagraphTranslationId", "FlashCardId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Language_Code",
                schema: "Idioma",
                table: "Language",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageUser_LanguageId_UserId",
                schema: "Idioma",
                table: "LanguageUser",
                columns: new[] { "LanguageId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguageUser_UserId",
                schema: "Idioma",
                table: "LanguageUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Page_BookId_Ordinal",
                schema: "Idioma",
                table: "Page",
                columns: new[] { "BookId", "Ordinal" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageUser_BookUserId",
                schema: "Idioma",
                table: "PageUser",
                column: "BookUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PageUser_PageId_BookUserId",
                schema: "Idioma",
                table: "PageUser",
                columns: new[] { "PageId", "BookUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paragraph_PageId_Ordinal",
                schema: "Idioma",
                table: "Paragraph",
                columns: new[] { "PageId", "Ordinal" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParagraphTranslation_LanguageId",
                schema: "Idioma",
                table: "ParagraphTranslation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ParagraphTranslation_ParagraphId_LanguageId",
                schema: "Idioma",
                table: "ParagraphTranslation",
                columns: new[] { "ParagraphId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sentence_ParagraphId_Ordinal",
                schema: "Idioma",
                table: "Sentence",
                columns: new[] { "ParagraphId", "Ordinal" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Token_SentenceId_Ordinal",
                schema: "Idioma",
                table: "Token",
                columns: new[] { "SentenceId", "Ordinal" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Token_WordId",
                schema: "Idioma",
                table: "Token",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ApplicationUserId",
                schema: "Idioma",
                table: "User",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBreadCrumb_PageId",
                schema: "Idioma",
                table: "UserBreadCrumb",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBreadCrumb_UserId",
                schema: "Idioma",
                table: "UserBreadCrumb",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Verb_LanguageId",
                schema: "Idioma",
                table: "Verb",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Word_LanguageId_TextLowerCase",
                schema: "Idioma",
                table: "Word",
                columns: new[] { "LanguageId", "TextLowerCase" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordRank_LanguageId_Ordinal",
                schema: "Idioma",
                table: "WordRank",
                columns: new[] { "LanguageId", "Ordinal" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordRank_LanguageId_WordId",
                schema: "Idioma",
                table: "WordRank",
                columns: new[] { "LanguageId", "WordId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordRank_WordId",
                schema: "Idioma",
                table: "WordRank",
                column: "WordId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslation_LanguageToId",
                schema: "Idioma",
                table: "WordTranslation",
                column: "LanguageToId");

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslation_VerbId",
                schema: "Idioma",
                table: "WordTranslation",
                column: "VerbId");

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslation_WordId",
                schema: "Idioma",
                table: "WordTranslation",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordUser_LanguageUserId",
                schema: "Idioma",
                table: "WordUser",
                column: "LanguageUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WordUser_WordId_LanguageUserId",
                schema: "Idioma",
                table: "WordUser",
                columns: new[] { "WordId", "LanguageUserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BookStat",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "BookTag",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "BookUserStat",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "FlashCardAttempt",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "FlashCardParagraphTranslationBridge",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "PageUser",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Token",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "UserBreadCrumb",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "UserSetting",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "WordRank",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "WordTranslation",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "FlashCard",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "ParagraphTranslation",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "BookUser",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Sentence",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Verb",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "WordUser",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Paragraph",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "LanguageUser",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Word",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Page",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Book",
                schema: "Idioma");

            migrationBuilder.DropTable(
                name: "Language",
                schema: "Idioma");
        }
    }
}
