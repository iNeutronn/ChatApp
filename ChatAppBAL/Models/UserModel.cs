using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBAL.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? UserName { get; set;}

        public IEnumerable<int>? JoinedChatsIds { get; set; }
    }
}
