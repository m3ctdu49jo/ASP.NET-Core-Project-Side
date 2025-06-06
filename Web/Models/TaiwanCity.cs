namespace ShoppingMall.Web.Models;

public class TaiwanCity
{
    public string CityName { get; set; } = string.Empty;
    public List<AreaList>? CountryList { get; set; } = null;
}


public class AreaList
{
    public string ZipCode { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public string AreaEngName { get; set; } = string.Empty;
}