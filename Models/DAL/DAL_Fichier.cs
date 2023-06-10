using LMDServerAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LMDServerAPI.Utilities;
namespace LMDServerAPI.Models.DAL
{
    public class DAL_Fichier
    {
        public  static int add(Fichier fichier)
        {
            
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string req = "INSERT INTO Fichier (FileUrl,userId,Taille,Format) output INSERTED.ID values(@FileUrl,@userId,@Taille,@Format)";

                SqlCommand cmd= new SqlCommand(req, con);
                cmd.Parameters.AddWithValue("@FileUrl", fichier.FileUrl ?? (Object)DBNull.Value);
                cmd.Parameters.AddWithValue("@userId", 1000);

                cmd.Parameters.AddWithValue("@Taille", fichier.Taille ?? (Object)DBNull.Value);

                cmd.Parameters.AddWithValue("@Format", fichier.Format ?? (Object)DBNull.Value);
                return Convert.ToInt32(DataBaseAccessUtilities.ScalarRequest(cmd));
            }
        }
        public static Fichier getEntityFromDataRow(DataRow dataRow)
        {
            Fichier fichier = new Fichier();
            fichier.Id = Convert.ToInt32(dataRow["ID"]);
            fichier.FileUrl = Convert.ToString(dataRow["FileUrl"]);
            fichier.Taille= Convert.ToString(dataRow["Taille"]);
            fichier.Format= Convert.ToString(dataRow["Format"]);
            return fichier;
        }
        public static List<Fichier> getListFromDataTable(DataTable dataTable)
        {
            List<Fichier> fichiers = new List<Fichier>();
            if (dataTable != null)
            {
                foreach(DataRow dr in dataTable.Rows)
                {
                    fichiers.Add(getEntityFromDataRow(dr));
                }
            }
            
            return fichiers;
        }
        public static List<Fichier> getAll()
        {
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string req="SELECT * from Fichier";
                SqlCommand cmd = new SqlCommand(req, con);
                DataTable dt;
                dt=DataBaseAccessUtilities.SelectRequest(cmd);
                return getListFromDataTable(dt);
            }

            
        }
        public static Fichier GetFichier(int id)
        {
            
            using (SqlConnection con = DBConnection.GetConnection())
            {
                string req = "SELECT * from Fichier where ID=@EntityKey";
                SqlCommand cmd = new SqlCommand(req, con);
                cmd.Parameters.AddWithValue("@EntityKey", id);
                DataTable dt = DataBaseAccessUtilities.SelectRequest(cmd);
                if(dt!=null&& dt.Rows.Count != 0)
                {
                    return getEntityFromDataRow(dt.Rows[0]);
                }
                else
                {
                    return null;
                }
            }
            
        }
        public static void DeleteFichier(int id)
        {
            using(SqlConnection con = DBConnection.GetConnection())
            {
                string req = "DELETE from Fichier where ID=@EntityKey";
                SqlCommand cmd = new SqlCommand(req, con);
                cmd.Parameters.AddWithValue("@EntityKey", id);
                DataBaseAccessUtilities.NonQueryRequest(cmd);
            }
        }
        public static void UpdateFichier(int id, Fichier fichier)
        {
            using(SqlConnection con = DBConnection.GetConnection())
            {
                string req = "UPDATE Fichier set FileUrl=@FileUrl,userId=@userId,Taille=@Taille,Format=@Format where Id=@EntityKey";
                SqlCommand cmd = new SqlCommand(req, con);
                cmd.Parameters.AddWithValue("@EntityKey", id);
                cmd.Parameters.AddWithValue("@FileUrl", fichier.FileUrl ?? (Object)DBNull.Value);
                cmd.Parameters.AddWithValue("@userId", 1000);
                cmd.Parameters.AddWithValue("@Taille", fichier.Taille ?? (Object)DBNull.Value);

                cmd.Parameters.AddWithValue("@Format", fichier.Format ?? (Object)DBNull.Value);
                DataBaseAccessUtilities.NonQueryRequest(cmd);
            }
        }
    }
}
