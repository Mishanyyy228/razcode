using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using taskLibrary;
using System.Net.Http.Json;
using TodoEntities;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace lab.model
{
    public class Repository1 : TodoHttpClient
    {
        private readonly HttpClient httpClient;
        private readonly string TodosUrl = "api/todos";

        public Repository1()
        {
            httpClient = GetHttpClient();
        }
        //2

        //1
        //авторизация
        
        //1
        //регистрация

        //3
        //получение картинки
       
        //3
        //mananev13@gmail.com
        //установление картинки
        
        //2
        //добавление задачи

    }
}
