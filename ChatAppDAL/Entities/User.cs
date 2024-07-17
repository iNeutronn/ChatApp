using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDAL.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = null!;
        public ICollection<Chat> CreatedChats { get; set; } = null!;
        public ICollection<Chat> JoinedChats { get; set; } = null!;
    }
}
