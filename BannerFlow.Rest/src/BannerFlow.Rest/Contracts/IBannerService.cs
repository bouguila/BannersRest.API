using BannerFlow.Rest.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BannerFlow.Rest.Contracts
{
    public interface IBannerService 
    {
        List<Banner> GetAllBanners();

        Banner GetBannerById(int id);

        Banner AddBanner(Banner banner);

        void UpdateBanner(int id, Banner bannerIn);

        void DeleteBanner(Banner bannerIn);

        void DeleteBannerById(int id);
    }
}
