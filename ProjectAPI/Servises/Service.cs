using Azure.Storage.Blobs;
using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectAPI.DTO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//using static System.Net.Mime.MediaTypeNames;

namespace Core.Servises
{
    public class Service
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork<Test> testUnitOfWork;
        private readonly IUnitOfWork<Frameworks> frameworksUnitOfWork;
        private readonly IUnitOfWork<MainTrack> mainTrackUnitOfWork;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting;

        public Service(IConfiguration configuration,IUnitOfWork<Test> TestUnitOfWork, IUnitOfWork<Frameworks> FrameworksUnitOfWork, IUnitOfWork<MainTrack> mainTrackUnitOfWork , Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting)
        {
            this.configuration = configuration;
            testUnitOfWork = TestUnitOfWork;
            frameworksUnitOfWork = FrameworksUnitOfWork;
            this.mainTrackUnitOfWork = mainTrackUnitOfWork;
            this.hosting = hosting;
        }


        public async Task<List<object>> RendomQuestions(string framework)
        {
            var random = new Random();
            var Questions = await testUnitOfWork.Entity.FindAll(x=>x.FrameworkId == framework,x => x.Q_Id);

            if (Questions.Count() == 0)
                return null;

            var count = Questions.Count();
            var list = new List<object>();
            var list1 = new List<string>();
            for (var i = 0; i < count && i < 10; i++)
            {
                var index = random.Next(0, Questions.Count());
                var QuestionIndex = Questions[index];
                if (list1.Count() == 0 || !list1.Contains(QuestionIndex))
                {
                    list1.Add(QuestionIndex);
                    var question = await testUnitOfWork.Entity.GetAsync(QuestionIndex);
                    list.Add(question);
                }
                else
                {
                    --i;
                }

            }
            return list;

        }
        public async Task<string> CompressAndSaveImageAsync(IFormFile file , string directory , int width = 800, int quality = 50)
        {
            //string uploads = Path.Combine(hosting.WebRootPath, $@"{directory}");
            //string filePath = Path.Combine(uploads, file.FileName);
            //using (var image = await Image.LoadAsync(file.OpenReadStream()))
            //{
            //    // ضغط الصورة
            //    image.Mutate(x => x.Resize(new ResizeOptions
            //    {
            //        Mode = ResizeMode.Max,
            //        Size = new Size(width, 0) 
            //    }));

            //    using var outputStream = new FileStream(filePath, FileMode.Create);
            //    await image.SaveAsync(outputStream, new JpegEncoder { Quality = quality });
            //}

            //return file.FileName;

            if (file == null || file.Length == 0)
                return null;

            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            var url = blobClient.Uri.ToString();

            return Path.GetFileName(url);


        }


        public async Task<bool> ComparerAnswer(string questionId, string selectedAnwser)
        {
            var question = await testUnitOfWork.Entity.GetAsync(questionId);

            if (question.CorrectAnswers != selectedAnwser)
                return false;
            return true;


        }

        public string DetermineLevel(int count)
        {
            if (count <= 4)
                return "beginner";
            else if (count > 4 && count <= 7)
                return "Intermediate";
            else
                return " Advanced";



        }


        //public async Task<string> HashingPassword(string password)
        //{

        //}
       

    }
}
