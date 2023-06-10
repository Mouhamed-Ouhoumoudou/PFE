﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;
namespace LMDServerAPI.NAS
{
    public class NAS_Access
    {
        IPAddress _address;
        int _port;
        public NAS_Access()
        {

        }
        public NAS_Access(IPAddress iPAddress, int port)
        {
            this._address = iPAddress;
            this._port = port;
        }
        public static string getSharedFolder()
        {
            return "\\\\172.16.1.20\\NAS\\site1\\";
        }
        public static string getUsername()
        {
            return "Mouhamed";
           
        }
        public static string getPasseword()
        {
           
          return   "123456";
            
        }
        public static int getPort()
        { 


            return 21; 
        }
        public static int getSecurePort()
        {
            return 22;
        }
        public static string getDomaine()
        {
            return "ftp://serveurDeLasociete.com:22/TEST/";
        
        }
        public static string getSecureDomaine()
        {
            return "IPdeServeur";//vrai ip qui est accessible sur internet
      
        }
        public static string getBackupFolder()
        {
       
            return "ftp://172.16.1.20/TEST/Backup/";
        }
    }
}
