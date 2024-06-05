using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public enum Roles
    {
        [Description("None")]
        None,
        [Description("Manager")]
        Manager,
        [Description("MainManager")]
        MainManager,
        [Description("Admin")]
        Admin
    }

    public static class RolesEnumExtensions
    {
        public static string RoleToString(this Roles role)
        {
            switch (role) 
            { 
                case Roles.None:
                    return "None";
                case Roles.Manager:
                    return "Manager";
                case Roles.MainManager:
                    return "MainManager";
                case Roles.Admin:
                    return "Admin";
                default:
                    return "None";
            }
        }
    }
}
