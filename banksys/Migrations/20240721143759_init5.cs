using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace banksys.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Payees",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payees_UserId",
                table: "Payees",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payees_Users_UserId",
                table: "Payees",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payees_Users_UserId",
                table: "Payees");

            migrationBuilder.DropIndex(
                name: "IX_Payees_UserId",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payees");
        }
    }
}
