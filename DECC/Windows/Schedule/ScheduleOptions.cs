using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DECC.Windows.Schedule
{
    internal class ScheduleOptions
    {
        internal static string[] SearchCategories
        {
            get
            {
                var result = new List<string>();
                foreach (var category in Enum.GetValues(typeof(SheduleSearchCategories)))
                    result.Add(((SheduleSearchCategories)category).EnumToString());
                return result.ToArray();
            }
        }
        
        internal static SheduleSearchCategories StringToEnum(string categoryName)
        {
            int index;
            SheduleSearchCategories result;

            try {
                index = Array.IndexOf(SearchCategories, categoryName);
                result = (SheduleSearchCategories)index;
            } 
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }

    internal enum SheduleSearchCategories
    {
        Teacher,
        Group,
        Discipline
    }

    internal static class SheduleSearchCategoriesExtensions
    {
        internal static string EnumToString(this SheduleSearchCategories me)
        {
            return me switch
            {
                SheduleSearchCategories.Teacher => "Преподаватель",
                SheduleSearchCategories.Group => "Группа",
                SheduleSearchCategories.Discipline => "Дисциплина",
                _ => string.Empty,
            };
        }
    }
}
