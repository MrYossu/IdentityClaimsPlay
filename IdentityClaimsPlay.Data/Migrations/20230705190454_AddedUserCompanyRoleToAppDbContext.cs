#nullable disable

namespace IdentityClaimsPlay.Data.Migrations {
  /// <inheritdoc />
  public partial class AddedUserCompanyRoleToAppDbContext : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropForeignKey(
        name: "FK_UserCompanyRole_AspNetUsers_UserId",
        table: "UserCompanyRole");

      migrationBuilder.DropForeignKey(
        name: "FK_UserCompanyRole_Companies_CompanyId",
        table: "UserCompanyRole");

      migrationBuilder.DropPrimaryKey(
        name: "PK_UserCompanyRole",
        table: "UserCompanyRole");

      migrationBuilder.RenameTable(
        name: "UserCompanyRole",
        newName: "UserCompanyRoles");

      migrationBuilder.RenameIndex(
        name: "IX_UserCompanyRole_UserId",
        table: "UserCompanyRoles",
        newName: "IX_UserCompanyRoles_UserId");

      migrationBuilder.RenameIndex(
        name: "IX_UserCompanyRole_CompanyId",
        table: "UserCompanyRoles",
        newName: "IX_UserCompanyRoles_CompanyId");

      migrationBuilder.AddPrimaryKey(
        name: "PK_UserCompanyRoles",
        table: "UserCompanyRoles",
        column: "Id");

      migrationBuilder.AddForeignKey(
        name: "FK_UserCompanyRoles_AspNetUsers_UserId",
        table: "UserCompanyRoles",
        column: "UserId",
        principalTable: "AspNetUsers",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
        name: "FK_UserCompanyRoles_Companies_CompanyId",
        table: "UserCompanyRoles",
        column: "CompanyId",
        principalTable: "Companies",
        principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropForeignKey(
        name: "FK_UserCompanyRoles_AspNetUsers_UserId",
        table: "UserCompanyRoles");

      migrationBuilder.DropForeignKey(
        name: "FK_UserCompanyRoles_Companies_CompanyId",
        table: "UserCompanyRoles");

      migrationBuilder.DropPrimaryKey(
        name: "PK_UserCompanyRoles",
        table: "UserCompanyRoles");

      migrationBuilder.RenameTable(
        name: "UserCompanyRoles",
        newName: "UserCompanyRole");

      migrationBuilder.RenameIndex(
        name: "IX_UserCompanyRoles_UserId",
        table: "UserCompanyRole",
        newName: "IX_UserCompanyRole_UserId");

      migrationBuilder.RenameIndex(
        name: "IX_UserCompanyRoles_CompanyId",
        table: "UserCompanyRole",
        newName: "IX_UserCompanyRole_CompanyId");

      migrationBuilder.AddPrimaryKey(
        name: "PK_UserCompanyRole",
        table: "UserCompanyRole",
        column: "Id");

      migrationBuilder.AddForeignKey(
        name: "FK_UserCompanyRole_AspNetUsers_UserId",
        table: "UserCompanyRole",
        column: "UserId",
        principalTable: "AspNetUsers",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);

      migrationBuilder.AddForeignKey(
        name: "FK_UserCompanyRole_Companies_CompanyId",
        table: "UserCompanyRole",
        column: "CompanyId",
        principalTable: "Companies",
        principalColumn: "Id");
    }
  }
}