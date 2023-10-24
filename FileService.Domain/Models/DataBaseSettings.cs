using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;


namespace FileService.Domain.Models
{
    public class DataBaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string VideoCollectionName { get; set; }
        public string FileCollectionName { get; set; }
        public string ImageCollectionName { get; set; }


    }
}
