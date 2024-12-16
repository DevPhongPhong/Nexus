using Nexus.Models.Enums;

namespace EIM.Attributes.FilterPipelines.Authorizations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class EmployeeTypeAccessAttribute : Attribute
{
    public EmployeeType[] AllowedEmployeeTypes { get; }

    public EmployeeTypeAccessAttribute(params EmployeeType[] allowedEmployeeTypes)
    {
        AllowedEmployeeTypes = allowedEmployeeTypes;
    }
}
