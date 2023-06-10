using Microsoft.AspNetCore.Http;
namespace LMDServerAPI.Models.Entities
{
    public class Fichier
    {
        public int Id { get; set; }
        public string FileUrl { get; set; }
        public IFormFile formFile { get; set; }
        public string Taille { get; set; }
        public string Format { get; set; }
        public Fichier()
        {

        }
        public Fichier(int Id, string FilUrl, IFormFile formFile)
        {
            this.Id = Id;
            this.FileUrl = FileUrl;
            this.formFile = formFile;
        }
    }
}
