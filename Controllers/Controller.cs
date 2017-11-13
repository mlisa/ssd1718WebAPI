using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using System.Web.Http;
using System.Configuration;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class OrdiniController : ApiController
    {

        [HttpGet]                // in esecuzione solo con un get dal client
        [ActionName("GetAllOrders")]   // nome del metodo esposto nella API
        public string GetAllOrders()
        {
            string connString;
            List<ordini> lstOrdini = null;

           
            return JsonConvert.SerializeObject(lstOrdini);
        }


        /*[HttpGet]     // in esecuzione solo con un get dal client
        [ActionName("GetCustOrders")]   // nome del metodo esposto
        public IHttpActionResult GetCustOrders(int id)
        {
            //var user = listOrdini.FirstOrDefault((u) => u.id == id
            );
            if (user == null)
                return NotFound();
            return Ok(user);
        }*/
    }
}