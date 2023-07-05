#nullable disable

namespace IdentityClaimsPlay.Data.Migrations {
  /// <inheritdoc />
  public partial class ChangedCompanyPropertyOfUserCompanyRoleToBeNullable : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropForeignKey(
        name: "FK_AspNetUsers_Companies_CompanyId",
        table: "AspNetUsers");

      migrationBuilder.DropForeignKey(
        name: "FK_UserCompanyRole_Companies_CompanyId",
        table: "UserCompanyRole");

      migrationBuilder.DropIndex(
        name: "IX_AspNetUsers_CompanyId",
        table: "AspNetUsers");

      migrationBuilder.DropColumn(
        name: "CompanyId",
        table: "AspNetUsers");

      migrationBuilder.AlterColumn<string>(
        name: "CompanyId",
        table: "UserCompanyRole",
        type: "nvarchar(450)",
        nullable: true,
        oldClrType: typeof(string),
        oldType: "nvarchar(450)");

      migrationBuilder.AddForeignKey(
        name: "FK_UserCompanyRole_Companies_CompanyId",
        table: "UserCompanyRole",
        column: "CompanyId",
        principalTable: "Companies",
        principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropForeignKey(
        name: "FK_UserCompanyRole_Companies_CompanyId",
        table: "UserCompanyRole");

      migrationBuilder.AlterColumn<string>(
        name: "CompanyId",
        table: "UserCompanyRole",
        type: "nvarchar(450)",
        nullable: false,
        defaultValue: "",
        oldClrType: typeof(string),
        oldType: "nvarchar(450)",
        oldNullable: true);

      migrationBuilder.AddColumn<string>(
        name: "CompanyId",
        table: "AspNetUsers",
        type: "nvarchar(450)",
        nullable: true);

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

      migrationBuilder.AddForeignKey(
        name: "FK_UserCompanyRole_Companies_CompanyId",
        table: "UserCompanyRole",
        column: "CompanyId",
        principalTable: "Companies",
        principalColumn: "Id",
        onDelete: ReferentialAction.Cascade);
    }
  }
}