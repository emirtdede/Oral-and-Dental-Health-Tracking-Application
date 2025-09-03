using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisSagligiTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddAciklamaToFircalama : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuncellenmeTarihi",
                table: "FircalamaKayitlari");

            migrationBuilder.DropColumn(
                name: "Not",
                table: "FircalamaKayitlari");

            migrationBuilder.DropColumn(
                name: "OlusturmaTarihi",
                table: "FircalamaKayitlari");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "FircalamaKayitlari",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "FircalamaKayitlari");

            migrationBuilder.AddColumn<DateTime>(
                name: "GuncellenmeTarihi",
                table: "FircalamaKayitlari",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Not",
                table: "FircalamaKayitlari",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OlusturmaTarihi",
                table: "FircalamaKayitlari",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
