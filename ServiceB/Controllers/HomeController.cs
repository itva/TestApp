using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using IDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceB.Components.Helpers;
using ServiceB.Controllers.Helpers;
using ServiceB.Models;

namespace ServiceB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestDbContext _dbContext;
        private readonly ITestFileContext _fileContext;

        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _environment;

        private readonly string _username = "qqq1#2@www.eee";
        private readonly string _password = "12345@@%#";

        public HomeController(ITestDbContext dbContext, ITestFileContext fileContext, IConfiguration config, IHostingEnvironment environment)
        {
            _dbContext = dbContext;
            _fileContext = fileContext;

            _config = config;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {

            var rslt = await GetData();

            return View(new IndexViewModel(){Filter = null, Data = rslt.ToList()});
        }

        private async Task<IEnumerable<TreeRow>> GetData(string filter = null)
        {
            var dataDb = await _dbContext.GetSubdivisionsAsync();

            var dataA = new Dictionary<long, bool>();

            #region ServiceA

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config.GetValue<string>("Customs:ServiceAAddr"));

                //запрос токена
                var content =
                    JsonConvert.SerializeObject(new Cred() { username = _username, password = _password });
                var token = await DoPost<string>(client,
                    $"{_config.GetValue<string>("Customs:ServiceAAddr")}/api/account/token",
                    content,
                    new List<MediaTypeWithQualityHeaderValue>()
                    {
                        new MediaTypeWithQualityHeaderValue("application/json")
                    },
                    null);

                //забрать данные от сервиса А
                dataA = await DoGet<Dictionary<long, bool>>(client,
                    $"{_config.GetValue<string>("Customs:ServiceAAddr")}/api/status/getdata",
                    new List<MediaTypeWithQualityHeaderValue>()
                    {
                        new MediaTypeWithQualityHeaderValue("application/json")
                    },
                    new List<(string key, string value)>() { ("Authorization", "Bearer " + token) }
                );

                //если у сервиса А нет данных - возможно, он просто не получил исходную информацию. перезаполним.
                if (dataA == null || !dataA.Any())
                {
                    dataA = await DoPost<Dictionary<long, bool>>(client,
                        $"{_config.GetValue<string>("Customs:ServiceAAddr")}/api/status/UploadNewData",
                        JsonConvert.SerializeObject(dataDb.Select(rec => rec.Id).ToList()),
                        new List<MediaTypeWithQualityHeaderValue>()
                        {
                            new MediaTypeWithQualityHeaderValue("application/json")
                        },
                        new List<(string key, string value)>() { ("Authorization", "Bearer " + token) }
                    );
                }
            }

            #endregion

            var rslt = from recDb in dataDb
                       from recA in dataA
                       where recDb.Id == recA.Key
                       select new TreeRow()
                       {
                           Id = recDb.Id,
                           ParentId = recDb.ParentId,
                           Text = $"{recDb.Name} ({(recA.Value ? "Активно" : "Заблокировано")})",
                           Value = recDb.Name
                       };
            return rslt;
        }

        private async Task<T> DoPost<T>(HttpClient client, string path, string content,
            IEnumerable<MediaTypeWithQualityHeaderValue> acceptHeaders, IEnumerable<(string key, string value)> headers)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            if (acceptHeaders != null)
            {
                foreach (var row in acceptHeaders)
                {
                    client.DefaultRequestHeaders.Accept.Add(row);
                }
            }

            client.DefaultRequestHeaders.Clear();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.key, header.value);
                }
            }

            HttpResponseMessage response =
                await client.PostAsync(path, new StringContent(content, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(readTask.GetAwaiter().GetResult());
                return result;
            }
            else
            {
                throw new Exception($"POST {path} Статус: {response.StatusCode}");
            }
        }

        private async Task<T> DoGet<T>(HttpClient client, string path, 
            IEnumerable<MediaTypeWithQualityHeaderValue> acceptHeaders, IEnumerable<(string key, string value)> headers)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            if (acceptHeaders != null)
            {
                foreach (var row in acceptHeaders)
                {
                    client.DefaultRequestHeaders.Accept.Add(row);
                }
            }

            client.DefaultRequestHeaders.Clear();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.key, header.value);
                }
            }

            HttpResponseMessage response =
                await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(readTask.GetAwaiter().GetResult());
                return result;
            }
            else
            {
                throw new Exception($"POST {path} Статус: {response.StatusCode}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Filter(string filterVal)
        {
            var rslt = await GetData(filterVal);

            return View("Index",new IndexViewModel() { Filter = filterVal, Data = rslt.ToList() });
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadFile)
        {
            //загрузить файл на диск и в базу
            if (uploadFile != null)
            {
                var fileName = $"{_environment.WebRootPath}/Files/{DateTime.Now.Ticks}_{Path.GetRandomFileName()}";
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fileStream);
                }
                
                var processedFile=await _fileContext.UploadSubdivisionsFromFileAsync(fileName);

                if (processedFile == null)
                {
                    return View("Alarm",
                        $"Не удалось прочитать файл {WebUtility.HtmlEncode(uploadFile.Name)}. Возможно, файл имеет некорректную структуру.");
                }

                var uploadResult = await _dbContext.SaveSubDivisionsInDbAsync(processedFile.ToList());
                switch (uploadResult)
                {
                    case 0:break;
                    case -1:
                        return View("Alarm",
                            $"Файл {WebUtility.HtmlEncode(uploadFile.Name)} содержит записи с неуникальными идентификаторами");
                    case -2:
                        return View("Alarm",
                            $"Файл {WebUtility.HtmlEncode(uploadFile.Name)} содержит записи, ссылающиеся на несуществующую родительскую запись");
                    case -3:
                        return View("Alarm",
                            $"Файл {WebUtility.HtmlEncode(uploadFile.Name)} содержит записи, ссылающиеся сами на себя");
                    default:
                        return View("Alarm",
                            $"Непредвиденная ошибка при загрузке файла {WebUtility.HtmlEncode(uploadFile.Name)}");
                }
            }
            //очистить данные на ServiceA
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_config.GetValue<string>("Customs:ServiceAAddr"));

                //запрос токена
                var content =
                    JsonConvert.SerializeObject(new Cred() { username = _username, password = _password });
                var token = await DoPost<string>(client,
                    $"{_config.GetValue<string>("Customs:ServiceAAddr")}/api/account/token",
                    content,
                    new List<MediaTypeWithQualityHeaderValue>()
                    {
                        new MediaTypeWithQualityHeaderValue("application/json")
                    },
                    null);

                    var clear = await DoPost<Dictionary<long, bool>>(client,
                        $"{_config.GetValue<string>("Customs:ServiceAAddr")}/api/status/UploadNewData",
                        JsonConvert.SerializeObject(new List<long>()),
                        new List<MediaTypeWithQualityHeaderValue>()
                        {
                            new MediaTypeWithQualityHeaderValue("application/json")
                        },
                        new List<(string key, string value)>() { ("Authorization", "Bearer " + token) }
                    );
                
            }

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}