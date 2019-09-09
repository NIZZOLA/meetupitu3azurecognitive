using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImgCognitiveWeb.Utils
{
    public static class ExtensionMethods
    {
        public static bool IsImage( this IFormFile file )
        {
            string[] extensions = new string[] { ".JPG", ".GIF", ".BMP", ".JPEG", ".PNG", ".TIF" };
            foreach (var item in extensions)
            {
                if (file.FileName.ToUpper().Contains(item))
                    return true;
            }
            return false;
        }
    }
}
