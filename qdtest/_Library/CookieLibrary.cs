using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace qdtest._Library
{
    public class CookieLibrary
    {
        public static HttpCookie Base64Encode(HttpCookie input)
        {
            foreach (String key in input.Values.AllKeys)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input[key]);
                string encoded = System.Convert.ToBase64String(bytes);
                input[key] = encoded;
            }
            return input;
        }
        public static HttpCookie Base64Decode(HttpCookie input)
        {
            foreach (String key in input.Values.AllKeys)
            {
                byte[] bytes = System.Convert.FromBase64String(input[key]);
                string decoded = System.Text.Encoding.UTF8.GetString(bytes);
                input[key] = decoded;
            }
            return input;
        }
    }
}