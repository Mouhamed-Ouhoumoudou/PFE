 using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LMDServerAPI.Models.BLLn;
using LMDServerAPI.Models.Entities;
using LMDServerAPI.NAS;
using System.IO;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FichierController : Controller
    {
        private string fullPath = "Tmp";
        // GET: api/<FichierController>
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                List<Fichier> fichiers = BLL_Fichier.getAll();

                return Json(new { success = true, message = "fichiers trouver", data = fichiers });
            }
            catch (Exception e)
            {

                return Json(new { success = false, message = e.Message });
            }

        }

        // GET api/<FichierController>/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {

            try
            {
                Fichier fichier = BLL_Fichier.GetFichier(id);
                string fileName = Path.GetFileName(fichier.FileUrl);
                string extension = Path.GetExtension(fileName);
                //supression de point
                extension = extension.Substring(1);

                string contentType = "";
                if (extension == "png" || extension == "jpg" || extension == "jpeg")
                {
                    //exemple image/png
                    contentType = "image/" + extension;
                }
                else
                {
                    //exemple application/pdf
                    contentType = "application/" + extension;
                }
                /* recuperation des octets du fichier telechargé */
              // byte[] info = NAS_Operation.DisplayFileFromServer(fileName);
              byte [] info=NAS_Operation.DownloadWithSFTP(fileName);
                string Base64String = "";
                if (info != null) //si le fichier n'est pas vide ,autrement si le fichier existe
                                  //on convertis ces octets en Base64  pour  qu'ils soint compris par le JavaScript lors de la visuaisation
                {
                    Base64String = Convert.ToBase64String(info, 0, info.Length);
                }
                
                /* pour faciter l'exploit on a crééun une classe Result qui contient url de fichier et son type de contenue dans deux attributs separés
                 cela va nous servir coté JavaScript lors de la lecture ou visualisation du fichier */
                Result result = new Result();
                result.url = "data:" + contentType + ";base64," + Base64String;
                result.contentType = contentType;

                return Json(new { success = true, message = "Fichier trouvé", data = result  });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }

        }

        // POST api/<FichierController>
        [HttpPost]
        public void Post([FromForm] Fichier fichier)
        {
            var formFile = fichier.formFile;
            /* si le formulaire contient un fichier et que ce fichier n'est pas vide */
            if (formFile.Length > 0)
            {
                /* on copie alors ce fichier de formulaire dans le repertoire temporaire dont le chemin est dans la variable "fullpath" 
                 qui est declarée en haut dans ce controller
                
                 ce stockage temporaire va servir les Differentes methodes  qui ont lien direct avec NAS
                ces methodes se trouvent dans  la classe Nas_Operation qui est dans le dossier NAS*/
                var filePath = Path.Combine(fullPath, formFile.FileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    formFile.CopyTo(stream);
                }
                /* si c'est un ajout d'un nouveau fichier */
                if (fichier.Id == 0)
                {
                    /*on appelle la methode qui envoi le fichier vers le NAS */
                   // NAS_Operation.UploadFileToServer(formFile);
                   //si le meme utilisateur a l'acces avec SFTP on peut faire
                   NAS_Operation.uploadWithSFTP(formFile);

                    /* ensuite on recupere le nom du fichier puis l'inserer dans la base de donnée */
                    fichier.FileUrl = "https://localhost:44348/" + filePath; //on peut aussi faire fichier.FileUrl=formeFile.FileName; 
                    float  taille = formFile.Length / 1024;
                  
                    fichier.Taille = taille + " Ko";
                    fichier.Format = Path.GetExtension(filePath);
                    BLL_Fichier.add(fichier);
                }
                else
                {
                    // si c'est la modification  on recupere le nom l'ancien fichier depuis BD
                    // puis on envois son nom et le nouveau fichier à la methode qui fait modication coté NAS ensuite on modifie le nom coté BD
                    string oldFile =Path.GetFileName(BLL_Fichier.GetFichier(fichier.Id).FileUrl);
                    fichier.FileUrl = "https://localhost:44348/" + filePath;
                    NAS_Operation.updateFileInServer(oldFile, formFile);
                    BLL_Fichier.UpdateFichier(fichier.Id, fichier);
                }
                // apres le  Traitement ci-dessus que ce soit ajout ou modifiaction , on supprime le fichier dans le dossier Tmp
                System.IO.File.Delete("Tmp/" + formFile.FileName);
            }
        }

        // PUT api/<FichierController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FichierController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            /* on recupere le nom du fichier à supprimer dans la BD ,on le supprime coté NAS puis coté BD.
              si le fichier n'existe pas coté NAS et qu'on a envoyé le nom une exception sera declanchée */
            Fichier fichier = BLL_Fichier.GetFichier(id);
            string fileName = Path.GetFileName(fichier.FileUrl);
            // NAS_Operation.DeleteFileInServer(fileName);
            NAS_Operation.DeleteWithSFTP(fileName);
            BLL_Fichier.DeleteFichier(id);
        }
        
    }
}
