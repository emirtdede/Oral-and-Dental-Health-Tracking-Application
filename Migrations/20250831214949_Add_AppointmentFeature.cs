using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisSagligiTakip.Migrations
{
    /// <inheritdoc />
    public partial class Add_AppointmentFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MuayeneRandevulari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HastaUserId = table.Column<int>(type: "int", nullable: false),
                    HekimUserId = table.Column<int>(type: "int", nullable: true),
                    OlusturanUserId = table.Column<int>(type: "int", nullable: false),
                    BaslangicZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Konum = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuayeneRandevulari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MuayeneRandevulari_Users_HastaUserId",
                        column: x => x.HastaUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MuayeneRandevulari_Users_HekimUserId",
                        column: x => x.HekimUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MuayeneRandevulari_Users_OlusturanUserId",
                        column: x => x.OlusturanUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MuayeneRandevulari_BaslangicZamani",
                table: "MuayeneRandevulari",
                column: "BaslangicZamani");

            migrationBuilder.CreateIndex(
                name: "IX_MuayeneRandevulari_HastaUserId_BaslangicZamani",
                table: "MuayeneRandevulari",
                columns: new[] { "HastaUserId", "BaslangicZamani" });

            migrationBuilder.CreateIndex(
                name: "IX_MuayeneRandevulari_HekimUserId_BaslangicZamani",
                table: "MuayeneRandevulari",
                columns: new[] { "HekimUserId", "BaslangicZamani" });

            migrationBuilder.CreateIndex(
                name: "IX_MuayeneRandevulari_OlusturanUserId",
                table: "MuayeneRandevulari",
                column: "OlusturanUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MuayeneRandevulari");
        }
    }
}
