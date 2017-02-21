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
        public IActionResult Index()
        {
            return View();
        }

        // Controller for first game
        [HttpGet]
        public IActionResult Game()
        {
            //instantiate new game and deal with player starting on 500 and phil on 5000            
            DealCards dc = new DealCards();
            dc.Deal(500, 5000);


            // - For debugging specific hands -
            //int turns = 0;
            //while(Convert.ToString(dc.winningPlayerHand) != "StraightFlush")
            //{
            //    dc.Deal(500, 5000);
            //    turns++;
            //}
            //Console.WriteLine(turns);


            /*create new model and map the values from the Dealcards object to game.
              Considered using AutoMapper but didn't think it was worth it since this is
              the only model being mapped.*/
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

        //Controller for playing new game, passing current wallets of each player
        [HttpPost]
        public IActionResult Game(string i, string j)
        {
            //logic for checking each wallet (if new game or continue)
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

            //again mapping object to model to pass to view
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