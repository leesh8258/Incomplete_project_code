using System;
using System.Collections.Generic;
using System.Reflection;

public static class CsvParser
{
    public static List<T> ParseCSV<T>(string csvContent) where T : new()
    {
        List<T> items = new List<T>();
        string[] lines = csvContent.Split('\n');

        if (lines.Length < 3) return items; // 최소 헤더 + 타입 + 데이터 필요

        // 1행: 헤더
        string[] headers = lines[0].Split(',');

        // 2행: 타입
        string[] types = lines[1].Split(',');

        // 열 타입 매핑
        Dictionary<string, Type> columnTypeMapping = new Dictionary<string, Type>();
        for (int i = 0; i < headers.Length; i++)
        {
            string header = headers[i].Trim();
            string typeString = i < types.Length ? types[i].Trim() : "string"; // 기본 타입은 string

            Type type = GetTypeFromString(typeString);
            if (type != null)
            {
                columnTypeMapping[header] = type;
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Unknown type '{typeString}' for column '{header}'. Defaulting to string.");
                columnTypeMapping[header] = typeof(string);
            }
        }

        // 3행 이후: 데이터
        for (int i = 2; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');
            T item = new T();

            for (int j = 0; j < headers.Length; j++)
            {
                if (j < values.Length)
                {
                    string header = headers[j].Trim();
                    string value = values[j].Trim();

                    // 필드 검색
                    var field = typeof(T).GetField(header, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (field != null)
                    {
                        try
                        {
                            // 타입 변환 후 필드 설정
                            Type expectedType = columnTypeMapping[header];
                            object convertedValue = ConvertValue(expectedType, value);
                            field.SetValue(item, convertedValue);
                        }
                        catch (Exception ex)
                        {
                            UnityEngine.Debug.LogError($"Error parsing field '{header}' with value '{value}': {ex.Message}");
                        }
                    }
                }
            }
            items.Add(item);
        }

        return items;
    }

    private static object ConvertValue(Type type, string value)
    {
        if (type == typeof(int)) return int.TryParse(value, out int intValue) ? intValue : 0;
        if (type == typeof(float)) return float.TryParse(value, out float floatValue) ? floatValue : 0f;
        if (type == typeof(bool)) return bool.TryParse(value, out bool boolValue) && boolValue;
        if (type == typeof(string)) return value;
        return null;
    }

    private static Type GetTypeFromString(string typeString)
    {
        return typeString.ToLower() switch
        {
            "int" => typeof(int),
            "float" => typeof(float),
            "bool" => typeof(bool),
            "string" => typeof(string),
            _ => null
        };
    }
}
