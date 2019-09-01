using System;

using Microsoft.EntityFrameworkCore.Migrations;


namespace Disunity.Management.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackageIdentifier",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageIdentifier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DisunityDistroId = table.Column<string>(nullable: true),
                    Mods = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_PackageIdentifier_DisunityDistroId",
                        column: x => x.DisunityDistroId,
                        principalTable: "PackageIdentifier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Profiles_DisunityDistroId",
                table: "Profiles",
                column: "DisunityDistroId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetProfiles_TargetMetaId",
                table: "TargetProfiles",
                column: "TargetMetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Targets_ActiveProfileId",
                table: "Targets",
                column: "ActiveProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetProfiles");

            migrationBuilder.DropTable(
                name: "Targets");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "PackageIdentifier");
        }
    }
}
