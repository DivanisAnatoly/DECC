using DECC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DECC.Services
{
    internal class IdentityService
    {
        internal static UserRole GetUserRole()
        {
            return (UserRole)Properties.Settings.Default.Role;
        }
        
        internal static bool IsTeacher()
        {
            return (UserRole)Properties.Settings.Default.Role == UserRole.Teacher;
        }

        internal static bool IsStudent()
        {
            return (UserRole)Properties.Settings.Default.Role == UserRole.Student;
        }

        internal static bool IsAdmin()
        {
            return (UserRole)Properties.Settings.Default.Role == UserRole.Admin;
        }
    }
}
