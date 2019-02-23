using System;
using Xunit;
using BannerFlow.Rest.Controllers;
using System.Collections.Generic;
using BannerFlow.Rest.Contracts;
using BannerFlow.Rest.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BannerFlow.Rest.Utilities;

namespace BannerFlow.Rest.Tests
{
    public class BannerControllerTest
    {
        BannerController _controller;
        Mock<IBannerService> _service;

        public BannerControllerTest()
        {
            setupServiceMock(_service);

            _controller = new BannerController(_service.Object);
        }

        #region
        private void setupServiceMock(Mock<IBannerService> service)
        {
            var testBanner = new Banner() { Id = 1, Html = "test" };
            var listB = new List<Banner>();
            listB.Add(testBanner);
            _service = new Mock<IBannerService>();
            _service.Setup(m => m.DeleteBannerById(It.IsAny<int>())).Callback<int>((id) => listB.RemoveAll(x => x.Id == id));
            _service.Setup(m => m.GetBannerById(1)).Returns(testBanner);
            _service.Setup(m => m.GetAllBanners()).Returns(listB);
        }
        #endregion

        /**************************** GetAllBanners method ****************************/
        [Fact]
        public void GetAll_WhenCalled_ReturnsListOfBanners()
        {
            // Act
            var okResult = _controller.GetAll();

            // Assert
            Assert.IsType<List<Banner>>(okResult.Value);
        }

        [Fact]
        public void GetAll_WhenCalled_ReturnsAllItems()
        {
            var response = _controller.GetAll();
            // Act
            var items = response.Value as List<Banner>;

            // Assert
            Assert.NotNull(items);
            Assert.Single<Banner>(items);
        }

        /******************  GetById method  ******************/
        //
        [Fact]
        public void GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _controller.GetById(5);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsBanner()
        {
            // Arrange
            var id = 1;

            // Act
            var banner = _controller.GetById(id);
            // Assert
            Assert.IsType<Banner>(banner.Value);
        }

        [Fact]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            // Arrange
            var id = 1;
            var html = "test";

            // Act
            var okResult = _controller.GetById(id);

            // Assert
            Assert.Equal(id, (okResult.Value as Banner).Id);
            Assert.Equal(html, (okResult.Value as Banner).Html);
        }


        /************************* create Method  *************************/
        [Fact]
        public void Add_InvalidHtmlPassed_ReturnsBadRequest()
        {
            // Arrange
            var idMissingItem = new Banner()
            {
                Html = "<h1>test<h1>",
                Created = DateTime.Now
            };
            _controller.ModelState.AddModelError("Html", "Invalid");

            // Act
            var badResponse = _controller.Create(idMissingItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }

        [Fact]
        public void Add_ExistingId_ReturnsBadRequest()
        {
            // Arrange
            var banner = new Banner()
            {
                Html = "<h1>",
                Created = DateTime.Now
            };
            _controller.ModelState.AddModelError("Id", "Alresdy Exists");

            // Act
            var badResponse = _controller.Create(banner);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            Banner banner = new Banner()
            {
                Html = "<h2> Test </h2>",
                Created = DateTime.UtcNow
            };

            // Act
            var createdResponse = _controller.Create(banner);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(createdResponse.Result);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var testItem = new Banner()
            {
                Html = "<h2> Test </h2>",
            };

            // Act
            var createdResponse = _controller.Create(testItem);
            var result = createdResponse.Result as CreatedAtRouteResult;
            var item = result.Value as Banner;

            // Assert
            Assert.IsType<Banner>(item);
            Assert.Equal("<h2> Test </h2>", item.Html);
        }

        /*******************************  Remove  method *********************************/
        [Fact]
        public void Remove_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var notExistingId = 5;

            // Act
            var notFound = _controller.DeleteById(notExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(notFound);
        }

        [Fact]
        public void Remove_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            var existingId = 1;

            // Act
            var okResponse = _controller.DeleteById(existingId);

            // Assert
            Assert.IsType<NoContentResult>(okResponse);
        }

        [Fact]
        public void Remove_ExistingIdPassed_RemovesOneItem()
        {
            // Arrange
            var existingId = 1;

            // Act
            var okResponse = _controller.DeleteById(existingId);
            var li = _controller.GetAll().Value as List<Banner>;

            // Assert
            Assert.Equal(0, li.Count);
        }
    }
}
