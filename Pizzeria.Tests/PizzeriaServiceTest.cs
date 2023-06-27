using System;
using Xunit;
using Moq;
using Pizzeria.Core.Interfaces;
using Pizzeria.Services.Services;
using Pizzeria.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pizzeria.Tests
{
    public class PizzeriaServiceTest
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllOutletDetailsData()
        {
            // arrange
            var mockRepository = new Mock<IPizzeriaRepository>();

            var service = new PizzeriaService(mockRepository.Object);

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
            mockRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(expectedResult);

            // act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0, false, 0, false, 0, false)]
        [InlineData(1, false, 0, false, 0, false)]
        [InlineData(0, false, 1, false, 0, false)]
        [InlineData(0, false, 0, false, 1, false)]
        [InlineData(1, true, 0, false, 0, false)]
        [InlineData(0, false, 1, true, 0, false)]
        [InlineData(0, false, 0, false, 1, true)]
        [InlineData(1, false, 1, false, 1, false)]
        [InlineData(2, false, 3, false, 9, false)]
        [InlineData(33, true, 2, true, 0, true)]
        [InlineData(0, true, 0, true, 0, true)]
        public async Task GetOrderPrice_ReturnsTotallPrice(
            int numOfPizza1, bool pizza1Topping,
            int numOfPizza2, bool pizza2Topping,
            int numOfPizza3, bool pizza3Topping)
        {
            // arrange
            var mockRepository = new Mock<IPizzeriaRepository>();

            var service = new PizzeriaService(mockRepository.Object);

            int price1 = 12, price2 = 24, price3 = 5;
            var mockData = new List<OutletPizzaDetail>()
            {
                new OutletPizzaDetail()
                {
                    OutletID = 1,
                    OutletName = "mock pizzeria",
                    PizzaOrderList = new List<PizzaDetail>()
                    {
                        new PizzaDetail() {PizzaID = 1,Price = price1, Ingredients = ""},
                        new PizzaDetail() {PizzaID = 2,Price = price2, Ingredients = ""},
                        new PizzaDetail() {PizzaID = 3,Price = price3, Ingredients = ""},
                    }
                }
            };

            int expectedPrice = numOfPizza1 * price1 + (numOfPizza1 > 0 && pizza1Topping ? 1 : 0);
            expectedPrice += numOfPizza2 * price2 + (numOfPizza2 > 0 && pizza2Topping ? 1 : 0);
            expectedPrice += numOfPizza3 * price3 + (numOfPizza3 > 0 && pizza3Topping ? 1 : 0);

            mockRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(mockData);

            int? toppingID1 = null, toppingID2 = null, toppingID3 = null;
            if (pizza1Topping)
                toppingID1 = 1;

            if (pizza2Topping)
                toppingID2 = 1;

            if (pizza3Topping)
                toppingID3 = 1;

            var order = new CustomerOrder() {
                OutletID = 1,
                PizzaOrderList = new List<SinglePizzaOrder>()
                {
                    new SinglePizzaOrder(){PizzaID = 1, Quantity = numOfPizza1, ToppingID = toppingID1},
                    new SinglePizzaOrder(){PizzaID = 2, Quantity = numOfPizza2, ToppingID = toppingID2},
                    new SinglePizzaOrder(){PizzaID = 3, Quantity = numOfPizza3, ToppingID = toppingID3}
                }
            };

            // act
            var result = await service.GetOrderPrice(order);

            // Assert
            Assert.Equal(expectedPrice, result);
        }

        [Fact]
        public async Task GetOrderPrice_ThrowExceptionWhenOutletIDNotExist()
        {
            // arrange
            var mockRepository = new Mock<IPizzeriaRepository>();

            var service = new PizzeriaService(mockRepository.Object);

            var mockData = new List<OutletPizzaDetail>()
            {
                new OutletPizzaDetail()
                {
                    OutletID = 1,
                    OutletName = "mock pizzeria",
                    PizzaOrderList = new List<PizzaDetail>()
                }
            };

            mockRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(mockData);

            var order = new CustomerOrder()
            {
                OutletID = 2,
                PizzaOrderList = new List<SinglePizzaOrder>()
            };

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetOrderPrice(order));

        }

        [Fact]
        public async Task GetOrderPrice_ThrowExceptionWhenPizzaNotForSale()
        {
            // arrange
            var mockRepository = new Mock<IPizzeriaRepository>();

            var service = new PizzeriaService(mockRepository.Object);

            var mockData = new List<OutletPizzaDetail>()
            {
                new OutletPizzaDetail()
                {
                    OutletID = 1,
                    OutletName = "mock pizzeria",
                    PizzaOrderList = new List<PizzaDetail>()
                    {
                        new PizzaDetail() {PizzaID = 1,Price = 12, Ingredients = ""},
                    }
                }
            };

            mockRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(mockData);

            var order = new CustomerOrder()
            {
                OutletID = 1,
                PizzaOrderList = new List<SinglePizzaOrder>()
                {
                    new SinglePizzaOrder(){PizzaID = 3, Quantity = 1, ToppingID = 1}
                }
            };

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => service.GetOrderPrice(order));
        }
    }
}
