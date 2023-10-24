using FileService.Domain.DTO;
using FileService.Domain.Models;
using FileService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using MongoDB.Bson;
using System.Net.Http.Headers;
namespace FileServiceAPI.Controllers
{
    [Route("api/MongoController")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileRepository _FileRepository;
        public FileController(FileRepository fileRepository)
        {
            _FileRepository = fileRepository;
        }


        [HttpGet("File/{id}")]
        [RequestSizeLimit(5368709120)]
        public async Task<IActionResult> GetVideo(string id)
        {
            var file = await _FileRepository.GetFileById(id);
            FileModel fileModel = await _FileRepository.GetFileModelById(id);
            string fileName = fileModel.Name;
            var result = File(file, "video/mp4", fileName);
            return result;
        }


        [HttpGet("{id}")]
        [RequestSizeLimit(5368709120)]
        public async Task<IActionResult> GetFile(string id)
        {
            var File = await _FileRepository.GetFileById(id);

            return Ok(File);
        }


        [HttpPost]
        [RequestSizeLimit(5368709120)]
        public async Task<IActionResult> AddVideo([FromForm] FileModelDTO fileModelDTO)
        {
            var bob = await FileTest(fileModelDTO);

            var extensions = new List<string>() 
            {
                "application/pdf",
                "application/msword", 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "application/vnd.ms-powerpoint",
                "text/plain",
                "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };

            if (!extensions.Contains(fileModelDTO.formFile.ContentType) || fileModelDTO.formFile.Length == 0 || !bob.IsSuccessStatusCode)
            {
                return BadRequest("некорректный или опасный файл");
            }

            FileModel res = await _FileRepository.AddFileToMongo(fileModelDTO);
            return Ok($"id:{res.Id}");
        }



        /*
        public async Task<IActionResult> UpdateImage(ImageModel image)
        {
            await _ImageRepository.UpdateImage(image);
            return Ok();
        }
        */


        [HttpDelete]
        public async Task<IActionResult> DeleteFile(string id)
        {
            _FileRepository.DeleteFile(new ObjectId(id));
            return Ok();
        }


        private async Task<HttpResponseMessage> FileTest(FileModelDTO fileModelDTO)
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://www.filescan.io/api/scan/file");
            httpRequestMessage.Headers.Add("X-Api-Key", "2rvMHJfeZn_RS65uUzY6kLNRH0dnwxM0C9PJjkgU");

            content.Headers.Add("X-Api-Key", "2rvMHJfeZn_RS65uUzY6kLNRH0dnwxM0C9PJjkgU");

            content.Add(new StreamContent(fileModelDTO.formFile.OpenReadStream()), "file", fileModelDTO.formFile.FileName);

            /*
            var pairs = new List<KeyValuePair<string, string>>()
            {
                 new KeyValuePair<string, string>("save_preset", "false"),
                 new KeyValuePair<string, string>("description", ""),
                 new KeyValuePair<string, string>("tags", ""),
                 new KeyValuePair<string, string>("propagate_tags", "false"),
                 new KeyValuePair<string, string>("password", ""),
                 new KeyValuePair<string, string>("is_private", "true"),
                 new KeyValuePair<string, string>("skip_whitelisted", "true"),
                 new KeyValuePair<string, string>("rapid_mode", "true"),
                 new KeyValuePair<string, string>("osint", "true"),
                 new KeyValuePair<string, string>("extended_osint", "true"),
                 new KeyValuePair<string, string>("extracted_files_osint", "true"),
                 new KeyValuePair<string, string>("visualization", "true"),
                 new KeyValuePair<string, string>("file_download", "true"),
                 new KeyValuePair<string, string>("resolve_domains", "true"),
                 new KeyValuePair<string, string>("input_file_yara", "true"),
                 new KeyValuePair<string, string>("extracted_files_yara", "true"),
                 new KeyValuePair<string, string>("whois", "true"),
                 new KeyValuePair<string, string>("ips_meta", "true"),
                 new KeyValuePair<string, string>("images_ocr", "true")
            };
            content.Add(new FormUrlEncodedContent(pairs));
            */

            content.Add(new StringContent("false"), "save_preset");
            content.Add(new StringContent(""), "description");
            content.Add(new StringContent(""), "tags");
            content.Add(new StringContent("false"), "propagate_tags");
            content.Add(new StringContent(""), "password");
            content.Add(new StringContent("false"), "is_private");
            content.Add(new StringContent("false"), "skip_whitelisted");
            content.Add(new StringContent("false"), "rapid_mode");
            content.Add(new StringContent("true"), "osint");
            content.Add(new StringContent("true"), "extended_osint");
            content.Add(new StringContent("true"), "extracted_files_osint");
            content.Add(new StringContent("true"), "visualization");
            content.Add(new StringContent("true"), "files_download");
            content.Add(new StringContent("true"), "resolve_domains");
            content.Add(new StringContent("true"), "input_file_yara");
            content.Add(new StringContent("true"), "extracted_files_yara");
            content.Add(new StringContent("true"), "whois");
            content.Add(new StringContent("true"), "ips_meta");
            content.Add(new StringContent("true"), "images_ocr");


            httpRequestMessage.Content = content;
            var response = await client.SendAsync(httpRequestMessage);

            return response;

        }
    }
}

