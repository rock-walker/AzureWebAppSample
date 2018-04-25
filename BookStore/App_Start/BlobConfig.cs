using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BookStore
{
    public class BlobConfig
    {
        private static CloudBlobClient _blobClient;
        private const string BlobContainerName = "start-app-logs";
        private const string FilePrefix = "HelloWorld";
        private static CloudBlobContainer _blobContainer;

        public static async Task LogStartupEvent()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"].ToString());

            _blobClient = storageAccount.CreateCloudBlobClient();
            _blobContainer = _blobClient.GetContainerReference(BlobContainerName);
            await _blobContainer.CreateIfNotExistsAsync();

            await _blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            var fileName = FilePrefix + "_" + DateTime.Now.Ticks;
            var bytes = GenerateLogFile(fileName);

            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(fileName);
            await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
        }

        private static byte[] GenerateLogFile(string fileName)
        {
                var now = DateTime.Now;
            var serverName = System.Environment.MachineName;

                var fileData = now + Environment.NewLine + serverName;
                byte[] bytes = Encoding.UTF8.GetBytes(fileData);
            return bytes;
        }
    }
}