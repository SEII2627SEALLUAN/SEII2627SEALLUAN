using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ItemDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII.UT.ItemsController_test
{
    public class GetItemsForPurchase_test : AppForSEII4SqliteUT
    {
        public GetItemsForPurchase_test()
        {
            //creating data for the test
            var swimming = new Sport("Synchronized swimming", 2, 3, "Swim beautifully and in sync", "Must wear a standardized swimsuit");
            var boxing = new Sport("Boxing", 2, 1, "Knock your opponent down", "Dirty play, overheads or kicks are not allowed, must wear gloves");
            var onePiece = new Item("One-piece swimsuit", 30, 16, Gender.Female, "L", 75);
            onePiece.Sport = swimming; //<- might be worth a constructor!! to ask and/or consider
            var glove = new Item("Boxing glove", 25, 18, Gender.Other, "M", 100);
            glove.Sport = boxing;
            var swimmingCap  = new Item("Swim cap", 5, 8, Gender.Female, "S", 0); //no swimming caps left in the store
            swimmingCap.Sport = swimming;
            //to context
            _context.AddRange(swimming, boxing);
            _context.AddRange(onePiece, glove, swimmingCap);
            _context.SaveChanges();
        }
    }
}