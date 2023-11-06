using Kontact_Keeper_Pro.Services.Interfaces;

namespace Kontact_Keeper_Pro.Services
{
    public class ImageService : IImageService
    {
        private readonly string _defaultImage = "/img/silo_img.jpg";

        #region ConvertByteArrayToFile
        public string? ConvertByteArrayToFile(byte[]? fileData, string? extension)
        {
            try
            {
                if (fileData == null)
                {
                    // show default
                    return _defaultImage;
                }
                string? imageBase64Data = Convert.ToBase64String(fileData);
                imageBase64Data = string.Format($"data:{extension};base64, {imageBase64Data}");
                return imageBase64Data;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region ConvertFileToByteArrayAsynC
        public async Task<byte[]> ConvertFileToByteArrayAsynC(IFormFile? file)
        {
            try
            {
                if (file != null)
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);
                    byte[] byteFile = memoryStream.ToArray();
                    memoryStream.Close();

                    return byteFile;
                }
                return null!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion    
    }
}
