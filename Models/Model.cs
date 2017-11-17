using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Data.Common;


namespace WebApplication1.Models
{
    class Model 
    {
        public delegate void viewEventHandler(object sender, string textToWrite); // questo gestisce l'evento (mittente e testo generato per l'interfaccia)
        public event viewEventHandler FlushText;  // questo genera l'evento 
        public void doSomething()
        {
            for (int i = 0; i < 10; i++)
                FlushText(this, $"i={i}");
        }

        public void connect(string connString, bool isSql, String id)

        {
            //string sqLiteConnString = @"Data Source=c:\Temp\dbGAP.sqlite;Version=3;";
            IDbConnection conn = null;

            try {
                if (isSql)
                    conn = new SQLiteConnection(connString);
                else
                    conn = new SqlConnection(connString);
                conn.Open();
                IDbCommand com = conn.CreateCommand();
                //string queryText = "select nome, descr from ordini as o , clienti as c where o.idcliente = c.id and c.nome LIKE 'minnie'";
                string queryText = "select idserie, periodo, val from histordini where idserie = @id";
                com.CommandText = queryText;
                IDbDataParameter param = com.CreateParameter();
                param.DbType = DbType.Int32;
                param.ParameterName = "@id";
                param.Value = id;
                com.Parameters.Add(param);
                IDataReader reader = com.ExecuteReader();
                while(reader.Read())
                {
                    FlushText(this, reader["idserie"] + " " + reader["periodo"] + " " + reader["val"]);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                FlushText(this, e.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            
        }

        public void connectDs(string connString, bool isSql, String id)
        {
            DataSet ds = new DataSet();
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory("System.Data.SQLite");

            using (DbConnection conn = dbFactory.CreateConnection())
            {
                try
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    DbDataAdapter dbAdapter = dbFactory.CreateDataAdapter();
                    DbCommand dbCommand = conn.CreateCommand();
                    dbCommand.CommandText = "select c.id, nome, descr from ordini as o , clienti as c where o.idcliente = c.id and c.id = @id";
                    IDbDataParameter param = dbCommand.CreateParameter();
                    param.DbType = DbType.Int32;
                    param.ParameterName = "@id";
                    param.Value = id;
                    dbCommand.Parameters.Add(param);
                    dbAdapter.SelectCommand = dbCommand;
                    dbAdapter.Fill(ds);
                    ds.Tables[0].TableName = "clienti";
                    foreach (DataRow dr in ds.Tables["clienti"].Rows)
                        FlushText(this, dr["id"] + " " + dr["nome"] + " " + dr["descr"]);
                    
                }
                catch (Exception ex)
                {
                    FlushText(this, "[FillDataSet] Error: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();

                }
            }
        }

        public string readTableViaF(string connString, string queryText, string factory)
        {
            int i, numcol;
            string res = "[";
            List<string> columns = new List<string>();
            DbProviderFactory dbFactory = null;

            dbFactory = DbProviderFactories.GetFactory(factory);

            using (DbConnection conn = dbFactory.CreateConnection())
            {
                try
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    IDbCommand com = conn.CreateCommand();
                    com.CommandText = queryText;
                    IDataReader reader = com.ExecuteReader();

                    numcol = reader.FieldCount;
                    for (i = 0; i < numcol; i++)
                        columns.Add(reader.GetName(i));

                    while (reader.Read())
                    {
                        res += "{";
                        for (i = 0; i < numcol; i++)
                        {
                            res += "\"" + columns[i] + "\":\"" + reader[i] + "\",";
                        }
                        res += "},";
                        res = res.Replace(",}", "}");
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    FlushText(this, ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            return (res + "]").Replace(",]", "]");
        }


        /*public List<ordini> connectEntity(int id)
        {
            masterEntities context = new masterEntities();
            List<ordini> list = new List<ordini>();

            foreach(ordini o in context.ordini)
            {
                if (o.idcliente.Equals(id))
                {
                    FlushText(this, "Cliente" + o.idcliente + ": ordine " + o.descr);
                    list.Add(o);
                }
            }

            list = context.ordini.SqlQuery("Select * from ordini where idcliente = " + id).ToList();
          
            return list;
            
        }*/
    }
}
