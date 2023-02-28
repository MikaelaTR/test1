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

        [HttpGet("GetAllFiles")]
        public async Task GetAllFiles(string path)
        {

        }

        [HttpPost("UploadFile")]
        public async Task UploadFile(IFormFile TestFile)
        {
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                TestFile.CopyTo(stream);  
            }

            blobContainerClient = blobServiceClient.GetBlobContainerClient("filesharecontainer");
            string fileName = (TestFile.FileName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(filePath, true);           
        }


    }
}
