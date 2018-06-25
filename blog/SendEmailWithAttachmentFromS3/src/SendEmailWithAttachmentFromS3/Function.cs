using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon;
using System.IO;
using MimeKit;
using Newtonsoft.Json.Linq;
using Amazon.S3;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SendEmailWithAttachmentFromS3
{
    public class Function
    {
        public void FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
        {
            // Get environment variables
            var bucketName = System.Environment.GetEnvironmentVariable("CV_BUCKET_NAME");
            if (string.IsNullOrEmpty(bucketName)) throw new Exception("Invalid configuration: CV_BUCKET_NAME environment variable is missing");
            var cvName = System.Environment.GetEnvironmentVariable("CV_FILE_NAME");
            if (string.IsNullOrEmpty(cvName)) throw new Exception("Invalid configuration: CV_FILE_NAME environment variable is missing");

            // Extract the sender email address from raw SES message
            var snsMessageRaw = snsEvent.Records[0].Sns.Message;
            var sender = JObject.Parse(snsMessageRaw)["mail"]["source"].Value<string>();
            if (string.IsNullOrEmpty(sender)) throw new Exception("Could not find sender email address");

            // Download the latest PDF
            var s3Client = new AmazonS3Client();
            var s3Service = new S3Service(context, s3Client);
            var path = s3Service.DownloadFileAsync(bucketName, cvName).Result;
            if (string.IsNullOrEmpty(path)) throw new Exception("Could not download PDF from S3");

            // Send the response email with the CV attached
            var emailService = new RawEmailService(context);
            emailService.SendEmailAsync(sender, path).Wait();

            context.Logger.LogLine($"CV has been sent to {sender} successfully.");
        }
    }
}
