using System;
using System.Collections.Generic;

namespace Yogeshwar.Web
{
    public partial class Notification
    {
        public int Id { get; set; }
        public int ProductAccessoriesId { get; set; }
        public int OrderId { get; set; }
        public byte Status { get; set; }
        public DateTime Date { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual ProductAccessory ProductAccessories { get; set; } = null!;
    }
}
