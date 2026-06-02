using System.Collections.Generic;

namespace WebApp.Models
{
    // CatalogViewModel binds catalog items collection and active filters for display on the main page.
    public class CatalogViewModel
    {
        // The list of catalog items fetched matching current filters.
        public List<ItemDto> Items { get; set; } = [];

        // The complete distinct list of categories used to populate the catalog filter dropdown.
        public List<string> Categories { get; set; } = [];

        // Filter: name search query.
        public string? Name { get; set; }

        // Filter: selected category value.
        public string? Category { get; set; }

        // Filter: sort direction (asc, desc, or default).
        public string? SortByPrice { get; set; }
    }
}
