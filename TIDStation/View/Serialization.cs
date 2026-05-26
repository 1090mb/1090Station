using System.Drawing;
using System.Text.RegularExpressions;

namespace TIDStation.View
{
    public static class Serialization
    {
        public static string Serialize(object obj) 
        {
            string s = obj switch
            {
                //var _ when obj is int i => i.ToString(), // example of specific type handling
                _ => obj.ToString() ?? string.Empty,
            };
            return Regex.Escape(s);
        }

        public static object Deserialize<T>(string s) 
        {
            string sText = Regex.Unescape(s);
            if (typeof(T) == typeof(Color)) return ColorTranslator.FromHtml(sText);
            return Convert.ChangeType(sText, typeof(T));
        }
    }
}
