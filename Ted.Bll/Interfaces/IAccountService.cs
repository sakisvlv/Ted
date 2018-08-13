using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Ted.Model.Auth;

namespace Ted.Bll.Interfaces
{
    public interface IAccountService
    {
        string GetToken(User user);
        JsonResult Error(string message);
    }
}
