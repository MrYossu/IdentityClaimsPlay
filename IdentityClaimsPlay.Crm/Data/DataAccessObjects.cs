namespace IdentityClaimsPlay.Crm.Data;

public record NameValuePair(string Name, string Value);

public record ClaimDto(string Type, string Value);

public class PermissionDto {
  public PermissionDto(string value, bool hasPermission) {
    Value = value;
    HasPermission = hasPermission;
  }

  public string Value { get; set; }
  public bool HasPermission { get; set; }
}