using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Internal;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace VerifyingDataIntegrityWithAWSS3
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;

        public S3Service()
        {
            var cred = new BasicAWSCredentials("{ACCESS KEY ID}", "{SECRET KEY}");
            _s3Client = new AmazonS3Client(cred, RegionEndpoint.GetBySystemName("us-east-1"));
        }

        public async Task DownloadFileAsync()
        {
            var outputPath = @"./leaves.jpg";
            var request = new GetObjectRequest
            {
                BucketName = "{BUCKET NAME}",
                Key = "leaves.jpg",
                EtagToMatch = "\"AABBCCDDF7516B4CA5D7D291DB04FB20\"".ToLower() // Case-sensitive. Replace it with correct hash to test success case
            };

            try
            {
                using (var response = await _s3Client.GetObjectAsync(request))
                {
                    if (File.Exists(outputPath))
                    {
                        File.Delete(outputPath);
                    }

                    await response.WriteResponseStreamToFileAsync(outputPath, false, CancellationToken.None);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UploadFileAsync()
        {
            var inputPath = @"./leaves.jpg";
            var bucketName = "{BUCKET NAME}";
            var key = "leaves.jpg";

            var md5 = ComputeHashOfFile(inputPath);

            var request = new PutObjectRequest
            {
                MD5Digest = md5,
                BucketName = bucketName,
                Key =  key,
                FilePath = inputPath,
            };

            try
            {
                var response = await _s3Client.PutObjectAsync(request);
                Console.Write(response.ETag);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private static string ComputeHashOfFile(string fullPath)
        {
            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(stream);
                    return Convert.ToBase64String(hash);
                }
            }
        }
    }
}
