﻿namespace IdentityClaimsPlay.Data.Data;

public class EntityBase {
  public string Id { get; set; } = Guid.NewGuid().ToString();
}