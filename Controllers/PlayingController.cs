using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTic.Controllers
{
    public class PlayingController : Controller
    {
        
        public IActionResult Play(int id)
        {
            if(id == null)
            {
                ViewBag.id = 0;
            }else
            {
                ViewBag.id = id;
            }
            
            return View();
        }

        public IActionResult joinRoom(int id)
        {
            return RedirectToAction("Play", new { id });
        }
    }
}
