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
        private const string _creditCardNumber = "1234-6789-1234-6789";
        private const string _ccv = "123";
        private const string _expirationDate = "10/30";
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

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase() { //inputs from user
            var PurchaseNoItem = new PurchaseForCreateDTO(_street, _city, _province, _creditCardNumber, _ccv, 
                                _expirationDate, _userName, _customerNameSurname, new List<PurchaseItemForCreateDTO>(), //empty list
                                "Birthday present for my son", false);

            var PurchaseUnknownUser = new PurchaseForCreateDTO(_street, _city, _province, _creditCardNumber, _ccv, 
                                    _expirationDate, "mike.tyson@uclm.es", "Mike Tyson", //not valid
                                    new List<PurchaseItemForCreateDTO>(){new PurchaseItemForCreateDTO(_item1Name, 1)},
                                    "Birthday present for my son", false);

            var NonexistenceOfItem = new PurchaseForCreateDTO(_street, _city, _province, _creditCardNumber, _ccv, _expirationDate,
                                    _userName, _customerNameSurname,
                                    new List<PurchaseItemForCreateDTO>() {new PurchaseItemForCreateDTO("Jedi lightsaber", 1)}, //not valid
                                    "Birthday present", false);

            var OutOfStockItem = new PurchaseForCreateDTO(_street, _city, _province, _creditCardNumber, _ccv, _expirationDate,
                                    _userName, _customerNameSurname,
                                    new List<PurchaseItemForCreateDTO>() {new PurchaseItemForCreateDTO(_item2Name, 1)}, //Trunks is included, but there's 0 in stock. Problem?
                                    "Birthday present", false);

            var NotEnoughStockItem = new PurchaseForCreateDTO(_street, _city, _province, _creditCardNumber, _ccv, _expirationDate,
                                    _userName, _customerNameSurname,
                                    new List<PurchaseItemForCreateDTO>() {new PurchaseItemForCreateDTO(_item1Name, 99999)}, //not that many
                                    "Birthday present", false);

            var allTests = new List<object[]>
            {                       //input and error
                new object[] { PurchaseNoItem, "Error! The item isn't available." }, 
                new object[] { PurchaseUnknownUser,"Error! UserName is not registered" }, //without .
                new object[] { NonexistenceOfItem,"Error! The item isn't available." },
                new object[] { OutOfStockItem, "Error! Only" }, //change error message for these tests or not? Easier
                new object[] { NotEnoughStockItem, "Error! Only" }, 
            };

            return allTests;
        }

        [Theory] //STILL TO CHECK, ONLY REPLACED
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