using System.Collections.Generic;

namespace WebApp.Models
{
    public class CatalogViewModel
    {
        public List<ItemDto> Items { get; set; } = [];
        public List<string> Categories { get; set; } = [];
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? SortByPrice { get; set; }
    }
}
