using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Test.Controllers
{
    public partial class AdminController
    {
        public IActionResult Values()
        {
            return View();
        }
    }
}