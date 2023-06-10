using LMDServerAPI.Models.Entities;
using LMDServerAPI.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace LMDServerAPI.Models.BLLn
{
    public class BLL_Fichier
    {
        public static int add(Fichier fichier)
        {
            
            return DAL_Fichier.add(fichier);
        }
        public static List<Fichier> getAll()
        {
            return DAL_Fichier.getAll();
        }
        public static Fichier GetFichier(int id)
        {
            return DAL_Fichier.GetFichier(id);
        }
        public static void DeleteFichier(int id)
        {
            DAL_Fichier.DeleteFichier(id);
        }
        public static void UpdateFichier(int id, Fichier fichier)
        {
            DAL_Fichier.UpdateFichier(id, fichier);
        }
    }
}
