using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DECC.Windows.Teachers
{
    internal class TeachersOptions
    {
        internal static string[] SearchCategories
        {
            get
            {
                var result = new List<string>();
                foreach (var category in Enum.GetValues(typeof(TeacherSearchCategories)))
                    result.Add(((TeacherSearchCategories)category).EnumToString());
                return result.ToArray();
            }
        }

        internal static TeacherSearchCategories StringToEnum(string categoryName)
        {
            int index;
            TeacherSearchCategories result;

            try
            {
                index = Array.IndexOf(SearchCategories, categoryName);
                result = (TeacherSearchCategories)index;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }

    internal enum TeacherSearchCategories
    {
        Teacher,
        Phone,
        Position,
    }

    internal static class TeacherSearchCategoriesExtensions
    {
        internal static string EnumToString(this TeacherSearchCategories me)
        {
            return me switch
            {
                TeacherSearchCategories.Teacher => "Преподаватель",
                TeacherSearchCategories.Phone => "Телефон",
                TeacherSearchCategories.Position => "Должность",
                _ => string.Empty,
            };
        }
    }
}
