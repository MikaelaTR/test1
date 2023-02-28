using AdvancedProjectMVC.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedProjectMVC.Controllers
{
    public class FileSharingController : Controller
    {
        private BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=advancedprojectfileshare;AccountKey=PX9Acb1JmVX9oQ2ZDSjzoMXimDQLb0cuInpzK/xxAP5GeYNgFoovg6qIBjL2uB04VGeaXZKGwnOX+AStnNBvxw==;EndpointSuffix=core.windows.net");

        private BlobContainerClient blobContainerClient;
        
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("GetFile")]
        //public async Task<IActionResult> GetFile(string path)
        //{
        //    BlobObject result = (BlobObject)await GetFile(path);
        //    return File(result.Content, result.ContentType);
        //}

        [HttpPost("UploadFile")]
        public async Task UploadFile(SharedFile file)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient("filesharecontainer");
            string fileName = file.FileName;
            string filePath = file.FilePath;
            BlobClient blobClient = blobContainerClient.GetBlobClient("Test String");
            Console.WriteLine("upload file gogogo ");
            await blobClient.UploadAsync(BinaryData.FromString("test string uploaded"));           
        }
    }
}
