﻿using System;
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
            //chu HOA
            return FormsAuthentication.HashPasswordForStoringInConfigFile(data, "SHA1");
        }
        public static int ToInt(String input)
        {
            int re=0;
            if (Int32.TryParse(input, out re))
            {
                return re;
            }
            return 0;
        }
    }
}