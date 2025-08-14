﻿namespace BaseProject.Application.DTOs.AuthIdentity.UsersIdentity;

public class RoleAssignRequestDto
{
    public string UserId { get; set; }

    public List<SelectItem> Roles { get; set; } = [];
    public List<SelectItem> Scopes { get; set; }
}

public class SelectItem
{
    public string Name { get; set; }

    public bool Selected { get; set; }
}
