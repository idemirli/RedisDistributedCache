using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisDistributedCache.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisDistributedCache.Controllers
{
    public class CustomersController : BaseController
    {
        public CustomersController(IDistributedCache distributedCache) : base(distributedCache)
        {
        }

        public IActionResult Index()
        {
            //SetKeyCache();

            //SetComplextTypeCache();

            return View();
        }


        public async Task<IActionResult> AsyncIndex()
        {
            await AsyncSetKeyCache();

            return View("Index");
        }



        public IActionResult Show()
        {
            string takim = _distributedcache.GetString("takim");
            string renk = _distributedcache.GetString("renk");

            //Json
            //string jsonCustomer = _distributedcache.GetString("customer:1");
            //Customer customer = jsonCustomer != null ? JsonConvert.DeserializeObject<Customer>(jsonCustomer) : new Customer();


            //Binary
            //Byte[] byteCustomer = _distributedcache.Get("customer:1");
            //string jsonCustomer = Encoding.UTF8.GetString(byteCustomer);
            //Customer customer= jsonCustomer != null ? JsonConvert.DeserializeObject<Customer>(jsonCustomer) : new Customer();

            if (!string.IsNullOrEmpty(takim))
            {
                ViewBag.takim = takim;
                ViewBag.renk = renk;
                //ViewBag.Customer = customer;

            }
            return View("Index");
        }

        public IActionResult Remove()
        {
            _distributedcache.Remove("username");
            _distributedcache.Remove("takim");
            _distributedcache.Remove("customer:1");
            return View("Index");
        }

        public IActionResult ImageCache()
        {
            //Önce wroot altında images klasörü oluşturup resim ekledim.
            SetImagePdfCache();

            return View();
        }

        public IActionResult ImageShow()
        {
            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedcache.Get("resim");
            return File(imageByte, "image/jpg");
        }


        private void SetKeyCache()
        {
            //Uygulamamın memory'sine kaydetmiyorum , Redis'i tamamen ayrı bir server olarak düşünün.
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();  //1 dakika cache'te duracak.
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            _distributedcache.SetString("username", "ibrahim", options);
            _distributedcache.SetString("takim", "Galatasaray", options);
            _distributedcache.SetString("renk", "sariKirmizi", options);
        }

        private async Task AsyncSetKeyCache()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            await _distributedcache.SetStringAsync("takim", "Galatasaray", options);
            await _distributedcache.SetStringAsync("renk", "kirmizi", options);
        }

        private void SetComplextTypeCache()
        {
            //iki yöntem var , Json Serialize işlemi ,  Binary serialize işlemi , Json Serialize daha basit .

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            Customer customer = new Customer { Id = 1, Name = "Yavuz", Age = 28, Surname = "Türk" };
            string jsonCustomer = JsonConvert.SerializeObject(customer);


            //Json
            //_distributedcache.SetString("customer:1", jsonCustomer, options);

            
            
            //Binary
            Byte[] byteCustomer = Encoding.UTF8.GetBytes(jsonCustomer);
            _distributedcache.Set("customer:1",byteCustomer,options);

        }

        private void SetImagePdfCache()
        {
            //İlk önce dosyamın yolunu alıyorum.
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/deneme.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedcache.Set("resim", imageByte);
        
        }

    }
}
