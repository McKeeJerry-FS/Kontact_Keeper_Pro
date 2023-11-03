namespace Kontact_Keeper_Pro.Services.Interfaces
{
    public interface IImageService
    {
        public Task<byte[]> ConvertFileToByteArrayAsynC(IFormFile? file);
        public string? ConvertByteArrayToFile(byte[]? FileData, string? extension);
    }
}
