#nullable disable

namespace IdentityClaimsPlay.Data.Migrations {
  /// <inheritdoc />
  public partial class AddedAddressToCompany : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) =>
      migrationBuilder.AddColumn<string>(
        name: "Address",
        table: "Companies",
        type: "nvarchar(max)",
        nullable: false,
        defaultValue: "");

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
      migrationBuilder.DropColumn(
        name: "Address",
        table: "Companies");
  }
}