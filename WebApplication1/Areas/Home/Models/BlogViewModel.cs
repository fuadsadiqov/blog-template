using GP.Domain.Entities.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Models;

public class BlogViewModel
{
    public List<Category> Categories { get; set; }
    public List<SelectListItem> Tags { get; set; }
    public Blog? Blog { get; set; }
}