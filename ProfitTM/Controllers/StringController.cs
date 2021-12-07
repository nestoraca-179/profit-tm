namespace ProfitTM.Controllers
{
    public class StringController
    {
        public static string VerifyValueDb(string value)
        {
            string result = "NULL";

            if (!string.IsNullOrEmpty(value))
                result = string.Format("'{0}'", value);

            return result;
        }
    }
}