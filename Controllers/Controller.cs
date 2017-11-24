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

        public class Clienti
        {
            public int id;
            public int req;
            public int mag;
        }

        public class Magazzini
        {
            public int id;
            public int node_id;
            public int cap;
        }
        public class Costi
        {
            public int id;
            public int mag;
            public int cli;
            public int cost;
        }

        public class Instance
        {
            public string name;
            public int numcustomers;
            public int numfacilities;
            public int[,] cost;
            public int[,] req;
            public int[] cap;
        }
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
            //string res = "non trovato";
            string queryText = "SELECT * FROM clienti";
            string s = m.readTableViaF(connString, queryText, factory);
            Clienti[] clienti = JsonConvert.DeserializeObject<Clienti[]>(s);

            string queryText1 = "SELECT * FROM magazzini";
            string s1 = m.readTableViaF(connString, queryText1, factory);
            Magazzini[] magazzini = JsonConvert.DeserializeObject<Magazzini[]>(s1);

            string queryText2 = "SELECT * FROM costi";
            string s2 = m.readTableViaF(connString, queryText2, factory);
            Costi[] costi = JsonConvert.DeserializeObject<Costi[]>(s2);


            Instance instance = new Instance();

            instance.name = "fromDB";
            //La tabella contiene gli ordini di ogni cliente su ogni magazzino:
            instance.numcustomers = clienti.Length/magazzini.Length;
            instance.numfacilities = magazzini.Length;
            int[,] cost = new int[instance.numfacilities, instance.numcustomers];
            for (int i = 0; i < costi.Length; i++)
            {
                cost[costi[i].mag - 1, costi[i].cli - 1] = costi[i].cost;
            }

            int[,] req = new int[instance.numfacilities, instance.numcustomers];
            for (int i = 0; i < clienti.Length; i++)
            {
                req[clienti[i].mag - 1, clienti[i].id - 1] = clienti[i].req;
            }

            int[] cap = new int[instance.numfacilities];
            for (int i = 0; i < magazzini.Length; i++)
            {
                cap[magazzini[i].id - 1] = magazzini[i].cap;
            }

            instance.cost = cost;
            instance.req = req;
            instance.cap = cap;



            return JsonConvert.SerializeObject(instance);
        }



    }
}