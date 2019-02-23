using BannerFlow.Rest.Contracts;
using BannerFlow.Rest.Entities.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace BannerFlow.Rest.Services
{
    public class BannerService : IBannerService
    {
        private readonly IMongoCollection<Banner> _banners;

        public BannerService(IConfiguration config)
        {
            var dbClient = new MongoClient(config.GetConnectionString("BannerStoreDb"));
            var database = dbClient.GetDatabase("BannerFlowDb");
            
            _banners = database.GetCollection<Banner>("Banners");
        }

        public List<Banner> GetAllBanners()
        {
            return _banners.Find(banner => true).ToList();
        }

        public Banner GetBannerById(int id)
        {
            return _banners.Find<Banner>(banner => banner.Id == id).FirstOrDefault();
        }

        public Banner AddBanner(Banner banner)
        {
            _banners.InsertOne(banner);
            return banner;
        }

        public void UpdateBanner(int id, Banner bannerIn)
        {
            var filter = Builders<Banner>.Filter.Eq("Id", id);
            var oldBanner = _banners.Find<Banner>(filter).FirstOrDefault();

            bannerIn.Created = oldBanner.Created;
            bannerIn.Modified= DateTime.UtcNow;

            _banners.ReplaceOne(filter, bannerIn);
        }

        public void DeleteBanner(Banner bannerIn)
        {
            _banners.DeleteOne(banner => banner.Id == bannerIn.Id);
        }

        public void DeleteBannerById(int id)
        {
            _banners.DeleteOne(banner => banner.Id == id);
        }
    }
}