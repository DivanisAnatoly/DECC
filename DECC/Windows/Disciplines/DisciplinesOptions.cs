using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DECC.Windows.Disciplines
{

    internal class DisciplinesOptions
    {
        internal static string[] SearchCategories
        {
            get
            {
                var result = new List<string>();
                foreach (var category in Enum.GetValues(typeof(DisciplineSearchCategories)))
                    result.Add(((DisciplineSearchCategories)category).EnumToString());
                return result.ToArray();
            }
        }

        internal static DisciplineSearchCategories StringToEnum(string categoryName)
        {
            int index;
            DisciplineSearchCategories result;

            try
            {
                index = Array.IndexOf(SearchCategories, categoryName);
                result = (DisciplineSearchCategories)index;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }

    internal enum DisciplineSearchCategories
    {
        Discipline,
        LeadTeacher,
    }

    internal static class GroupSearchCategoriesExtensions
    {
        internal static string EnumToString(this DisciplineSearchCategories me)
        {
            return me switch
            {
                DisciplineSearchCategories.Discipline => "Дисциплина",
                DisciplineSearchCategories.LeadTeacher => "Ведущий преподаватель",
                _ => string.Empty,
            };
        }
    }
}
