using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlob.Api.Models;

namespace AzureBlob.Api.Logics
{
    public class FileManagerLogic : IFileManagerLogic
    {
        private readonly BlobServiceClient _blobServiceClient;

        private readonly string sAzureId = "fbb34aed-4fee-4cdd-a9db-5ab1c1078f9c";
        private readonly string sAzureName = "RG_Storage_MCH";
        public FileManagerLogic(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task Upload(FileModel model)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("containermch01");

            var blobClient = blobContainer.GetBlobClient(model.ImageFile.FileName);

            await blobClient.UploadAsync(model.ImageFile.OpenReadStream());
        }

        public async Task<byte[]> Get(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("containermch01");

            var blobClient = blobContainer.GetBlobClient(imageName);
            var downloadContent = await blobClient.DownloadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        public async Task<List<FileInfos>> GetList() { 
            var blobContainer = _blobServiceClient.GetBlobContainerClient("containermch01");

            List<FileInfos> list = new List<FileInfos>();
            await foreach (BlobItem blobItem in blobContainer.GetBlobsAsync())
            {
                FileInfos temp = new FileInfos();
                temp.Filename = blobItem.Name;
                temp.AccoutName = _blobServiceClient.AccountName;
                temp.Created = (DateTimeOffset)blobItem.Properties.CreatedOn;
                temp.ContainerName = blobContainer.Name;
                temp.AzureId = sAzureId;
                temp.AzureName = sAzureName;

                list.Add(temp);
            }
            return list;
        }

        public async Task Delete(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("containermch01");

            var blobClient = blobContainer.GetBlobClient(imageName);

            await blobClient.DeleteAsync();
        }
    }
}