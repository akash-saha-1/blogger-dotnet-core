using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Web.Repository
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;

        public CloudinaryImageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]
            );
        }
        async Task<string?> IImageRepository.UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(account);

            // Upload
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName,
                UseFilename = true,
                UniqueFilename = false,
                Folder = configuration.GetSection("Cloudinary")["folderName"]
            };

            var uploadResult = await client.UploadAsync(uploadParams);

            if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult?.SecureUrl?.ToString();
            }

            return null;
        }
    }
}
