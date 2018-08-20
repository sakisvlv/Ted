using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model.Auth;

namespace Ted.Bll.Interfaces
{
    public interface IAccountService
    {
        Task<string> GetToken(User user);
        JsonResult Error(string message);
    }
}
