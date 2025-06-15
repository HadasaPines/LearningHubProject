using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class UserNameAlreadyExistsException : Exception
    {
        public UserNameAlreadyExistsException(string firstName, string lastName) : base($"The user with the name: {firstName}{lastName} does not exist")
        {

        }
        public int? StatusCode { get; } = 440;
    }
}

