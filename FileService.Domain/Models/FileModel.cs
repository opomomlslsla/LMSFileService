using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain.Models
{
    public class FileModel
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string FileId { get; set; }
        public bool IsConnectedToDocument { get; set; }
        public Guid DocumentId { get; set; }
    }
}
