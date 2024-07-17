using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBAL.Models
{
    public class MessageModel
    {
        public string? Content { get; set; }
        public int ChatId { get; set; }
        public int AutorId { get; set; }
    }
}
