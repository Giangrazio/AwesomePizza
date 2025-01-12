using Xunit;
using AwesomePizzaBLL.Services;
using AwesomePizzaBLL.Models;
using AwesomePizzaDAL.Repositories;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using GenericUnitOfWork.UoW;
using AwesomePizzaDAL.Entities;
using AwesomePizzaBLL.Mapping;
using AwesomePizza.Tests.Common;

using Microsoft.AspNetCore.Mvc;
using System.Net;
using AwesomePizza.Tests.DatabaseSnapshot;

public class OrderServiceTests
{
    [Fact]
    public async Task CreateOrder_ShouldCallAddMethod_WhenOrderIsValidAsync()
    {
        var serverInstance = FakeServerFactory.GetFakeServer();
        // Arrange: Crea un ordine fittizio
        var orderModel = new OrderModel { 
            CustomerName = "John Doe", 
            CustomerEmail = "d@d.it",
            CustomerPhone = "5215185548",
            DeliveryAddress = "edjwfuj",
            OrderProducts = new List<OrderProductModel>() { 
                new OrderProductModel() { 
                    ProductId = 1, 
                    Quantity = 1 
                } 
            } 
        };
        
        var orderResult = await ApiRequester.PostAsync<OrderModel>(serverInstance, "api/order", orderModel);

        Assert.NotNull(orderResult);
        Assert.Equal(orderModel.CustomerName, orderResult.Value.CustomerName);
        Assert.Equal(orderModel.OrderProducts.Count, orderResult.Value.OrderProducts.Count);   
        Assert.True(orderResult.Value.OrderDate > DateTime.MinValue);
    }

    [Fact]
    public async Task GetOrdersr_ShouldCallAddMethod_WhenClienIsNotAuthorize()
    {
        var serverInstance = FakeServerFactory.GetFakeServer();
                
        var ordersResult = await ApiRequester.GetAsync<AwesomePizzaAPI.HttpMessage>(serverInstance, "api/order");

        Assert.NotNull(ordersResult);
        Assert.Equal("Accesso negato", ordersResult.Value.message);
    }

    [Fact]
    public void GetOrderStatusByCode_ShouldThrowException_WhenOrderCodeIsInValid()
    {
        var serverInstance = FakeServerFactory.GetFakeServer();
                
        Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await ApiRequester.GetAsync<OrderModel>(serverInstance, $"api/order/bycode/ddddddd"));
    }

    [Fact]
    public void CreateOrder_ShouldThrowException_WhenOrderIsNull()
    {
        var serverInstance = FakeServerFactory.GetFakeServer();
        // Act & Assert
        Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await ApiRequester.PostAsync<OrderModel>(serverInstance, "api/order", null));
    }
}