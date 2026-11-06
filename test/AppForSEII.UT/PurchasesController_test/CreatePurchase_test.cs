using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.PurchaseDTOs;
using AppForSEII.API.DTOS.PurchaseDTOS;

namespace AppForSEII.UT.PurchasesController_test
{
    public class CreatePurchase_test : AppForSEII4SqliteUT
    {
        private const string _userName = "elena.navarro@uclm.es"; //we make them here to reause a lot
        private const string _customerNameSurname = "Elena Navarro";
        private const string _street = "Calle Juan Sebastián Elcano";
        private const string _city = "Albacete";
        private const string _province = "Albacete";
        private const string _ccNumber = "1234-6789-1234-6789";
        private const string _ccv = "123";
        private const string _expDate = "10/30";
        private const string _item1Name = "Boxing glove";
        private const string _item2Name = "Trunks";

        public CreatePurchase_test()
        {
            var boxing = new Sport("Boxing", 2, 1, "Knock your opponent down", "Dirty play, overheads or kicks are not allowed, must wear gloves");

            var items = new List<Item>(){ //treat as list perhaps better
                new Item(_item1Name, 25, 18, Gender.Other, "M", 100),  //enough stock
                new Item(_item2Name, 20, 18, Gender.Male,  "M",  0),   //no stock
            };
            items[0].Sport = boxing; //assign each, both to boxing
            items[1].Sport = boxing;

            ApplicationUser user = new ApplicationUser("0", "Elena", "Navarro", new DateOnly(2026, 11, 10), 30, Gender.Female, "elena@uclm.es");               
            //add to context
            _context.ApplicationUsers.Add(user);
            _context.Add(boxing);
            _context.AddRange(items);
            _context.SaveChanges();
        }
        public static IEnumerable<object[]> TestCasesFor_CreatePurchase() {
            var PurchaseNoITem = new PurchaseForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<PurchaseItemDTO>());

            var PurchaseItems = new List<PurchaseItemDTO>() { new PurchaseItemDTO(2, _movie2Title, _movie2Genre, 4.0, "My favourite movie") };

            var PurchaseFromBeforeToday = new PurchaseForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today, DateTime.Today.AddDays(5), PurchaseItems);

            var PurchaseToBeforeFrom = new PurchaseForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(5), DateTime.Today.AddDays(2), PurchaseItems);

            var PurchaseApplicationUser = new PurchaseForCreateDTO("victor.lopez@uclm.es", _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(4), PurchaseItems);

            var PurchaseMovieNotAvailable = new PurchaseForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                new List<PurchaseItemDTO>() { new PurchaseItemDTO(1, _movie1Title, _movie1Genre, 1.0) });


            var allTests = new List<object[]>
            {             //input for createpurchase - Error expected
                new object[] { PurchaseNoITem, "Error! You must include at least one movie to be rented",  },
                new object[] { PurchaseFromBeforeToday, "Error! Your Purchase date must start later than today", },
                new object[] { PurchaseToBeforeFrom, "Error! Your Purchase must end later than it starts", },
                new object[] { PurchaseApplicationUser, "Error! UserName is not registered", },
                new object[] { PurchaseMovieNotAvailable, "Error! Movie titled 'The lord of the rings' is not available for being rented from", },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreatePurchase_Error_test(PurchaseForCreateDTO PurchaseDTO, string errorExpected) {
            // Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;

            var controller = new PurchasesController(_context, logger);

            // Act
            var result = await controller.CreatePurchase(PurchaseDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreatePurchase_Success_test() {
            //ARRANGE
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            var PurchaseDTO = new PurchaseForCreateDTO(_userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                to, from, new List<PurchaseItemDTO>()
                { new PurchaseItemDTO(2, _movie1Title, _movie1Genre, 1.0) });

            var expectedPurchaseDetailDTO = new PurchaseDetailDTO(2, DateTime.Now,
                _userName, _customerNameSurname,
                _deliveryAddress, PaymentMethodTypes.CreditCard,
                to, from, new List<PurchaseItemDTO>()
                { new PurchaseItemDTO(2, _movie1Title, _movie1Genre, 1.0) });

            //ACT
            var result = await controller.CreatePurchase(PurchaseDTO);

            //ASSERT
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualPurchaseDetailDTO = Assert.IsType<PurchaseDetailDTO>(createdResult.Value);

            Assert.Equal(expectedPurchaseDetailDTO, actualPurchaseDetailDTO);

        }
    }
}