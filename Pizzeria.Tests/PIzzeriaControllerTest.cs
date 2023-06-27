using System;
using Xunit;
using Moq;
using Pizzeria.Core.Interfaces;
using Pizzeria.Controllers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Pizzeria.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Pizzeria.Tests
{
    public class PizzeriaControllerTest
    {
        [Fact]
        public async Task GetAllAsync_ReturnsOkResultWithExpectedData()
        {
            // arrange
            var mockService = new Mock<IPizzeriaService>();
            var mockLogger = new Mock<ILogger<PizzeriaController>>();

            var home = new PizzeriaController(mockLogger.Object, mockService.Object);

            var expectedResult = new List<OutletPizzaDetail>()
            {
                new OutletPizzaDetail()
                {
                    OutletID = 1,
                    OutletName = "mock pizzeria",
                    PizzaOrderList = new List<PizzaDetail>()
                    {
                        new PizzaDetail()
                        {
                            PizzaID = 44,
                            Price = 12,
                            Ingredients = "mock ingredients"
                        }
                    }
                }
            };
            mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(expectedResult);

            // act
            var result = await home.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResult = Assert.IsAssignableFrom<IList<OutletPizzaDetail>>(okResult.Value);
            Assert.Equal(expectedResult, actualResult);
        }

        //[Theory]
        //[InlineData(42, "Foo")] // Example data set 1
        //[InlineData(100, "Bar")] // Example data set 2
        [Fact]
        public async Task GetTotalPriceAsync_ReturnsOkResultWithTotalPrice()
        {
            // arrange
            var mockService = new Mock<IPizzeriaService>();
            var mockLogger = new Mock<ILogger<PizzeriaController>>();

            var home = new PizzeriaController(mockLogger.Object, mockService.Object);

            int expectedResult = 100;
            mockService.Setup(s => s.GetOrderPrice(It.IsAny<CustomerOrder>())).ReturnsAsync(expectedResult);

            // act
            var result = await home.GetTotalPriceAsync(new CustomerOrder());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResult = Assert.IsAssignableFrom<int>(okResult.Value);
        }

        [Fact]
        public async Task OpenNewOutletAsync_ReturnsOkResultWithNewOutletData()
        {
            // arrange
            var mockService = new Mock<IPizzeriaService>();
            var mockLogger = new Mock<ILogger<PizzeriaController>>();

            var home = new PizzeriaController(mockLogger.Object, mockService.Object);

            var expectedResult = new OutletPriceChange();
            mockService.Setup(s => s.OpenNewOutletAsync(It.IsAny<OutletOpenNew>())).ReturnsAsync(expectedResult);

            // act
            var result = await home.OpenNewOutletAsync(new OutletOpenNew());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResult = Assert.IsAssignableFrom<OutletPriceChange>(okResult.Value);
        }

        [Fact]
        public async Task UpdatePizzaPriceAsync_ReturnsOkResultWithUpdatedPriceList()
        {
            // arrange
            var mockService = new Mock<IPizzeriaService>();
            var mockLogger = new Mock<ILogger<PizzeriaController>>();

            var home = new PizzeriaController(mockLogger.Object, mockService.Object);

            var expectedResult = new OutletPriceChange();
            mockService.Setup(s => s.UpdatePizzaPriceAsync(It.IsAny<OutletPriceChange>())).ReturnsAsync(expectedResult);

            // act
            var result = await home.UpdatePizzaPriceAsync(new OutletPriceChange());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualResult = Assert.IsAssignableFrom<OutletPriceChange>(okResult.Value);
        }
    }
}
