using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisSagligiTakip.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDisSagligiVerisiSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisSagligiVerileri_Users_UserId",
                table: "DisSagligiVerileri");

            migrationBuilder.DropColumn(
                name: "DisHekimi",
                table: "DisSagligiVerileri");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "DisSagligiVerileri",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DisHekimiAdi",
                table: "DisSagligiVerileri",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GorselYolu",
                table: "DisSagligiVerileri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HastaUserId",
                table: "DisSagligiVerileri",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisSagligiVerileri_HastaUserId",
                table: "DisSagligiVerileri",
                column: "HastaUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DisSagligiVerileri_Users_HastaUserId",
                table: "DisSagligiVerileri",
                column: "HastaUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DisSagligiVerileri_Users_UserId",
                table: "DisSagligiVerileri",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisSagligiVerileri_Users_HastaUserId",
                table: "DisSagligiVerileri");

            migrationBuilder.DropForeignKey(
                name: "FK_DisSagligiVerileri_Users_UserId",
                table: "DisSagligiVerileri");

            migrationBuilder.DropIndex(
                name: "IX_DisSagligiVerileri_HastaUserId",
                table: "DisSagligiVerileri");

            migrationBuilder.DropColumn(
                name: "DisHekimiAdi",
                table: "DisSagligiVerileri");

            migrationBuilder.DropColumn(
                name: "GorselYolu",
                table: "DisSagligiVerileri");

            migrationBuilder.DropColumn(
                name: "HastaUserId",
                table: "DisSagligiVerileri");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "DisSagligiVerileri",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<string>(
                name: "DisHekimi",
                table: "DisSagligiVerileri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_DisSagligiVerileri_Users_UserId",
                table: "DisSagligiVerileri",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
