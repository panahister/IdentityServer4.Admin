using System;
using System.Collections.Generic;
using System.Text;

namespace Custom.EntityFramework.Entities
{
    public enum  Gender
    {
        Male=0,
        Female=1
    }
    public enum PermissionStatus : byte
    {
        None = 0,
        Allow = 1,
        Deny = 2,
    }
    public enum PermissionAttribute : byte
    {
        None = 0,
        Readonly = 1,
        Hide = 2,
    }
    public enum UIResourceType : byte
    {
        None = 0,
        Menu = 1,
        Form = 2,
        Control = 3,
        Field = 4
    }
    public enum ResourceType : byte
    {
        None = 0,
        UI = 1,
        Business = 2,
        Service = 3
    }
}
