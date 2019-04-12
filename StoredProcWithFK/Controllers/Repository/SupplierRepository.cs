using Newtonsoft.Json;
using StoredProcWithFK.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoredProcWithFK.Controllers.Repository
{
    public class SupplierRepository
    {
        public void Create(Supplier supplier)
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spAddSupplier", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", supplier.Name);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Edit(Supplier supplier)
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spUpdateSupplier", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", supplier.Id);
                cmd.Parameters.AddWithValue("@Name", supplier.Name);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool Delete(int id)
        {
            if (id == 0)
            {
                return false;
            }
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spDeleteSupplier", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public string GetSupplier()
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spSupplierDropdown", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                List<Supplier> supplier = new List<Supplier>();
                while (dr.Read())
                {
                    supplier.Add(new Supplier
                    {
                        Id = Convert.ToInt16(dr[0]),
                        Name = Convert.ToString(dr[1])
                    });
                }
                var data = JsonConvert.SerializeObject(supplier);
                return data;
            }
        }

        public string GetSupplierById(int id)
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetSupplierById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@IdOut", SqlDbType.Int, 2).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                Supplier supplier = new Supplier();
                supplier.Id = Convert.ToInt16(cmd.Parameters["@IdOut"].Value);
                supplier.Name = Convert.ToString(cmd.Parameters["@Name"].Value);
                var data = JsonConvert.SerializeObject(supplier);
                return data;
            }
        }
    }
}