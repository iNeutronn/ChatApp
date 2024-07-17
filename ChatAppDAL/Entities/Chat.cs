using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDAL.Entities
{
    public class Chat : BaseEntity
    {
        public string ChatName { get; set; } = null!;
        public int CreatorId { get; set; }
        [ForeignKey(nameof(CreatorId))]
        public User Creator { get; set; } = null!;
        public ICollection<Message> Messages { get; set; } = null!;
        public ICollection<User> Users { get; set; } = null!;
    }
}
