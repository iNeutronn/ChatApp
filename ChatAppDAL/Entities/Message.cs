using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDAL.Entities
{
    public class Message : BaseEntity
    {
        public int ChatId { get; set; }
        public Chat Chat { get; set; } = null!;
        public int AutorId { get; set; }
        public User Autor { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
