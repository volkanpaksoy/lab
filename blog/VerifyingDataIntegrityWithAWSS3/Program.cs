using System;
using System.Threading.Tasks;

namespace VerifyingDataIntegrityWithAWSS3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var s3Service = new S3Service();
            await s3Service.DownloadFileAsync();
            // await s3Service.UploadFileAsync();
        }
    }
}
