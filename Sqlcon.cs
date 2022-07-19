using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;



namespace Magazine
{
    public class Sqlcon
    {
        
        SqlConnection sqlConnect = null;

        SqlCommand sqlCommand;
        public Sqlcon()
        {
            
            
        }

        public async Task Open()
        {
            sqlConnect = new SqlConnection(@"Data Source=DESKTOP-8T9EGI7\SQLEXPRESS;Initial Catalog=" + "magazine" + ";Integrated Security=true");
            await sqlConnect.OpenAsync();
            


        }


        public async Task<DataTable> CommnadWithQuery(string query)
        {
            try
            {
               
                await Open();
                sqlCommand = new SqlCommand(query, sqlConnect);
              
                var data = await sqlCommand.ExecuteReaderAsync();
                DataTable table = new DataTable();
                table.Load(data);
                return table;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<bool> CommnadWithNonQuery(string query)
        {
            try
            {
                await Open();
                sqlCommand = new SqlCommand(query, sqlConnect);
                var data = await sqlCommand.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
         public async Task<bool> Insert(string table_name, List<CommandData> update_data)
        {
            try
            {
                string query = "INSERT INTO " + table_name + " (";
                update_data = QueryConverter(update_data);
                query += string.Join(", ", update_data.Select(x => x.Table)) + ") VALUES (" + string.Join(", ", update_data.Select(x => x.Data)) + ");";
                return await CommnadWithNonQuery(query);
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); return false; }
        }

        private List<CommandData> QueryConverter(List<CommandData> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Data.GetType().Name == "string")
                {
                    data[i].Data = "'" + data[i].Data + "'";
                }
            }
            return data;

        }

        public async Task<bool> Update(string table_name, string id_colum_name, object id, List<CommandData> update_data)
        {
            try
            {
                string query = "UPDATE " + table_name + " SET ";
                update_data = QueryConverter(update_data);
                for (int i = 0; i < update_data.Count; i++)
                {
                    query += update_data[i].Table = " = " + update_data[i].Data;
                    if (i != update_data.Count) { query += ","; }
                }
                query += " WHERE " + id_colum_name + " = " + id;
                return await CommnadWithNonQuery(query);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(string id_colum_name, object id, string table_name)
        {
            return await CommnadWithNonQuery("DELETE FROM " + table_name + " WHERE " + id_colum_name + " = " + id);
        }
        public async Task<DataTable> GetTable(string table)
        {
            return await CommnadWithQuery("SELECT * FROM " + table);
        }

        public class CommandData
        {
            public string Table { get; set; }
            public object Data { get; set; }
        }



    }
}
