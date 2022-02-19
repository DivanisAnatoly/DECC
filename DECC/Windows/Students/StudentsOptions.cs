using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DECC.Windows.Students
{
    internal class StudentsOptions
    {
        internal static string[] SearchCategories
        {
            get
            {
                var result = new List<string>();
                foreach (var category in Enum.GetValues(typeof(StudentSearchCategories)))
                    result.Add(((StudentSearchCategories)category).EnumToString());
                return result.ToArray();
            }
        }

        internal static StudentSearchCategories StringToEnum(string categoryName)
        {
            int index;
            StudentSearchCategories result;

            try
            {
                index = Array.IndexOf(SearchCategories, categoryName);
                result = (StudentSearchCategories)index;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }

    internal enum StudentSearchCategories
    {
        Student,
        Group,
        Profile,
    }

    internal static class TeacherSearchCategoriesExtensions
    {
        internal static string EnumToString(this StudentSearchCategories me)
        {
            return me switch
            {
                StudentSearchCategories.Student => "Обучающийся",
                StudentSearchCategories.Group => "Группа",
                StudentSearchCategories.Profile => "Профиль",
                _ => string.Empty,
            };
        }
    }
}
