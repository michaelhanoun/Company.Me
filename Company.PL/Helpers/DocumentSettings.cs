using System.Threading.Tasks;

namespace Company.PL.Helpers
{
    public static class DocumentSettings
    {
        public static async Task<string> Upload (IFormFile file,string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files",folderName);
            if(!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var fileName = $"{Guid.NewGuid()}{file.FileName}";
            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath,FileMode.Create);
            await file.CopyToAsync(stream);
            return fileName;
        }
        public static void Delete(string fileName,string folderName) 
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\files", folderName,fileName);
            if(File.Exists(filePath))
                File.Delete(filePath);

        }
    }
}
