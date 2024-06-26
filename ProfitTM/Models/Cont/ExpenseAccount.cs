﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class ExpenseAccount
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public static List<ExpenseAccount> GetAllExpenseAccounts(string connect)
        {
            List<ExpenseAccount> accounts = new List<ExpenseAccount>();
            string DBcont = connect;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBcont))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from scGastos", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new ExpenseAccount()
                                {
                                    ID = reader["co_gas"].ToString(),
                                    Name = reader["des_gas"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                accounts = null;
                Incident.CreateIncident("ERROR BUSCANDO CUENTAS DE GASTOS", ex);
            }

            return accounts;
        }
    }
}