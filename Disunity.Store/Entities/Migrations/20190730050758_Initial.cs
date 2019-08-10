using System;
using Disunity.Store.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Disunity.Store.Entities.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:mod_dependency_type", "dependency,optional_dependency,incompatible")
                .Annotation("Npgsql:Enum:org_member_role", "owner,admin,member");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orgs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    ShowUsers = table.Column<bool>(nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orgs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoredFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ObjectId = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VersionNumbers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Major = table.Column<int>(nullable: false),
                    Minor = table.Column<int>(nullable: false),
                    Patch = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionNumbers", x => x.ID);
                    table.UniqueConstraint("AK_VersionNumbers_Major_Minor_Patch", x => new { x.Major, x.Minor, x.Patch });
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ShadowOrgId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Orgs_ShadowOrgId",
                        column: x => x.ShadowOrgId,
                        principalTable: "Orgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisunityVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Url = table.Column<string>(nullable: true),
                    VersionNumberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisunityVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisunityVersions_VersionNumbers_VersionNumberId",
                        column: x => x.VersionNumberId,
                        principalTable: "VersionNumbers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnityVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VersionNumberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnityVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnityVersions_VersionNumbers_VersionNumberId",
                        column: x => x.VersionNumberId,
                        principalTable: "VersionNumbers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
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
                name: "OrgMembers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    OrgId = table.Column<int>(nullable: false),
                    Role = table.Column<OrgMemberRole>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgMembers", x => new { x.UserId, x.OrgId });
                    table.ForeignKey(
                        name: "FK_OrgMembers_Orgs_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Orgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisunityVersionCompatibilities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VersionId = table.Column<int>(nullable: false),
                    MinCompatibleVersionId = table.Column<int>(nullable: true),
                    MaxCompatibleVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisunityVersionCompatibilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DisunityVersionCompatibilities_UnityVersions_MaxCompatibleV~",
                        column: x => x.MaxCompatibleVersionId,
                        principalTable: "UnityVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisunityVersionCompatibilities_UnityVersions_MinCompatibleV~",
                        column: x => x.MinCompatibleVersionId,
                        principalTable: "UnityVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisunityVersionCompatibilities_DisunityVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "DisunityVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModDisunityCompatibilities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VersionId = table.Column<int>(nullable: false),
                    MinCompatibleVersionId = table.Column<int>(nullable: true),
                    MaxCompatibleVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModDisunityCompatibilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModDisunityCompatibilities_DisunityVersions_MaxCompatibleVe~",
                        column: x => x.MaxCompatibleVersionId,
                        principalTable: "DisunityVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModDisunityCompatibilities_DisunityVersions_MinCompatibleVe~",
                        column: x => x.MinCompatibleVersionId,
                        principalTable: "DisunityVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModDependencies",
                columns: table => new
                {
                    DependentId = table.Column<int>(nullable: false),
                    DependencyId = table.Column<int>(nullable: false),
                    DependencyType = table.Column<ModDependencyType>(nullable: false),
                    MinVersionId = table.Column<int>(nullable: true),
                    MaxVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModDependencies", x => new { x.DependentId, x.DependencyId });
                });

            migrationBuilder.CreateTable(
                name: "ModVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ModId = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: false),
                    Downloads = table.Column<int>(nullable: true, defaultValue: 0),
                    VersionNumberId = table.Column<int>(nullable: false),
                    WebsiteUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    Readme = table.Column<string>(nullable: false),
                    FileId = table.Column<string>(maxLength: 1024, nullable: false),
                    IconUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModVersions_VersionNumbers_VersionNumberId",
                        column: x => x.VersionNumberId,
                        principalTable: "VersionNumbers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModVersionDownloadEvents",
                columns: table => new
                {
                    ModVersionId = table.Column<int>(nullable: false),
                    SourceIp = table.Column<string>(nullable: false),
                    LatestDownload = table.Column<DateTime>(nullable: false),
                    TotalDownloads = table.Column<int>(nullable: true, defaultValue: 1),
                    CountedDownloads = table.Column<int>(nullable: true, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModVersionDownloadEvents", x => new { x.SourceIp, x.ModVersionId });
                    table.ForeignKey(
                        name: "FK_ModVersionDownloadEvents_ModVersions_ModVersionId",
                        column: x => x.ModVersionId,
                        principalTable: "ModVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    Slug = table.Column<string>(maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: true),
                    IsDeprecated = table.Column<bool>(nullable: true, defaultValue: false),
                    IsPinned = table.Column<bool>(nullable: true, defaultValue: false),
                    LatestId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    TargetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mods", x => x.Id);
                    table.UniqueConstraint("AK_Mods_OwnerId_Slug", x => new { x.OwnerId, x.Slug });
                    table.ForeignKey(
                        name: "FK_Mods_ModVersions_LatestId",
                        column: x => x.LatestId,
                        principalTable: "ModVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mods_Orgs_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Orgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModTargetCompatibilities",
                columns: table => new
                {
                    VersionId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false),
                    MinCompatibleVersionId = table.Column<int>(nullable: true),
                    MaxCompatibleVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModTargetCompatibilities", x => new { x.VersionId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_ModTargetCompatibilities_ModVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "ModVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TargetVersions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TargetId = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    VersionNumber = table.Column<string>(maxLength: 16, nullable: false),
                    WebsiteUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    IconUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    Hash = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetVersions", x => x.ID);
                    table.UniqueConstraint("AK_TargetVersions_TargetId_VersionNumber", x => new { x.TargetId, x.VersionNumber });
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LatestId = table.Column<int>(nullable: true),
                    Slug = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Targets_TargetVersions_LatestId",
                        column: x => x.LatestId,
                        principalTable: "TargetVersions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TargetVersionCompatibilities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VersionId = table.Column<int>(nullable: false),
                    MinCompatibleVersionId = table.Column<int>(nullable: true),
                    MaxCompatibleVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetVersionCompatibilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TargetVersionCompatibilities_UnityVersions_MaxCompatibleVer~",
                        column: x => x.MaxCompatibleVersionId,
                        principalTable: "UnityVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TargetVersionCompatibilities_UnityVersions_MinCompatibleVer~",
                        column: x => x.MinCompatibleVersionId,
                        principalTable: "UnityVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TargetVersionCompatibilities_TargetVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "TargetVersions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShadowOrgId",
                table: "AspNetUsers",
                column: "ShadowOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_DisunityVersionCompatibilities_MaxCompatibleVersionId",
                table: "DisunityVersionCompatibilities",
                column: "MaxCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DisunityVersionCompatibilities_MinCompatibleVersionId",
                table: "DisunityVersionCompatibilities",
                column: "MinCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DisunityVersionCompatibilities_VersionId",
                table: "DisunityVersionCompatibilities",
                column: "VersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisunityVersions_VersionNumberId",
                table: "DisunityVersions",
                column: "VersionNumberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModDependencies_DependencyId",
                table: "ModDependencies",
                column: "DependencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDependencies_MaxVersionId",
                table: "ModDependencies",
                column: "MaxVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDependencies_MinVersionId",
                table: "ModDependencies",
                column: "MinVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDisunityCompatibilities_MaxCompatibleVersionId",
                table: "ModDisunityCompatibilities",
                column: "MaxCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDisunityCompatibilities_MinCompatibleVersionId",
                table: "ModDisunityCompatibilities",
                column: "MinCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDisunityCompatibilities_VersionId",
                table: "ModDisunityCompatibilities",
                column: "VersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mods_LatestId",
                table: "Mods",
                column: "LatestId");

            migrationBuilder.CreateIndex(
                name: "IX_Mods_TargetId",
                table: "Mods",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ModTargetCompatibilities_MaxCompatibleVersionId",
                table: "ModTargetCompatibilities",
                column: "MaxCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModTargetCompatibilities_MinCompatibleVersionId",
                table: "ModTargetCompatibilities",
                column: "MinCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModTargetCompatibilities_TargetId",
                table: "ModTargetCompatibilities",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ModVersionDownloadEvents_ModVersionId",
                table: "ModVersionDownloadEvents",
                column: "ModVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModVersions_VersionNumberId",
                table: "ModVersions",
                column: "VersionNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_ModVersions_ModId_VersionNumberId",
                table: "ModVersions",
                columns: new[] { "ModId", "VersionNumberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgMembers_OrgId_Role",
                table: "OrgMembers",
                columns: new[] { "OrgId", "Role" },
                unique: true,
                filter: "\"Role\" = 'owner'");

            migrationBuilder.CreateIndex(
                name: "IX_Orgs_Slug",
                table: "Orgs",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Targets_LatestId",
                table: "Targets",
                column: "LatestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Targets_Slug",
                table: "Targets",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TargetVersionCompatibilities_MaxCompatibleVersionId",
                table: "TargetVersionCompatibilities",
                column: "MaxCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetVersionCompatibilities_MinCompatibleVersionId",
                table: "TargetVersionCompatibilities",
                column: "MinCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetVersionCompatibilities_VersionId",
                table: "TargetVersionCompatibilities",
                column: "VersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnityVersions_VersionNumberId",
                table: "UnityVersions",
                column: "VersionNumberId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDisunityCompatibilities_ModVersions_VersionId",
                table: "ModDisunityCompatibilities",
                column: "VersionId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_Mods_DependencyId",
                table: "ModDependencies",
                column: "DependencyId",
                principalTable: "Mods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_ModVersions_DependentId",
                table: "ModDependencies",
                column: "DependentId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_ModVersions_MaxVersionId",
                table: "ModDependencies",
                column: "MaxVersionId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_ModVersions_MinVersionId",
                table: "ModDependencies",
                column: "MinVersionId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModVersions_Mods_ModId",
                table: "ModVersions",
                column: "ModId",
                principalTable: "Mods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mods_Targets_TargetId",
                table: "Mods",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModTargetCompatibilities_Targets_TargetId",
                table: "ModTargetCompatibilities",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModTargetCompatibilities_TargetVersions_MaxCompatibleVersio~",
                table: "ModTargetCompatibilities",
                column: "MaxCompatibleVersionId",
                principalTable: "TargetVersions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModTargetCompatibilities_TargetVersions_MinCompatibleVersio~",
                table: "ModTargetCompatibilities",
                column: "MinCompatibleVersionId",
                principalTable: "TargetVersions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetVersions_Targets_TargetId",
                table: "TargetVersions",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mods_Orgs_OwnerId",
                table: "Mods");

            migrationBuilder.DropForeignKey(
                name: "FK_ModVersions_VersionNumbers_VersionNumberId",
                table: "ModVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_ModVersions_Mods_ModId",
                table: "ModVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_TargetVersions_Targets_TargetId",
                table: "TargetVersions");

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
                name: "DisunityVersionCompatibilities");

            migrationBuilder.DropTable(
                name: "ModDependencies");

            migrationBuilder.DropTable(
                name: "ModDisunityCompatibilities");

            migrationBuilder.DropTable(
                name: "ModTargetCompatibilities");

            migrationBuilder.DropTable(
                name: "ModVersionDownloadEvents");

            migrationBuilder.DropTable(
                name: "OrgMembers");

            migrationBuilder.DropTable(
                name: "StoredFiles");

            migrationBuilder.DropTable(
                name: "TargetVersionCompatibilities");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DisunityVersions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UnityVersions");

            migrationBuilder.DropTable(
                name: "Orgs");

            migrationBuilder.DropTable(
                name: "VersionNumbers");

            migrationBuilder.DropTable(
                name: "Mods");

            migrationBuilder.DropTable(
                name: "ModVersions");

            migrationBuilder.DropTable(
                name: "Targets");

            migrationBuilder.DropTable(
                name: "TargetVersions");
        }
    }
}
