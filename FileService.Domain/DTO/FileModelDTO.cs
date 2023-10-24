using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileService.Domain.DTO
{
    public class FileModelDTO
    {
        public IFormFile formFile { get; set; }
        public bool IsConnectedToDocument { get; set; }
        public Guid DocumentId { get; set; }

    }
}
