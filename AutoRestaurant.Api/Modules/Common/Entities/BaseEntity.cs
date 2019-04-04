using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRestaurant.Api.Modules.Common.Entities {
    public abstract class BaseEntity {
        public DateTime CreatedDate { get; } = DateTime.UtcNow;
    }
}
