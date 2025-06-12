// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("a2JFtmDB/WzxtljynZ3veEQzF0EoAeD8kJVcnCTOR6rg+GupMwcftWS2AX+zr907eLEOpmsGXZ07hqkzLrRHifA4ZFwf7HIEOIGtkrOSyu1IuF0NH1kHRxRjTKf87+paPHjjPXcTdVVCAYahJHbErPKhMmmUeNGeqSokKxupKiEpqSoqK7ySD/JFkzje2m7HmtKSFlrdOzhdgt02lH/22RupKgkbJi0iAa1jrdwmKioqLisoQX4Zxd+zVi7Ep9VzFeYDPoanau4xczeQDetspxNnefsr1T3ezy7IN/gsIuevJGZJlA68WaDpwnCWB380PC8L99+Pcbn5v9rgOzTTaSfkFmlc6NG6aYj3HW2gaYJmzv2HqlzYaoFWHBjAlswJDikoKisq");
        private static int[] order = new int[] { 5,3,8,4,12,11,12,9,11,12,11,13,13,13,14 };
        private static int key = 43;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
