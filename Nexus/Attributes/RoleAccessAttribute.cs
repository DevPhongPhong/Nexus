using Nexus.Models.Enums;

namespace EIM.Attributes.FilterPipelines.Authorizations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class RoleAccessAttribute : Attribute
{
    public Role[] AllowedEmployeeTypes { get; }
    public bool IsAll { get; } = false;

    public RoleAccessAttribute(params Role[] allowedEmployeeTypes)
    {
        AllowedEmployeeTypes = allowedEmployeeTypes;
        if (allowedEmployeeTypes == null || allowedEmployeeTypes.Length == 0)
        {
            IsAll = true;
        }
    }
}
