using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc_app.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApiProject.Models;

namespace mvc_app.Controllers
{
    public class UserOrdersController : Controller
    {
        UserApi _api = new UserApi();
        static List<int> OrderedProducts = new List<int>();
      
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> OrderMainPageAsync()
        {
         
            List<Product> Products = new List<Product>();
            HttpClient client = _api.initial();
            HttpResponseMessage res = await client.GetAsync($"api/Products");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                Products = JsonConvert.DeserializeObject<List<Product>>(results);

            }
            return View("OrderMainPageAsync",Products);

        }

        public async Task<IActionResult> GetproductsFromCart(int ProductId)
        {
            OrderedProducts.Add(ProductId);

            return await OrderMainPageAsync();
          
         
        }
       
        public async Task<IActionResult> ShowOrdersAsync()
        {

            OrderInfo newOrder = new OrderInfo();

            newOrder.ProductIds = OrderedProducts;
            newOrder.Status = "Inprogressapp";
            HttpClient client = _api.initial();
            var PostData = client.PostAsJsonAsync<OrderInfo>("api/Orders/add", newOrder);
            PostData.Wait();


            List<Order> AllOrders = new List<Order>();
      
            HttpResponseMessage res = await client.GetAsync($"api/Orders");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                AllOrders= JsonConvert.DeserializeObject<List<Order>>(results);

            }
            return View("ShowOrdersAsync",AllOrders);
        }
        public async Task<IActionResult> GetOrderDetails(int OrderToShow)
        {
            List<Product> ResultList = new List<Product>();
            //OrderedProducts.Add(ProductId);
            int a = OrderToShow;
            HttpClient client = _api.initial();
            HttpResponseMessage res = await client.GetAsync($"api/Orders/{OrderToShow}");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
           
                OrderDetail x= JsonConvert.DeserializeObject<OrderDetail>(results);
                var test = x.order;
                List<Product> ProductsData = new List<Product>();
                ProductsData = x.productsToShow;
                ViewBag.Products = ProductsData;



            }
            return await ShowOrdersAsync();


        }



    }

    public class OrderDetail {
        public Order order { get; set; }
        public List<Product> productsToShow { get; set; } = new List<Product>();
    }
}