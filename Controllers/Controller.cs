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
        Model m = new Model();
        string connString = ConfigurationManager.ConnectionStrings["SQLiteConn"].ConnectionString;
        string factory = "System.Data.SQLite";


        [HttpGet]                // in esecuzione solo con un get dal client
        [ActionName("GetAllClients")]   // nome del metodo esposto nella API
        public string GetAllClients()
        {
            string queryText;
            queryText = "SELECT id, req, mag FROM clienti";
            string s = m.readTableViaF(connString, queryText, factory);
            return s;
            //return JsonConvert.SerializeObject(lstClients);
        }

        [HttpGet]                // in esecuzione solo con un get dal client
        [ActionName("GetClient")]   // nome del metodo esposto nella API
        public string GetClient(int id)
        {
            string queryText;
            
            queryText = "SELECT id, req, mag FROM clienti WHERE id = " + id;
            
            string s = m.readTableViaF(connString, queryText, factory);
            return s;
            //return JsonConvert.SerializeObject(lstClients);
        }

        [HttpPost]
        [ActionName("insertCustomer")]
        public string insertCustomer(clienti obj)
        {
            string queryString = "insert into clienti (id, req, mag) values(";
            queryString += obj.id + "," + obj.req + ",'" + obj.mag +"')";
            m.execNonQueryViaF(connString, queryString, factory);
            return "Customer inserted";   // oppure dichiararla static
        }

        [HttpPut]
        [ActionName("updateCustomer")]
        public string updateCustomer(clienti obj)
        {
            string queryString = "update clienti set req = '" + obj.req +"' where id=" + obj.id;
            m.execNonQueryViaF(connString, queryString, factory);
            return "Customer updated";
        }

        [HttpDelete]
        [ActionName("deleteCustomer")]
        public string deleteCustomer(int id)
        {
            string queryString = "delete from clienti where id=" + id;
            m.execNonQueryViaF(connString, queryString, factory);
            return "Customer deleted";
        }

        public string getGAPInstance(int id)
        {
            string res = "non trovato";
            
            return res;
        }
    }
}