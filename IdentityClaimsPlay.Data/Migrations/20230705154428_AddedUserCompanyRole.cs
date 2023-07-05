#nullable disable

namespace IdentityClaimsPlay.Data.Migrations {
  /// <inheritdoc />
  public partial class AddedUserCompanyRole : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
        name: "UserCompanyRole",
        columns: table => new {
          Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
          UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
          CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
          Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
        },
        constraints: table => {
          table.PrimaryKey("PK_UserCompanyRole", x => x.Id);
          table.ForeignKey(
            name: "FK_UserCompanyRole_AspNetUsers_UserId",
            column: x => x.UserId,
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            name: "FK_UserCompanyRole_Companies_CompanyId",
            column: x => x.CompanyId,
            principalTable: "Companies",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        });

      migrationBuilder.CreateIndex(
        name: "IX_UserCompanyRole_CompanyId",
        table: "UserCompanyRole",
        column: "CompanyId");

      migrationBuilder.CreateIndex(
        name: "IX_UserCompanyRole_UserId",
        table: "UserCompanyRole",
        column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
      migrationBuilder.DropTable(
        name: "UserCompanyRole");
  }
}