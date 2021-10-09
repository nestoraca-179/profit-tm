namespace ProfitTM.Models
{
    public class User
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string Descrip { get; set; }
        public bool IsAdm { get; set; }
        public bool IsCon { get; set; }
        public bool IsNom { get; set; }
    }
}