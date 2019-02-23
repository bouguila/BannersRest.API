using BannerFlow.Rest.Entities.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace BannerFlow.Rest.Entities.Models
{
    [HasValidHtml]
    public class Banner
    {
        [BsonId]
        public int Id { get; set; }

        [Required]
        [BsonElement("Html")]
        public string Html { get; set; }

        [BsonElement("Created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [BsonElement("Modified")]
        public DateTime? Modified { get; set; }
    }
}
