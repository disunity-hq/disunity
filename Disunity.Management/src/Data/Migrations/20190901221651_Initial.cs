using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Disunity.Management.src.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisunityDistroIdentifier",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisunityDistroIdentifier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VersionSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    VersionSetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packages_VersionSet_VersionSetId",
                        column: x => x.VersionSetId,
                        principalTable: "VersionSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DisunityDistroId = table.Column<string>(nullable: true),
                    Mods = table.Column<string>(nullable: false),
                    PackageId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_DisunityDistroIdentifier_DisunityDistroId",
                        column: x => x.DisunityDistroId,
                        principalTable: "DisunityDistroIdentifier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VersionSetPackage",
                columns: table => new
                {
                    VersionSetId = table.Column<int>(nullable: false),
                    PackageId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionSetPackage", x => new { x.VersionSetId, x.PackageId });
                    table.ForeignKey(
                        name: "FK_VersionSetPackage_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersionSetPackage_VersionSet_VersionSetId",
                        column: x => x.VersionSetId,
                        principalTable: "VersionSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExecutablePath = table.Column<string>(nullable: true),
                    ManagedPath = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ActiveProfileId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Targets_Profiles_ActiveProfileId",
                        column: x => x.ActiveProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TargetProfiles",
                columns: table => new
                {
                    TargetMetaId = table.Column<int>(nullable: false),
                    ProfileMetaId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetProfiles", x => new { x.ProfileMetaId, x.TargetMetaId });
                    table.ForeignKey(
                        name: "FK_TargetProfiles_Profiles_ProfileMetaId",
                        column: x => x.ProfileMetaId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TargetProfiles_Targets_TargetMetaId",
                        column: x => x.TargetMetaId,
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_VersionSetId",
                table: "Packages",
                column: "VersionSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_DisunityDistroId",
                table: "Profiles",
                column: "DisunityDistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PackageId",
                table: "Profiles",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetProfiles_TargetMetaId",
                table: "TargetProfiles",
                column: "TargetMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Targets_ActiveProfileId",
                table: "Targets",
                column: "ActiveProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_VersionSetPackage_PackageId",
                table: "VersionSetPackage",
                column: "PackageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetProfiles");

            migrationBuilder.DropTable(
                name: "VersionSetPackage");

            migrationBuilder.DropTable(
                name: "Targets");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "DisunityDistroIdentifier");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "VersionSet");
        }
    }
}
