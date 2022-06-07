using System.Collections.Generic;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog;

public class ProductOverview
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public decimal Price { get; set; }
    public IList<string> UrlsString { get; set; }
    
}