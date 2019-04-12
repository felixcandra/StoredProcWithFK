using Newtonsoft.Json;
using StoredProcWithFK.Models;
using StoredProcWithFK.Models.Param;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StoredProcWithFK.Controllers
{
    public class ItemRepository
    {
        public void Create(ItemParam item)
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString; //Connection String
            using (SqlConnection con = new SqlConnection(CS)) //menggunakan using agar tidak perlu melakukan close connection karena otomatis menutup connection
            {
                SqlCommand cmd = new SqlCommand("spAddItem", con); //parameter pertama nama stored procedure, parameter kedua connection stringnya
                cmd.CommandType = CommandType.StoredProcedure; //tipe perintah yang akan dieksekusi
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Stock", item.Stock);
                cmd.Parameters.AddWithValue("@Suppliers_Id", item.Suppliers_Id);

                SqlParameter prm = new SqlParameter();
                prm.ParameterName = "@Id";
                prm.SqlDbType = SqlDbType.Int;
                prm.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(prm);

                con.Open();
                cmd.ExecuteNonQuery();
                int id = Convert.ToInt16(prm.Value);
            }
        }

        public void Edit(ItemParam item)
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spUpdateItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Stock", item.Stock);
                cmd.Parameters.AddWithValue("@Suppliers_Id", item.Suppliers_Id);

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
                SqlCommand cmd = new SqlCommand("spDeleteItem", con);
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

        public string GetItem()
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spSelectAllItem", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                List<Item> itemList = new List<Item>();
                while (dr.Read())
                {
                    SqlCommand cmd2 = new SqlCommand("spGetSupplierForItem", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@Id", dr[3]);

                    SqlDataReader dr2 = cmd2.ExecuteReader();
                    Supplier supplier = new Supplier();
                    while (dr2.Read())
                    {
                        supplier.Id = Convert.ToInt16(dr2[0]);
                        supplier.Name = Convert.ToString(dr2[1]);
                    }
                    itemList.Add(new Item()
                    {
                        Id = Convert.ToInt16(dr[0]),
                        Name = Convert.ToString(dr[1]),
                        Stock = Convert.ToInt16(dr[2]),
                        Suppliers = supplier
                    });
                }
                var data = JsonConvert.SerializeObject(itemList);
                return data;
            }
        }

        public string GetItemById(int id)
        {
            string CS = ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spGetItemById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Stock", SqlDbType.Int, 2).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Suppliers_Id", SqlDbType.Int, 2).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                ItemParam item = new ItemParam();
                item.Id = Convert.ToInt16(cmd.Parameters["@Id"].Value);
                item.Name = Convert.ToString(cmd.Parameters["@Name"].Value);
                item.Stock = Convert.ToInt16(cmd.Parameters["@Stock"].Value);
                item.Suppliers_Id = Convert.ToInt16(cmd.Parameters["@Suppliers_Id"].Value);
                var data = JsonConvert.SerializeObject(item);
                return data;
            }
        }

    }
}