using AdvancedProjectMVC.Models;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;

namespace AdvancedProjectMVC.Controllers
{
    public class FileSharingController : Controller
    {
        private BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=advancedprojectfileshare;AccountKey=PX9Acb1JmVX9oQ2ZDSjzoMXimDQLb0cuInpzK/xxAP5GeYNgFoovg6qIBjL2uB04VGeaXZKGwnOX+AStnNBvxw==;EndpointSuffix=core.windows.net");

        private BlobContainerClient? blobContainerClient;
        private string downloadPath = System.Convert.ToString(Microsoft.Win32.Registry.GetValue(
             @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
            , "{374DE290-123F-4565-9164-39C4925E467B}"
            , String.Empty));
        
        public async Task<IActionResult> Index()
        {
            return View(await GetAllFiles("serverName"));
        }

        [HttpGet("GetAllFiles")]
        public async Task<List<string>> GetAllFiles(string containerName)
        {
            List<string> files = new List<string>();
            containerName = "filesharecontainer";
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            try
            {
                var resultSegment = blobContainerClient.GetBlobsAsync().AsPages();

                await foreach (Page<BlobItem> blobPage in resultSegment)
                {
                    foreach (BlobItem blobItem in blobPage.Values)
                    {
                        Console.WriteLine("Blob name: {0}", blobItem.Name);
                        files.Add(blobItem.Name);
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
            blobContainerClient = null;
            return files;
        }


        public IActionResult Upload() { 
            return View();
        }


        //TODO: Archives
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile TestFile, string containerName)
        {
            containerName = "filesharecontainer";
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                TestFile.CopyTo(stream);  
            }

            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            string fileName = (TestFile.FileName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(filePath, true);
            blobContainerClient = null;
            
            return RedirectToAction("Index");
        }

        public async Task<BlobObject> DownloadFile(string containerName, string fileName)
        {
            
            containerName = "filesharecontainer";
            fileName = "testShare.txt";
            
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
           
            //downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            try
            {
                BlobDownloadResult blobDownloadResult = await blobClient.DownloadContentAsync();
                var downloadData = blobDownloadResult.Content.ToStream();

                BlobObject blobObject = new BlobObject { BlobContent = downloadData, BlobName = fileName };

                //FileStreamResult fsr =  File(blobObject.BlobContent, "text/plain", fileName);

                //using (var resultStream = System.IO.File.Create(downloadPath + fileName))
                //{
                //    blobObject.BlobContent.CopyTo(resultStream);
                //}

                blobContainerClient = null;
                return blobObject;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                blobContainerClient= null;
                return null;
            }            
        }
    }
}
