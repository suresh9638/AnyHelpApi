using anyhelp.Data.DataContext;


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace anyhelp.Service.Helper
{
    public static class FileHelperService
    {
        public static string GetReportImagePath(long UserID)
        {
            string folderName;

           
            folderName = Path.Combine("Files", "Post", UserID.ToString(), "Image");
            
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            return pathToSave;
        }
     



    }
}
