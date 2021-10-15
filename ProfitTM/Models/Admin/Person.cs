namespace ProfitTM.Models
{
    public class Person
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public Zone Zone { get; set; }
        public Account Account { get; set; }
        public Country Country { get; set; }
        public Segment Segment { get; set; }
        public string RIF { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Amount { get; set; }
        public Seller Seller { get; set; }
        public Cond Cond { get; set; }
        public bool Contrib { get; set; }
    }
}