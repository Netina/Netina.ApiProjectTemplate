namespace Common.Extensions;

public static class StringExtensions
{
    public static string ToPriceWhitPriceType(this long price, string priceType)
    {
        return price.ToString("N0") + " " + priceType;
    }

    public static string ToPriceWhitPriceType(this decimal price, string priceType)
    {
        return price.ToString("N0") + " " + priceType;
    }

    public static string ToPriceWhitPriceType(this double price, string priceType)
    {
        return price.ToString("N0") + " " + priceType;
    }

    public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
    {
        return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
    }

    public static int ToInt(this string value)
    {
        return Convert.ToInt32(value);
    }

    public static decimal ToDecimal(this string value)
    {
        return Convert.ToDecimal(value);
    }

    public static string ToNumeric(this int value)
    {
        return value.ToString("N0"); //"123,456"
    }

    public static string ToNumeric(this decimal value)
    {
        return value.ToString("N0");
    }

    public static string ToCurrency(this int value)
    {
        //fa-IR => current culture currency symbol => ریال
        //123456 => "123,123ریال"
        return value.ToString("C0");
    }

    public static string ToCurrency(this decimal value)
    {
        return value.ToString("C0");
    }

    public static string En2Fa(this string str)
    {
        return str.Replace("0", "۰")
            .Replace("1", "۱")
            .Replace("2", "۲")
            .Replace("3", "۳")
            .Replace("4", "۴")
            .Replace("5", "۵")
            .Replace("6", "۶")
            .Replace("7", "۷")
            .Replace("8", "۸")
            .Replace("9", "۹");
    }

    public static string Fa2En(this string str)
    {
        return str.Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9")
            //iphone numeric
            .Replace("٠", "0")
            .Replace("١", "1")
            .Replace("٢", "2")
            .Replace("٣", "3")
            .Replace("٤", "4")
            .Replace("٥", "5")
            .Replace("٦", "6")
            .Replace("٧", "7")
            .Replace("٨", "8")
            .Replace("٩", "9");
    }

    public static string FixPersianChars(this string str)
    {
        return str.Replace("ﮎ", "ک")
            .Replace("ﮏ", "ک")
            .Replace("ﮐ", "ک")
            .Replace("ﮑ", "ک")
            .Replace("ك", "ک")
            .Replace("ي", "ی")
            .Replace(" ", " ")
            .Replace("‌", " ")
            .Replace("ھ", "ه"); //.Replace("ئ", "ی");
    }

    public static string CleanString(this string str)
    {
        return str.Trim().FixPersianChars().Fa2En().NullIfEmpty();
    }

    public static string NullIfEmpty(this string str)
    {
        return str?.Length == 0 ? null : str;
    }

    public static string GetId(int length = 8)
    {
        return Guid.NewGuid().ToString("N").Substring(0, length);
    }

    public static string ConvertTo3Digit(this string str)
    {
        str = string.Concat(str.Split(','));
        var array = str.ToCharArray();
        Array.Reverse(array);
        str = new string(array);

        var newStr = "";
        for (var i = 0; i < str.Length; i++)
        {
            newStr += str[i];
            if ((i + 1) % 3 == 0)
                newStr += ",";
        }

        var newarray = newStr.ToCharArray();
        Array.Reverse(newarray);
        newStr = new string(newarray);
        if (newStr.Length > 0 && newStr[0] == ',')
            newStr = newStr.Substring(1);

        return newStr;
    }

    public static string ConvertToOrginal(this string str)
    {
        return string.Concat(str.Split(','));
    }

    public static string CheckPhoneNumber(string phoneNumber)
    {
        if (phoneNumber.Substring(0, 3).Contains("+98"))
            phoneNumber = phoneNumber.Replace("+98", "0");
        else if (phoneNumber.Substring(0, 2).Contains("98"))
            phoneNumber = string.Concat("0", phoneNumber.Substring(2));
        else if (phoneNumber[0] != '0')
            phoneNumber = string.Concat("0", phoneNumber);

        return phoneNumber;
    }
}