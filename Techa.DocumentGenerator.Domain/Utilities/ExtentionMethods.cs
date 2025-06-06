﻿namespace Techa.DocumentGenerator.Domain.Utilities
{
    public static class ExtentionMethods
    {
        public static bool NationalCodeValidator(this string nationalCode)
        {
            bool isValid;
            try
            {
                char[] chArray = nationalCode.ToCharArray();
                int[] numArray = new int[chArray.Length];
                for (int i = 0; i < chArray.Length; i++)
                {
                    numArray[i] = (int)char.GetNumericValue(chArray[i]);
                }
                int num2 = numArray[9];
                switch (nationalCode)
                {
                    case "0000000000":
                    case "1111111111":
                    case "22222222222":
                    case "33333333333":
                    case "4444444444":
                    case "5555555555":
                    case "6666666666":
                    case "7777777777":
                    case "8888888888":
                    case "9999999999":
                        isValid = false;
                        break;
                }
                int num3 = ((((((((numArray[0] * 10) + (numArray[1] * 9)) + (numArray[2] * 8)) + (numArray[3] * 7)) + (numArray[4] * 6)) + (numArray[5] * 5)) + (numArray[6] * 4)) + (numArray[7] * 3)) + (numArray[8] * 2);
                int num4 = num3 - ((num3 / 11) * 11);
                if ((((num4 == 0) && (num2 == num4)) || ((num4 == 1) && (num2 == 1))) || ((num4 > 1) && (num2 == Math.Abs((int)(num4 - 11)))))
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }
            catch (Exception)
            {
                isValid = false;
            }

            return isValid;
        }
        public static string RemoveSpecialCharacters(this string input)
        {
            // حذف کاراکترهای اضافی `json` از ابتدا و انتها
            if (input.StartsWith("```json"))
            {
                input = input.Substring(7); // حذف 7 کاراکتر ابتدای رشته (```json\n)
            }

            if (input.EndsWith("```"))
            {
                input = input.Substring(0, input.Length - 3); // حذف 3 کاراکتر انتهایی (```)
            }

            // حذف فاصله‌های اضافی
            return input.Trim();
        }
    }
}
