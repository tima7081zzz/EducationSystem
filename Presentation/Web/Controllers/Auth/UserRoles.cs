using DAL.Entities;

namespace Web.Controllers.Auth;

public static class UserRoles
{
    public static string ResolveByEnumValue(UserRoleType roleType)
    {
        return roleType.ToString();
    }
}