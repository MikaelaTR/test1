﻿using AdvancedProjectMVC.Models;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //containerName = "filesharecontainer";
            List<string> containerNames = new List<string>();
            try
            {
                var containers = blobServiceClient.GetBlobContainersAsync().AsPages();
                await foreach(Azure.Page<BlobContainerItem> containerPage in containers)
                {
                    foreach (BlobContainerItem containerItem in containerPage.Values)
                    {
                        containerNames.Add(containerItem.Name);
                    }

                    Console.WriteLine();
                }
            }
             catch (Exception ex) { 
                Console.WriteLine("Exception " + ex);
            }
            List<SharedFile> files = new List<SharedFile>();
            if (containerNames.Contains(containerName)) 
            {                
                blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
                try
                {
                    var resultSegment = blobContainerClient.GetBlobsAsync().AsPages();

                    await foreach (Page<BlobItem> blobPage in resultSegment)
                    {
                        foreach (BlobItem blobItem in blobPage.Values)
                        {
                            SharedFile file = new SharedFile();
                            file.FileName = blobItem.Name;

                            Console.WriteLine("Blob name: {0}", blobItem.Name);
                            files.Add(file);
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
            else
            {
                await blobServiceClient.CreateBlobContainerAsync(containerName);
                return files;
            }
        }

        public IActionResult Upload(string containerName)
        {
            ViewBag.ContainerName = containerName;
            return View();
        }


        //TODO: Archives
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile TempFile, string containerName, string serverName)
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

        public async Task<IActionResult> DownloadFile(string containerName, string fileName)
        {
            containerName = "filesharecontainer";
            
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

            try
            {
                BlobDownloadResult blobDownloadResult = await blobClient.DownloadContentAsync();
                var downloadData = blobDownloadResult.Content.ToStream();

                BlobObject blobObject = new BlobObject { BlobContent = downloadData, BlobName = fileName };

                using (var resultStream = System.IO.File.OpenWrite(downloadPath + "\\" + fileName))
                {
                    blobObject.BlobContent.CopyTo(resultStream);
                }

                blobContainerClient = null;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                blobContainerClient= null;
            } 

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete()
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
