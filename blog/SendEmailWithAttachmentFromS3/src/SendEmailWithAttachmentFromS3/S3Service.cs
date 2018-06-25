using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;

namespace SendEmailWithAttachmentFromS3
{
    public interface IS3Service
    {
        Task<string> DownloadFileAsync(string bucketName, string key);
    }

    public class S3Service : IS3Service
    {
        private readonly ILambdaContext context;
        private readonly IAmazonS3 s3Client;

        public S3Service(ILambdaContext context,IAmazonS3 s3Client)
        {
            this.context = context;
            this.s3Client = s3Client;
        }

        public async Task<string> DownloadFileAsync(string bucketName, string key)
        {
            using (s3Client)
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                using (var response = await s3Client.GetObjectAsync(request))
                {
                    var fileData = GetStreamData(response.ResponseStream);
                    var path = $"/tmp/{key}";
                    File.WriteAllBytes(path, fileData);
                    return path;
                }
            }
        }

        private byte[] GetStreamData(Stream sourceStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                sourceStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
