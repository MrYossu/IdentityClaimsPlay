#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityClaimsPlay.Crm.Migrations {
  /// <inheritdoc />
  public partial class AddedCharity : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
        name: "Charities",
        columns: table => new {
          Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
          Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
          Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
          Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
          CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
        },
        constraints: table => {
          table.PrimaryKey("PK_Charities", x => x.Id);
          table.ForeignKey(
            name: "FK_Charities_Companies_CompanyId",
            column: x => x.CompanyId,
            principalTable: "Companies",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        });

      migrationBuilder.CreateIndex(
        name: "IX_Charities_CompanyId",
        table: "Charities",
        column: "CompanyId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
      migrationBuilder.DropTable(
        name: "Charities");
  }
}