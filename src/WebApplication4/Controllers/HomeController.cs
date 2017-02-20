using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Poker;
using Poker.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication4.Controllers
{
    

    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Game()
        {
            DealCards dc = new DealCards();
            dc.Deal(500, 5000);


            Game data = new Game();
            data.result = dc.result;
            data.playerHand = dc.RenameHands(dc.FirstPlayerHand);
            data.cpuHand = dc.RenameHands(dc.FirstComputerHand);
            data.flop = dc.RenameHands(dc.FlopHand);
            data.playerWallet = dc.playerWallet;
            data.cpuWallet = dc.cpuWallet;
            data.playerResult = Convert.ToString(dc.winningPlayerHand);
            data.cpuResult = Convert.ToString(dc.winningCpuHand);

            return View(data);
        }

        public IActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Game(string i, string j)
        {
            double player = Convert.ToDouble(i);
            double cpu = Convert.ToDouble(j);
            DealCards dc = new DealCards();
            if (player == 0)
            {
                dc.Deal(500, cpu);
            }
            else if (cpu <= 0)
            {
                dc.Deal(player, 10000);
            }
            else
                dc.Deal(player, cpu);


            Game data = new Game();
            data.result = dc.result;
            data.playerHand = dc.RenameHands(dc.FirstPlayerHand);
            data.cpuHand = dc.RenameHands(dc.FirstComputerHand);
            data.flop = dc.RenameHands(dc.FlopHand);
            data.playerWallet = dc.playerWallet;
            data.cpuWallet = dc.cpuWallet;
            data.playerResult = Convert.ToString(dc.winningPlayerHand);
            data.cpuResult = Convert.ToString(dc.winningCpuHand);

            return View(data);
        }
        
    }
}