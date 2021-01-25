using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace web_encurtador_ale
{
    

    public class ShortLink
    {
        public string GetUrlChunk()
        {
            // Transform the "Id" property on this object into a short piece of text
            return WebEncoders.Base64UrlEncode(BitConverter.GetBytes(Id));
        }

        public int GetId(string urlChunk)
        {
            // Reverse our short url text back into an interger Id
            return BitConverter.ToInt32(WebEncoders.Base64UrlDecode(urlChunk));
        }

        public int Id { get; set; }

        public string Url { get; set; }
    }

}
