using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class RandomList
    {
        public static string GenerateRandomLetters(int length)
        {
            Random random = new Random();
            string letters = "abcdefghijklmnopqrstuvwxyz1234567890";
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = letters[random.Next(letters.Length)];
            }

            return new string(result);
        }
    }
}
