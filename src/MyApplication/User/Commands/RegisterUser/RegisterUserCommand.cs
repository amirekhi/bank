using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplication.User.Commands.RegisterUser
{
    public record RegisterUserCommand(string Name, string Email);
}