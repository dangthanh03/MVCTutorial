using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RunGroopWebApp.Helper;
using RunGroopWebApp.InterfaceRepository;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RunGroopWebApp.Service
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _config;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
          (
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
          );
            _config = new Cloudinary(acc);


        
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
           
                var UploadResult = new ImageUploadResult();
                if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();


                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    UploadResult = await _config.UploadAsync(uploadParams);
                }
                return UploadResult;
            
        
            
        }

        public async Task<bool> CheckCloudinaryConfiguration(string cloudName, string apiKey, string apiSecret)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"https://api.cloudinary.com/v1_1/{cloudName}/usage";
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiKey}:{apiSecret}"));

                var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _config.DestroyAsync(deleteParams);
            return result;
        }
    }
}
