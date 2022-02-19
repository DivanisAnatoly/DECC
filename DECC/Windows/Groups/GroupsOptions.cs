using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DECC.Windows.Groups
{

    internal class GroupsOptions
    {
        internal static string[] SearchCategories
        {
            get
            {
                var result = new List<string>();
                foreach (var category in Enum.GetValues(typeof(GroupSearchCategories)))
                    result.Add(((GroupSearchCategories)category).EnumToString());
                return result.ToArray();
            }
        }

        internal static GroupSearchCategories StringToEnum(string categoryName)
        {
            int index;
            GroupSearchCategories result;

            try
            {
                index = Array.IndexOf(SearchCategories, categoryName);
                result = (GroupSearchCategories)index;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }

    internal enum GroupSearchCategories
    {
        Group,
        Curator,
        Profile,
    }

    internal static class GroupSearchCategoriesExtensions
    {
        internal static string EnumToString(this GroupSearchCategories me)
        {
            return me switch
            {
                GroupSearchCategories.Group => "Группа",
                GroupSearchCategories.Curator => "Куратор",
                GroupSearchCategories.Profile => "Профиль",
                _ => string.Empty,
            };
        }
    }
}
