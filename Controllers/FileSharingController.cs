using AdvancedProjectMVC.Models;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.Text.RegularExpressions;

namespace AdvancedProjectMVC.Controllers
{
    public class FileSharingController : Controller
    {
        private BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=advancedprojectfileshare;AccountKey=PX9Acb1JmVX9oQ2ZDSjzoMXimDQLb0cuInpzK/xxAP5GeYNgFoovg6qIBjL2uB04VGeaXZKGwnOX+AStnNBvxw==;EndpointSuffix=core.windows.net");

        private BlobContainerClient? blobContainerClient;

        public async Task<IActionResult> Index(string serverName)
        {
            string formattedName = String.Concat(serverName.Where(c => !Char.IsWhiteSpace(c)));
            formattedName = formattedName.ToLower();
            ViewBag.ServerName = serverName;
            ViewBag.ContainerName = formattedName;
            return View(await GetAllFiles(formattedName));
        }


        //container is named after the server (or its hash or whatever else)
        [HttpGet("GetAllFiles")]
        public async Task<List<SharedFile>> GetAllFiles(string containerName)
        {
            await InitializeContainer(containerName);

            List<SharedFile> files = new List<SharedFile>();
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            try
            {                
                var resultSegment = blobContainerClient.GetBlobsAsync();
                    await foreach (BlobItem blobItem in resultSegment)
                    {
                        SharedFile file = new SharedFile();
                        var block = blobContainerClient.GetBlockBlobClient(blobItem.Name);
                        file.FileName = blobItem.Name;
                        file.DownloadURL = block.Uri.ToString();
                        Console.WriteLine("Blob name: {0}", blobItem.Name);
                        files.Add(file);
                    }
                    Console.WriteLine();
                
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

        public async Task InitializeContainer(string containerName)
        {
            List<string> containerNames = new List<string>();
            try
            {
                var containers = blobServiceClient.GetBlobContainersAsync().AsPages();
                await foreach (Azure.Page<BlobContainerItem> containerPage in containers)
                {
                    foreach (BlobContainerItem containerItem in containerPage.Values)
                    {
                        containerNames.Add(containerItem.Name);
                    }

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex);
            }

            if (containerNames.Contains(containerName))
            {
                blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);
            }
            else
            {
                await blobServiceClient.CreateBlobContainerAsync(containerName);
                blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.Blob);
            }

            blobContainerClient = null;
        }


        public IActionResult Upload(string containerName)
        {
            ViewBag.ContainerName = containerName;
            return View();
        }


        //TODO: Archives
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile TempFile, string serverName)
        {
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                TempFile.CopyTo(stream);  
            }

            blobContainerClient = blobServiceClient.GetBlobContainerClient(serverName);
            string fileName = (TempFile.FileName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(filePath, true);
            blobContainerClient = null;
            
            return RedirectToAction("Index", new {serverName = serverName});
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost("DeleteFile")]
        public async Task<IActionResult> DeleteFile(string containerName, string fileName)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobToDelete = blobContainerClient.GetBlobClient(fileName);
            await blobToDelete.DeleteIfExistsAsync(); 

            
            return RedirectToAction("Index", new { serverName = containerName});
        }
    }
}
