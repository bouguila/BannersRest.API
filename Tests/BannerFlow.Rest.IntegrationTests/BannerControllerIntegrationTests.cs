using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BannerFlow.Rest.Entities.Models;
using Xunit;
using System.Net;
using System.Text;
using System.Linq;
using BannerFlow.Rest.Utilities;

namespace BannerFlow.Rest.IntegrationTests
{
    public class BannerControllerIntegrationTests : IClassFixture<TestWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly string baseUri = "/api/banner";
        private readonly int _existingBannerId = 1000;
        private readonly int _InexistingBannerId = 9999;

        public BannerControllerIntegrationTests(TestWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add("x-api-key", "abcde1234");

            EnsureTestItems();
        }

        private async Task EnsureTestItems()
        {
            await CreateBanner(new Banner() { Id = _existingBannerId, Html = "test" });
        }

        /************************* POST Method  *************************/

        [Fact]
        public async Task Create_WhenCalled_ReturnsCreatedBanner()
        {
            //Arrange
            //Act
            //Assert
            var banner = new Banner
            {
                Id = _InexistingBannerId,
                Html = "<h1>element 2</h1>"
            };

            //Act
            var resBanner = await CreateBanner(banner);

            //Assert
            Assert.Equal(_InexistingBannerId, resBanner.Id);
            Assert.Equal("<h1>element 2</h1>", resBanner.Html);

            await DeleteBanner(_InexistingBannerId);
        }

        [Fact]
        public async Task Create_ValidObjectPassed_ObjectCreatedWithNullModified()
        {
            //Arrange
            var banner = new Banner
            {
                Id = _InexistingBannerId,
                Html = "<h1>element 2</h1>"
            };

            //Act
            var resBanner = await CreateBanner(banner);

            //Assert
            Assert.Null(resBanner.Modified);

            //revert to the revious state (rollback)
            await DeleteBanner(_InexistingBannerId);
        }

        [Fact]
        public async Task Create_ValidObjectPassed_ObjectCreatedWithCorrectTimestamp()
        {
            //Arrange
            var utcNow = DateTime.UtcNow;
            var banner = new Banner
            {
                Id = _InexistingBannerId,
                Html = "<h1>element 2</h1>",
                Created = utcNow
            };

            //Act
            var resBanner = await CreateBanner(banner);

            //Assert
            Assert.Equal(utcNow, resBanner.Created);

            //revert to the revious state (rollback)
            await DeleteBanner(_InexistingBannerId);
        }

        [Fact]
        public async Task Create_IdNotPassed_ReturnsBadRequest()
        {
            // Arrange
            var idMissingBanner = new Banner()
            {
                Html = "<h1> test </h1>"
            };

            //Act
            var content = new StringContent(JsonConvert.SerializeObject(idMissingBanner), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(baseUri, content);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Create_InvalidHtmlPassed_ReturnsBadRequest()
        {
            //Arrange
            var banner = new Banner
            {
                Id = 10,
                Html = "<h1>element 1"
            };

            //Act
            var content = new StringContent(JsonConvert.SerializeObject(banner), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(baseUri, content);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }


        /************************ GET method *************************/
        [Fact]
        public async Task GetAll_WhenCalled_ReturnsListOfBanners()
        {
            //Arrange
            var httpResponse = await _client.GetAsync(baseUri);
            httpResponse.EnsureSuccessStatusCode();

            //Act
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var banners = JsonConvert.DeserializeObject<IEnumerable<Banner>>(stringResponse);

            //Assert
            Assert.IsType<List<Banner>>(banners);
        }

        [Fact]
        public async Task GetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Act
            var httpResponse = await _client.GetAsync(baseUri + '/' + _InexistingBannerId);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task GetById_ExistingIdPassed_ReturnsCorrectBanner()
        {
            //Arrange
            var httpResponse = await _client.GetAsync(baseUri + '/' + _existingBannerId);
            httpResponse.EnsureSuccessStatusCode();

            //Act
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var banner = JsonConvert.DeserializeObject<Banner>(stringResponse);

            //Assert
            Assert.IsType<Banner>(banner);
            Assert.NotEmpty(banner.Html);
        }

        [Fact]
        public async Task GetHtml_ExistingIdPassed_HtmlRenderedCorrectly()
        {
            // Arrange
            var existingId = _existingBannerId;

            // Act
            var httpResponse = await _client.GetAsync(baseUri + '/' + existingId + "/html");
            httpResponse.EnsureSuccessStatusCode();
            var response = httpResponse.Content;
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("text/html", response.Headers.ContentType.ToString());
            Assert.True(HtmlUtility.IsValid(stringResponse));
        }


        /***********************  DELETE  method ************************/
        [Fact]
        public async Task Remove_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            //Act
            var httpResponse = await _client.DeleteAsync(baseUri + '/' + _InexistingBannerId);
            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Remove_ExistingIdPassed_ReturnsOkResult()
        {
            //Act
            var httpResponse = await _client.DeleteAsync(baseUri + '/' + _existingBannerId);
            httpResponse.EnsureSuccessStatusCode();

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, httpResponse.StatusCode);

            //Rollback latest deletion transaction
            await CreateBanner(new Banner() {Id=_existingBannerId, Html="test" });
        }

        [Fact]
        public async Task Remove_ExistingIdPassed_RemovesOneItem()
        {
            // Arrange
            var bannersBefore = await GetAllBanners();
            var countBefore = bannersBefore.Count();

            // Act
            var httpRes = await _client.DeleteAsync(baseUri + '/' + _existingBannerId);
            httpRes.EnsureSuccessStatusCode();

            var banners = await GetAllBanners();
            var countItems = banners.Count();

            // Assert
            Assert.Equal(countBefore - 1, countItems);

            //Rollback latest deletion transaction
            await CreateBanner(new Banner() { Id = _existingBannerId, Html = "test" });
        }

        /***********************  PUT  method ************************/
        [Fact]
        public async Task Update_NotExistingIdPassed_ReturnsNotFoundResponse()
        {
            //Arrange
            var banner = new Banner
            {
                Id = _InexistingBannerId,
                Html = "<h1>modified</h1>"
            };

            //Act
            var content = new StringContent(JsonConvert.SerializeObject(banner), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(baseUri + '/' + _InexistingBannerId, content);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Update_ExistingIdPassed_ReturnsNoContentSuccessResult()
        {
            //Arrange
            var banner = new Banner
            {
                Id = _existingBannerId,
                Html = "<h1>modified</h1>"
            };

            //Act
            var content = new StringContent(JsonConvert.SerializeObject(banner), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(baseUri + '/' + _existingBannerId, content);

            //Assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Update_ExistingIdPassed_ModifiedUtcNow()
        {
            //Arrange
            var banner = new Banner
            {
                Id = _existingBannerId,
                Html = "<h1>modified</h1>"
            };

            //Act
            var content = new StringContent(JsonConvert.SerializeObject(banner), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(baseUri + '/' + _existingBannerId, content);
            httpResponse.EnsureSuccessStatusCode();

            var updatedBanner = await GetBanner(_existingBannerId);

            //Assert
            Assert.NotNull(updatedBanner.Modified);
        }

        [Fact]
        public async Task Update_ExistingIdPassed_BannerUpdated()
        {
            // Arrange
            var banner = new Banner()
            {
                Id = _existingBannerId,
                Html = "<h1>updated 1</h1>"
            };

            //Act
            var content = new StringContent(JsonConvert.SerializeObject(banner), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(baseUri + '/' + _existingBannerId, content);
            httpResponse.EnsureSuccessStatusCode();

            var updatedBanner = await GetBanner(_existingBannerId);

            // Assert
            Assert.Equal("<h1>updated 1</h1>", updatedBanner.Html);
        }

        [Fact]
        public async Task Update_BannerIdDifferentRequestId_ReturnsNotFoundResponse()
        {
            //Arrange
            var id = _existingBannerId;
            var banner = new Banner
            {
                Id = 13,
                Html = "<h1>Test</h1>"
            };

            //Act
            var content = new StringContent(JsonConvert.SerializeObject(banner), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PutAsync(baseUri + '/' + id, content);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }


        #region
        private async Task<List<Banner>> GetAllBanners()
        {
            var httpResponse = await _client.GetAsync(baseUri);
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse1 = await httpResponse.Content.ReadAsStringAsync();
            var banners = JsonConvert.DeserializeObject<IEnumerable<Banner>>(stringResponse1);
            return banners.ToList();
        }

        private async Task<Banner> GetBanner(int id)
        {
            var httpGetResponse = await _client.GetAsync(baseUri + '/' + id);
            httpGetResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpGetResponse.Content.ReadAsStringAsync();
            var banner = JsonConvert.DeserializeObject<Banner>(stringResponse);
            return banner;
        }

        private async Task<Banner> CreateBanner(Banner banner)
        {
            var content = new StringContent(JsonConvert.SerializeObject(banner), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(baseUri, content);

            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Banner>(stringResponse);
        }

        private async Task DeleteBanner(int id)
        {
            var httpResponse = await _client.DeleteAsync(baseUri + '/' + id);
            httpResponse.EnsureSuccessStatusCode();
        }
        #endregion
    }
}