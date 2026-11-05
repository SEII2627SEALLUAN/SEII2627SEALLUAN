using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.PurchaseDTOs;
using AppForSEII.API.DTOS.PurchaseDTOS; //leave. It's strange but no errors
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII.UT.PurchasesController_test
{
    public class GetPurchase_test : AppForSEII4SqliteUT
    {
        public GetPurchase_test()
        {
            //creating data for the test
            var firstUser = new ApplicationUser("0", "Elena", "Navarro", new DateOnly(2026, 11, 10), 30, Gender.Female, "elena@uclm.es");
            var boxing = new Sport("Boxing", 2, 1, "Knock your opponent down", "Dirty play, overheads or kicks are not allowed, must wear gloves");
            var gloves = new Item("Boxing glove", 25, 18, Gender.Other, "M", 100);
            gloves.Sport = boxing;
            var trunks = new Item("Trunks", 20, 18, Gender.Male, "M", 50);
            trunks.Sport = boxing;

            var purchase = new Purchase(
                new DateTime(2026, 11, 15),
                "Birthday present for my son",
                70,
                "Calle Juan Sebastián Elcano",
                "Albacete",
                "Albacete",
                "1234-6789-1234-6789",
                "123",
                "10/30",
                true,                       
                PurchaseStatus.Delivered,
                firstUser.Id                
            );
            purchase.User = firstUser; //not in constructor per se, only Id

            //these for navigation
            var purchaseItems = new List<PurchaseItem>()
            {
                new PurchaseItem(gloves.Id, purchase.Id, 2, 25),
                new PurchaseItem(trunks.Id, purchase.Id, 1, 20)
            };
            purchaseItems[0].Item = gloves;
            purchaseItems[1].Item = trunks;
            //addinvg to my purchase
            foreach (var pi in purchaseItems)
            {
                purchase.PurchaseItems.Add(pi);
            }
            //to context
            _context.Add(firstUser);
            _context.Add(boxing);
            _context.Add(gloves);
            _context.Add(trunks);
            _context.Add(purchase);
            _context.AddRange(purchaseItems); //range instead of all
            _context.SaveChanges();
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetCorrectPurchase_test()
        {
            //ARRANGE
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            PurchasesController controller = new PurchasesController(_context, logger);
            var expectedPurchase = new PurchaseDetailDTO( //create expected
                "Elena",
                "Navarro",
                new DateTime(2026, 11, 15),
                "Birthday present for my son",
                true,
                70,
                new List<PurchaseItemDTO>()
                {
                    new PurchaseItemDTO("Boxing glove",  18,  Gender.Other, 25, "M", 2),
                    new PurchaseItemDTO("Trunks", 18, Gender.Male, 20, "M", 1)
                });

            //ACT
            var result = await controller.GetPurchase(1); //disregard SeedData
            //ASSERT
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualPurchase = Assert.IsType<PurchaseDetailDTO>(okResult.Value);
            Assert.True(expectedPurchase.Equals(actualPurchase)); //needed mine
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task PurchaseDoesntExist_test()
        {
            //ARRANGE
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            PurchasesController controller = new PurchasesController(_context, logger);
            //ACT
            var result = await controller.GetPurchase(9999); //not going to be found
            //ASSERT
            Assert.IsType<NotFoundResult>(result);
        }
    }
}