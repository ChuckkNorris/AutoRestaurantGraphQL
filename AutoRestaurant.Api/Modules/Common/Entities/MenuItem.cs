using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRestaurant.Api.Modules.Common.Entities {
    public class MenuItem : BaseEntity {
        [Key]
        public int MenuItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? MenuId { get; set; }
        [ForeignKey("MenuId")]
        public Menu Menu { get; set; }
    }
}
