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
    public class HtmlUtilityUnitTest
    {
        BannerController _controller;
        Mock<IBannerService> _service;

        public HtmlUtilityUnitTest()
        {
            _service = new Mock<IBannerService>();
            _controller = new BannerController(_service.Object);
        }


        [Fact]
        public void IsValid_WhenInvalidContent_ReturnsFalse()
        {
            // Arrange
            var html = "<html><h2></2></html>";
            // Act
            var result=HtmlUtility.IsValid(html);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_WhenValidContent_ReturnsTrue()
        {
            // Arrange
            var html = "<html><h2>title</h2></html>";
            // Act
            var result = HtmlUtility.IsValid(html);

            // Assert
            Assert.True(result);
        }
    }
}
