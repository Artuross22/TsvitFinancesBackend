using Microsoft.AspNetCore.Mvc.Rendering;

namespace TsvitFinances.Extensions;

public static class EnumHelper
{
    public static List<SelectListItem> GetSelectListFromEnum<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
                   .Cast<T>()
                   .Select(e => new SelectListItem
                   {
                       Value = ((int)(object)e).ToString(),
                       Text = e.ToString()
                   })
                   .ToList();
    }
}
