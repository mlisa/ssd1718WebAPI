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
    public class ClientiController : ApiController
    {

        [HttpGet]                // in esecuzione solo con un get dal client
        [ActionName("GetAllClients")]   // nome del metodo esposto nella API
        public string GetAllClients()
        {
            string connString, queryText, factory;
            List<magazzini> lstClients = null;
            Model m = new Model();
            connString = ConfigurationManager.ConnectionStrings["SQLiteConn"].ConnectionString;
            queryText = "SELECT id, req, mag FROM clienti";
            factory = "System.Data.SQLite";
            string s = m.readTableViaF(connString, queryText, factory);
            return s;
            //return JsonConvert.SerializeObject(lstClients);
        }

        [HttpGet]                // in esecuzione solo con un get dal client
        [ActionName("GetClient")]   // nome del metodo esposto nella API
        public string GetClient(int id)
        {
            string connString, queryText, factory;
            List<magazzini> lstClients = null;
            Model m = new Model();
            connString = ConfigurationManager.ConnectionStrings["SQLiteConn"].ConnectionString;
            queryText = "SELECT id, req, mag FROM clienti WHERE id = " + id;
            factory = "System.Data.SQLite";
            string s = m.readTableViaF(connString, queryText, factory);
            return s;
            //return JsonConvert.SerializeObject(lstClients);
        }

        [HttpPost]
        [ActionName("insertCustomer")]
        public string insertCustomer(clienti obj)
        {
            string connString = ConfigurationManager.ConnectionStrings["SQLiteConn"].ConnectionString;
            string factory = "System.Data.SQLite";
            string queryString = "insert into clienti (id, req, mag) values(";
            queryString += obj.id + "," + obj.req + ",'" + obj.mag +"')";
            Model m = new Model();
            m.execNonQueryViaF(connString, queryString, factory, true);
            return "Customer inserted";   // oppure dichiararla static
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