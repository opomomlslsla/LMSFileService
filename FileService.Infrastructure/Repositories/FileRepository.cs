using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using FileService.Domain.Models;
using FileService.Domain.DTO;

namespace FileService.Infrastructure.Repositories
{
    public class FileRepository
    {
        private readonly IMongoCollection<FileModel> _FilesCollection;
        private readonly IGridFSBucket _GridFSBucket;


        public FileRepository(
            IOptions<DataBaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            _FilesCollection = mongoDatabase.GetCollection<FileModel>(
                DatabaseSettings.Value.FileCollectionName);
            _GridFSBucket = new GridFSBucket(mongoDatabase);
        }

        public async Task<Stream> GetFileById(string id)
        {
            Stream fileStream = await _GridFSBucket.OpenDownloadStreamAsync(new ObjectId(id));
            return fileStream;
        }

        public async Task<FileModel> GetFileModelById(string id)
        {
            ObjectId _id = new ObjectId(id);
            var result = await _FilesCollection.Find(i => i.Id == _id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<FileModel> AddFileToMongo(FileModelDTO fileModelDTO)
        {
            var fileStream = fileModelDTO.formFile.OpenReadStream();
            string id = (await _GridFSBucket.UploadFromStreamAsync(fileModelDTO.formFile.FileName, fileStream)).ToString();

            FileModel fileModel = new FileModel
            {
                Name = fileModelDTO.formFile.FileName,
                Id = new ObjectId(id),
                IsConnectedToDocument = fileModelDTO.IsConnectedToDocument,
                DocumentId = fileModelDTO.DocumentId
            };

            await _FilesCollection.InsertOneAsync(fileModel);
            return fileModel;
        }

        /*
        public Task UpdateFile(FileModel file)
        {
            var filter = Builders<FileModel>.Filter.Eq("Id", file.Id);
            return _FilesCollection.ReplaceOneAsync(filter, file, new ReplaceOptions { IsUpsert = true });
        }
        */

        public async void DeleteFile(ObjectId id)
        {
            await _FilesCollection.DeleteOneAsync(i => i.Id == id);
            await _GridFSBucket.DeleteAsync(id);
        }
    }
}

