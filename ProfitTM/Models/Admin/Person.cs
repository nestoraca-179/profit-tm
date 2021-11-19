using System;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Person
    {
        // ATRIBUTOS COMUNES CLIENTE - PROVEEDOR
        public string ID { get; set; }
        public string Name { get; set; }
        public TypePerson Type { get; set; }
        public Zone Zone { get; set; }
        public Account Account { get; set; }
        public Country Country { get; set; }
        public Segment Segment { get; set; }
        public string RIF { get; set; }
        public string Email { get; set; }
        public string AltEmail { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string DelAddress { get; set; }
        public double Amount { get; set; }
        public Cond Cond { get; set; }
        public int PlazPag { get; set; }
        public string City { get; set; }
        public Currency Currency { get; set; }
        public string CoUsIn { get; set; }
        public string CoUsMo { get; set; }
        public string CoSucuIn { get; set; }
        public string CoSucuMo { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateMo { get; set; }
        public string CoTab { get; set; }
        public string Comment { get; set; }
        public bool ContribuE { get; set; }
        public double DescGlob { get; set; }
        public double DescPPago { get; set; }
        public string Fax { get; set; }
        public string DisCen { get; set; }
        public string NumCom { get; set; }
        public DateTime FecCom { get; set; }
        public DateTime DateReg { get; set; }
        public bool Disabled { get; set; }
        public string Matriz { get; set; }
        public double MontCre { get; set; }
        public double PorcEsp { get; set; }
        public string Respons { get; set; }
        public bool ReteRegisDoc { get; set; }
        public string Reviewed { get; set; }
        public string Rowguid { get; set; }
        public string TipAdi { get; set; }
        public string TipPer { get; set; }
        public string Trasnfe { get; set; }
        public string Validator { get; set; }
        public string Website { get; set; }
        public string Zip { get; set; }
        public List<string> ExtraFields { get; set; }

        // ATRIBUTOS CLIENTE
        public int Score { get; set; }
        public Seller Seller { get; set; }
        public string HorarCaja { get; set; }
        public string FrecuVist { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool Contrib { get; set; }
        public bool Juridic { get; set; }
        public bool Valid { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool SinCred { get; set; }
        public string SerialP { get; set; }
        public int CodID { get; set; }
        public string Salestax { get; set; }
        public string State { get; set; }

        // ATRIBUTOS PROVEEDOR
        public bool National { get; set; }
        public string FormType { get; set; }
        public string TaxID { get; set; }
        public bool RetenISLR { get; set; }
    }
}