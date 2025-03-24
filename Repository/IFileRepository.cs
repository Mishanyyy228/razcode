using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace lab.Repository
{
    interface IFileRepository
    {

        public Task<BitmapImage> GetImageUser(Image Image_User1, Image Image_User);
        public Task<string> GetUser(string user);

        public Task<BitmapImage> PostAndGetImageUser(Image Image_User1, Image Image_User);

        
    }
}
