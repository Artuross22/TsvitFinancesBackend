using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TsvitFinances.Dto.Asset;
public class AssetPreCreationDataDto
{
    public required List<SelectListItem> Sectors { get; set; }
    public required List<SelectListItem> Markets { get; set; }
    public required List<SelectListItem> InvestmentTerms { get; set; }
}
