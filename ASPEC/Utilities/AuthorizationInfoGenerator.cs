using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPEC.Utilities
{
    public static class AuthorizationInfoGenerator
    {
        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 0,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = false,
                RequireUppercase = false
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        public static string GenerateLogin(string secondName, string firstName, string patronymic, DateTime birthDate)
        {
            return string.Format("{0}{1}{2}{3}",
                    Translit.RuEn(firstName.ToUpper()[0].ToString()),
                    Translit.RuEn(patronymic.ToUpper()[0].ToString()),
                    Translit.RuEn(secondName.ToUpper()),
                    birthDate.Day.ToString()).ToLower();
        }

        public class PasswordOptions
        {
            public int RequiredLength { get; set; }
            public int RequiredUniqueChars     { get; set; }
            public bool RequireDigit           { get; set; }
            public bool RequireLowercase       { get; set; }
            public bool RequireNonAlphanumeric { get; set; }
            public bool RequireUppercase       { get; set; }
        }
    }
}
