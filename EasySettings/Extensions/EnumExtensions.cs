using System;

namespace Nessie.ATLYSS.EasySettings.Extensions;

public static class EnumExtensions
{
    public static int GetIndex<T>(this T enumValue) where T : Enum
    {
        Type enumType = enumValue.GetType();
        string[] enumNames = Enum.GetNames(enumType);
        return Array.IndexOf(enumNames, Enum.GetName(enumType, enumValue));
    }

    public static T FromIndex<T>(int index) where T : Enum
    {
        Type enumType = typeof(T);
        Array enumValues = Enum.GetValues(enumType);
        return (T)(index >= 0 && index < enumValues.Length ? enumValues.GetValue(index) : -1);
    }
}