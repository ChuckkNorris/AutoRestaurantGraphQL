using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoRestaurant.Api.Modules.Common.Entities {
    public class Menu : BaseEntity {
        [Key]
        public int MenuId { get; set; }
        public string Name { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
