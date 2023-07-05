#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityClaimsPlay.Crm.Migrations {
  /// <inheritdoc />
  public partial class AddedCompany : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.AddColumn<string>(
        name: "CompanyId",
        table: "AspNetUsers",
        type: "nvarchar(450)",
        nullable: true);

      migrationBuilder.CreateTable(
        name: "Companies",
        columns: table => new {
          Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
          Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
        },
        constraints: table => {
          table.PrimaryKey("PK_Companies", x => x.Id);
        });

      migrationBuilder.CreateIndex(
        name: "IX_AspNetUsers_CompanyId",
        table: "AspNetUsers",
        column: "CompanyId");

      migrationBuilder.AddForeignKey(
        name: "FK_AspNetUsers_Companies_CompanyId",
        table: "AspNetUsers",
        column: "CompanyId",
        principalTable: "Companies",
        principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropForeignKey(
        name: "FK_AspNetUsers_Companies_CompanyId",
        table: "AspNetUsers");

      migrationBuilder.DropTable(
        name: "Companies");

      migrationBuilder.DropIndex(
        name: "IX_AspNetUsers_CompanyId",
        table: "AspNetUsers");

      migrationBuilder.DropColumn(
        name: "CompanyId",
        table: "AspNetUsers");
    }
  }
}