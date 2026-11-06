using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.PurchaseDTOs;
using AppForSEII.API.DTOS.PurchaseDTOS;

namespace AppForSEII.UT.PurchasesController_test
{
    public class CreatePurchase_test : AppForSEII4SqliteUT
    {
        private const string _userName = "elena@uclm.es"; //we make them here to reuse a lot
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
                new object[] { PurchaseNoItem, "Error! You must include at least one item to be purchased" }, 
                new object[] { PurchaseUnknownUser,"Error! UserName is not registered" }, //without .
                new object[] { NonexistenceOfItem,"Error! The item isn't available." },
                new object[] { OutOfStockItem, "Error! Only" }, //change error message for these tests or not? Easier
                new object[] { NotEnoughStockItem, "Error! Only" }, 
            };

            return allTests;
        }

        [Theory] //General for errors, uses the IEnumerable<object[]> TestCasesFor_Createpurchase with the cases
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreatePurchase_Error_test(PurchaseForCreateDTO purchaseDTO, string errorExpected)
        {
            //ARRANGE
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);
            //ACT
            var result = await controller.CreatePurchase(purchaseDTO);
            //ASSERT
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
        public async Task CreatePurchase_Success_test()
        {
            //ARRANGE (preparation)
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            var purchaseDTO = new PurchaseForCreateDTO( //we will be testing this input
                _street, _city, _province, _creditCardNumber, _ccv, _expirationDate,
                _userName, _customerNameSurname,new List<PurchaseItemForCreateDTO>() {
                    new PurchaseItemForCreateDTO(_item1Name, 2)
                },
                "Birthday present for my son", false);
            //we expect this
            var expectedPurchaseDetailDTO = new PurchaseDetailDTO("Elena","Navarro", DateTime.Now, "Birthday present for my son", false,
                50,                                  // 25 * 2
                new List<PurchaseItemDTO>() {
                    new PurchaseItemDTO(_item1Name, 18, Gender.Other, 25, "M", 2)
                });

            //ACT (really add it, returns PurchaseDetail)
            var result = await controller.CreatePurchase(purchaseDTO);

            //ASSERT (get the types, assign, and finally compare)
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualPurchaseDetailDTO = Assert.IsType<PurchaseDetailDTO>(createdResult.Value);

            Assert.True(expectedPurchaseDetailDTO.Equals(actualPurchaseDetailDTO));
        }
    }
}