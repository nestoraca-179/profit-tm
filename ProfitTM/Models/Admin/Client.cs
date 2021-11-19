using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProfitTM.Models
{
    public class Client : Person
    {
        public static Client GetClient(string connect, string ID)
        {
            Client client;

            string query = string.Format("SELECT * FROM saCliente WHERE co_cli = '{0}'", ID);
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                client = new Client()
                                {
                                    ID = reader["co_cli"].ToString().Trim(),
                                    Name = reader["cli_des"].ToString(),
                                    Type = TypePerson.GetTypeAdmin(connect, reader["tip_cli"].ToString(), "C"),
                                    Zone = Zone.GetZone(connect, reader["co_zon"].ToString()),
                                    Account = Account.GetAccount(connect, reader["co_cta_ingr_egr"].ToString()),
                                    Country = Country.GetCountry(connect, reader["co_pais"].ToString()),
                                    Segment = Segment.GetSegment(connect, reader["co_seg"].ToString()),
                                    RIF = reader["rif"].ToString(),
                                    Email = reader["email"].ToString(),
                                    AltEmail = reader["email_alterno"].ToString(),
                                    Phone = reader["telefonos"].ToString(),
                                    Address1 = reader["direc1"].ToString(),
                                    Address2 = reader["direc2"].ToString(),
                                    DelAddress = reader["dir_ent2"].ToString(),
                                    Seller = Seller.GetSeller(connect, reader["co_ven"].ToString()),
                                    Cond = Cond.GetCond(connect, reader["cond_pag"].ToString()),
                                    Contrib = bool.Parse(reader["contrib"].ToString()),
                                    DateReg = DateTime.Parse(reader["fecha_reg"].ToString()),
                                    City = reader["ciudad"].ToString(),
                                    Currency = Currency.GetCurrency(connect, reader["co_mone"].ToString()),
                                    CoUsIn = reader["co_us_in"].ToString(),
                                    CoUsMo = reader["co_us_mo"].ToString(),
                                    CoSucuIn = reader["co_sucu_in"].ToString(),
                                    CoSucuMo = reader["co_sucu_mo"].ToString(),
                                    DateIn = DateTime.Parse(reader["fe_us_in"].ToString()),
                                    DateMo = DateTime.Parse(reader["fe_us_mo"].ToString()),
                                    CoTab = reader["co_tab"].ToString().Trim(),
                                    ContribuE = bool.Parse(reader["contribu_e"].ToString()),
                                    Fax = reader["fax"].ToString(),
                                    Disabled = bool.Parse(reader["inactivo"].ToString()),
                                    Juridic = bool.Parse(reader["juridico"].ToString()),
                                    Valid = bool.Parse(reader["valido"].ToString()),
                                    Salestax = reader["salestax"].ToString(),
                                    State = reader["estado"].ToString(),
                                    Monday = bool.Parse(reader["lunes"].ToString()),
                                    Tuesday = bool.Parse(reader["martes"].ToString()),
                                    Wednesday = bool.Parse(reader["miercoles"].ToString()),
                                    Thursday = bool.Parse(reader["jueves"].ToString()),
                                    Friday = bool.Parse(reader["viernes"].ToString()),
                                    Saturday = bool.Parse(reader["sabado"].ToString()),
                                    Sunday = bool.Parse(reader["domingo"].ToString()),
                                    MontCre = double.Parse(reader["mont_cre"].ToString()),
                                    DescPPago = double.Parse(reader["desc_ppago"].ToString()),
                                    DescGlob = double.Parse(reader["desc_glob"].ToString()),
                                    HorarCaja = reader["horar_caja"].ToString(),
                                    FrecuVist = reader["frecu_vist"].ToString(),
                                    TipPer = reader["tipo_per"].ToString(),
                                    Comment = reader["comentario"].ToString(),
                                    Respons = reader["respons"].ToString(),
                                    ReteRegisDoc = bool.Parse(reader["rete_regis_doc"].ToString()),
                                    PorcEsp = double.Parse(reader["porc_esp"].ToString()),
                                    TipAdi = reader["tipo_adi"].ToString(),
                                    Matriz = reader["matriz"].ToString(),
                                    CodID = int.Parse(reader["id"].ToString())
                                };
                            }
                            else
                            {
                                client = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                client = null;
            }

            return client;
        }

        public static List<Client> GetAllClients(string connect)
        {
            Client client;
            List<Client> clients = new List<Client>();
            string DBadmin = ConfigurationManager.ConnectionStrings[connect].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(DBadmin))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select * from saCliente", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                client = new Client()
                                {
                                    ID = reader["co_cli"].ToString().Trim(),
                                    Name = reader["cli_des"].ToString(),
                                    Type = TypePerson.GetTypeAdmin(connect, reader["tip_cli"].ToString(), "C"),
                                    Zone = Zone.GetZone(connect, reader["co_zon"].ToString()),
                                    Account = Account.GetAccount(connect, reader["co_cta_ingr_egr"].ToString()),
                                    Country = Country.GetCountry(connect, reader["co_pais"].ToString()),
                                    Segment = Segment.GetSegment(connect, reader["co_seg"].ToString()),
                                    RIF = reader["rif"].ToString(),
                                    Email = reader["email"].ToString(),
                                    AltEmail = reader["email_alterno"].ToString(),
                                    Phone = reader["telefonos"].ToString(),
                                    Address1 = reader["direc1"].ToString(),
                                    Address2 = reader["direc2"].ToString(),
                                    DelAddress = reader["dir_ent2"].ToString(),
                                    Seller = Seller.GetSeller(connect, reader["co_ven"].ToString()),
                                    Cond = Cond.GetCond(connect, reader["cond_pag"].ToString()),
                                    Contrib = bool.Parse(reader["contrib"].ToString()),
                                    DateReg = DateTime.Parse(reader["fecha_reg"].ToString()),
                                    City = reader["ciudad"].ToString(),
                                    Currency = Currency.GetCurrency(connect, reader["co_mone"].ToString()),
                                    CoUsIn = reader["co_us_in"].ToString(),
                                    CoUsMo = reader["co_us_mo"].ToString(),
                                    CoSucuIn = reader["co_sucu_in"].ToString(),
                                    CoSucuMo = reader["co_sucu_mo"].ToString(),
                                    DateIn = DateTime.Parse(reader["fe_us_in"].ToString()),
                                    DateMo = DateTime.Parse(reader["fe_us_mo"].ToString()),
                                    CoTab = reader["co_tab"].ToString().Trim(),
                                    ContribuE = bool.Parse(reader["contribu_e"].ToString()),
                                    Fax = reader["fax"].ToString(),
                                    Disabled = bool.Parse(reader["inactivo"].ToString()),
                                    Juridic = bool.Parse(reader["juridico"].ToString()),
                                    Valid = bool.Parse(reader["valido"].ToString()),
                                    Salestax = reader["salestax"].ToString(),
                                    State = reader["estado"].ToString(),
                                    Monday = bool.Parse(reader["lunes"].ToString()),
                                    Tuesday = bool.Parse(reader["martes"].ToString()),
                                    Wednesday = bool.Parse(reader["miercoles"].ToString()),
                                    Thursday = bool.Parse(reader["jueves"].ToString()),
                                    Friday = bool.Parse(reader["viernes"].ToString()),
                                    Saturday = bool.Parse(reader["sabado"].ToString()),
                                    Sunday = bool.Parse(reader["domingo"].ToString()),
                                    MontCre = double.Parse(reader["mont_cre"].ToString()),
                                    DescPPago = double.Parse(reader["desc_ppago"].ToString()),
                                    DescGlob = double.Parse(reader["desc_glob"].ToString()),
                                    HorarCaja = reader["horar_caja"].ToString(),
                                    FrecuVist = reader["frecu_vist"].ToString(),
                                    TipPer = reader["tipo_per"].ToString(),
                                    Comment = reader["comentario"].ToString(),
                                    Respons = reader["respons"].ToString(),
                                    ReteRegisDoc = bool.Parse(reader["rete_regis_doc"].ToString()),
                                    PorcEsp = double.Parse(reader["porc_esp"].ToString()),
                                    TipAdi = reader["tipo_adi"].ToString(),
                                    Matriz = reader["matriz"].ToString(),
                                    CodID = int.Parse(reader["id"].ToString())
                                };

                                client.ExtraFields = new List<string>();
                                for (int i = 1; i < 9; i++)
                                {
                                    client.ExtraFields.Add(reader["campo" + i].ToString());
                                }

                                clients.Add(client);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clients = null;
            }

            return clients;
        }
    }
}