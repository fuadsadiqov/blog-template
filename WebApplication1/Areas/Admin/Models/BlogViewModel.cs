using GP.Domain.Entities.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GP.MVC.Areas.Home.Models;

public class BlogViewModel
{
    public List<Category> Categories { get; set; }
    public List<SelectListItem> Tags { get; set; }
    public Blog? Blog { get; set; }
}