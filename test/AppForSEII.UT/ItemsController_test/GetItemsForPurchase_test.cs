using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ItemDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII.UT.ItemsController_test;

public class GetItemsForPurchase_test : AppForSEII4SqliteUT
{
    public GetItemsForPurchase_test()
    {
        //first, the instances which will exist in the test
        var sports = new List<Sport>() {
            new Sport("Boxing", 2, 1, "Knock your opponent down", "Dirty play, overheads or kicks are not allowed, must wear gloves"),
            new Sport("Soccer", 2, 3, "Score more goals than the opposing team", "Official FIFA ones, the referee has most of the authority"),
        };
        var items = new List<Item>(){
            new Item("Boxing glove", 25, 18, Gender.Other, "M", 100),
            new Item("Trunks", 20, 18, Gender.Male,  "M", 50),
            //this item has no stock left so it shouldn't be returned when purchasing
            new Item("Headgear", 30, 18, Gender.Other, "L", 0),
        };
        //no constructor, once again, may fix later
        items[0].Sport = sports[0];
        items[1].Sport = sports[0];
        items[2].Sport = sports[0];
        //add to context
        _context.AddRange(sports);
        _context.AddRange(items);
        _context.SaveChanges();
    }

    [Fact]
    [Trait("LevelTesting", "Unit Testing")]
    public async Task GetItemsForPurchase_NULL_NAME_GENDER_SPORT() //NULL ALL
    {
        //ARRANGE
        var mock = new Mock<ILogger<ItemsController>>();
        ILogger<ItemsController> logger = mock.Object;
        var controller = new ItemsController(_context, logger);
        var expectedItems = new List<ItemDTO>() { //all of them are expected
            new ItemDTO("Boxing glove", 18, Gender.Other, 25, "M", 100, "Boxing"),
            new ItemDTO("Trunks", 18, Gender.Male,  20, "M",  50, "Boxing"),
        };
        //ACT
        var result = await controller.GetItemsForPurchase(null, null, null); //a search with no parameters, all catalogue
        //ASSERT
        var okResult = Assert.IsType<OkObjectResult>(result); //check response type
        var itemDTOsActual = Assert.IsType<List<ItemDTO>>(okResult.Value); //get the item list (this assign with the comparison is possible)
        Assert.Equal(expectedItems, itemDTOsActual);
    }

    public static IEnumerable<object?[]> TestCasesFor_GetItemsForPurchase_OK() //GOOD CASES, DIFFERENT FILTERS
    {
        var itemDTOs = new List<ItemDTO>() { //to use in TC#
            new ItemDTO("Boxing glove", 18, Gender.Other, 25, "M", 100, "Boxing"),
            new ItemDTO("Trunks", 18, Gender.Male,  20, "M",  50, "Boxing"),
        };

        var itemDTOsTC1 = new List<ItemDTO>() { itemDTOs[0], itemDTOs[1] }; //ORDER BY NAME THEN PRICE IN METHOD!! First 
        var itemDTOsTC2 = new List<ItemDTO>() { itemDTOs[0] };  //name filter, first case
        var itemDTOsTC3 = new List<ItemDTO>() { itemDTOs[1] };  //gender filter, second case
        var itemDTOsTC4 = new List<ItemDTO>() { itemDTOs[0], itemDTOs[1] }; //sport filter (assigned earlier), both are boxing

        var allTests = new List<object?[]> //each is the three filters and then the expected
        {             
            new object?[] { null, null, null, itemDTOsTC1, },
            new object?[] { "Boxing glove", null, null, itemDTOsTC2, },
            new object?[] { null, Gender.Male, null, itemDTOsTC3, },
            new object?[] { null, null, "Boxing", itemDTOsTC4, },
        };

        return allTests;
    }

    [Theory]
    [Trait("LevelTesting", "Unit Testing")]
    [MemberData(nameof(TestCasesFor_GetItemsForPurchase_OK))]
    public async Task GetItemsForPurchase_filter_test(string? itemName, Gender? itemGender, string? itemSport, List<ItemDTO> expectedItems)
    {
        //ARRANGE
        var mock = new Mock<ILogger<ItemsController>>();
        ILogger<ItemsController> logger = mock.Object;
        var controller = new ItemsController(_context, logger);
        //ACT
        var result = await controller.GetItemsForPurchase(itemName, itemGender, itemSport);
        //ASSERT
        //we check that the response type is OK
        var okResult = Assert.IsType<OkObjectResult>(result);
        //and obtain the list of items, same assignation
        var itemDTOsActual = Assert.IsType<List<ItemDTO>>(okResult.Value);
        Assert.Equal(expectedItems, itemDTOsActual);
    }
}