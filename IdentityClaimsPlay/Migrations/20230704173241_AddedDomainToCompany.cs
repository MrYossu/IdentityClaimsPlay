#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityClaimsPlay.Migrations {
  /// <inheritdoc />
  public partial class AddedDomainToCompany : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) =>
      migrationBuilder.AddColumn<string>(
        name: "Domain",
        table: "Companies",
        type: "nvarchar(max)",
        nullable: false,
        defaultValue: "");

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
      migrationBuilder.DropColumn(
        name: "Domain",
        table: "Companies");
  }
}