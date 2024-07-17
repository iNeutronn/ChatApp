using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBAL.Exeptions
{
    public class UserNotFoundExeption : Exception
    {
        public UserNotFoundExeption(string message) : base(message)
        {
        }
    }
}
