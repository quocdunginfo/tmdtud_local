using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace qdtest._Library
{
    public class TextLibrary
    {
        public static string GetSHA1HashData(string data)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile("whatthefuck", "SHA1");
        }
    }
}