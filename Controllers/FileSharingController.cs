using AdvancedProjectMVC.Models;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        public async Task GetAllFiles()
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient("filesharecontainer");
            try
            {
                var resultSegment = blobContainerClient.GetBlobsAsync().AsPages();

                await foreach (Page<BlobItem> blobPage in resultSegment)
                {
                    foreach (BlobItem blobItem in blobPage.Values)
                    {
                        Console.WriteLine("Blob name: {0}", blobItem.Name);
                    }

                    Console.WriteLine();
                }
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        //TODO: Archives
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
