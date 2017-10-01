using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWC.Conversions
{
    public class NumberToText
    {
        public string GetText(ulong number)
        {
            string output = "";
            short[] process = BreakUp(number);
            for (int i = process.Length - 1; i >= 0; i--)
                if (process[i] > 0 || (i == 0 && process.Length == 1))
                    output += $"{Hundreds(process[i])} {level[i]} ";
            return output.ToString().Trim();
        }
        private readonly static String[] level = {"", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion"};
        private readonly static String[] first = {"Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        private readonly static String[] second = { "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        private readonly static String[] firstsecond = { "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen" };

        private static short[] BreakUp(ulong num)
        {
            if (num == 0)
                return new short[] { 0 };
            short[] nums = new short[(int)Math.Log10(num) / 3 + 1];
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = (short)(num % 1000);
                num /= 1000;
            }
            return nums;
        }
        private static String Hundreds(int num)
        {
            bool multi = num > 0;
            String sub = "";
            if (num / 100 > 0)
                sub += first[num / 100] + " Hundred ";
            num %= 100;
            if (num / 10 == 1 && num % 10 > 0)
                return sub + firstsecond[num % 10 - 1];
            if (num / 10 > 0)
                sub += second[num / 10 - 1] + " ";
            num %= 10;
            if (num == 0 && multi)
                return sub.Trim();
            else
                return sub + first[num].Trim();

        }
    }
}
