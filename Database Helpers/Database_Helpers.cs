using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Helpers
{
    public class helper
    {

        /// <summary>
        /// Dataset term used in insert and update function refers to set of column name with its value
        /// e.g name: lorem ispum , here name is the column name and lorem ispum is its value which will
        /// store in the database.
        /// This helper is written by Muhammad Bilal Amjad at Attrayant Designs (Pvt) Ltd
        /// bilalamjad@attrayantdesigns.com
        /// </summary>

        public Dictionary<string, string> values = new Dictionary<string, string>();

        public SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
        public bool insert(string table_name, Dictionary<string, string> dataset)
        {
            try
            {
                #region query_builder
                string query = "Insert into dbo." + table_name + " (";
                foreach (var col in dataset)
                {
                    query += col.Key.ToString() + ",";
                }
                query = query.TrimEnd(',');
                query += ") VALUES(";
                foreach (var val in dataset)
                {
                    query += "'" + val.Value.ToString() + "',";
                }
                query = query.TrimEnd(',');
                query += ")";
                #endregion

                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                cmd.ExecuteNonQuery();
                Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return false;
            }

        }
        public bool update(string table_name, Dictionary<string, string> dataset, string where_clause)
        {
            try
            {


                #region query_builder
                string query = "Update " + table_name + " SET ";

                int count = 0;
                foreach (var col in dataset)
                {
                    query += col.Key + " = '" + col.Value + "'";

                    count++;
                    if (count < dataset.Values.Count)
                    {
                        query = query + " , ";
                    }

                }

                query = query.TrimEnd(',');

                query += " " + where_clause;
                #endregion

                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                cmd.ExecuteNonQuery();
                Connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return false;
            }

        }
        public bool delete(string table_name, string where_clause)
        {
            try
            {
                #region query_builder
                string query = "Delete from " + table_name + " " + where_clause;
                #endregion
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                cmd.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return false;
            }
        }

        public bool log(string type, string description, string performed_by, string performed_on)
        {
            try
            {
                string query = "Insert into log(type,description,performed_by,performed_on) Values(@type,@desc,@pb,@po)";
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@pb", performed_by);
                cmd.Parameters.AddWithValue("@po", performed_on);
                cmd.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return false;
            }
        }

        public string insert_and_get_id(string table_name, Dictionary<string, string> dataset)
        {
            try
            {
                int id = 0;
                #region query_builder
                string query = "Insert into dbo." + table_name + " (";
                foreach (var col in dataset)
                {
                    query += col.Key.ToString() + ",";
                }
                query = query.TrimEnd(',');
                query += ") VALUES(";
                foreach (var val in dataset)
                {
                    query += "'" + val.Value.ToString() + "',";
                }
                query = query.TrimEnd(',');
                query += ") Select Scope_Identity()";
                #endregion


                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                id = Convert.ToInt32(cmd.ExecuteScalar());

                Connection.Close();

                return id.ToString();
            }
            catch (Exception ex)
            {
                Connection.Close();
                return "failed " + ex.Message;
            }

        }

        public int get_scalar(string query)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                Connection.Close();
                return count;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return 0;
            }
        }

        public string get_scalar_string(string query)
        {
            try
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand(query, Connection);
                string result = cmd.ExecuteScalar().ToString();
                Connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                Connection.Close();
                return "";
            }
        }



    }

}
