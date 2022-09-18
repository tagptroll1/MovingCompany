using Npgsql;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Dapper;

public static class DapperExtensions
{
    public static Task<int> InsertAsync<T>(this NpgsqlConnection connection, T entity, DbTransaction? transaction = null)
    {
        var fields = ModelField.CreateModel<T>();
        var sqlFieldNames = new List<string>();
        var sqlValuePlaceholders = new List<string>();
        var queryParams = new DynamicParameters();

        foreach (var fld in fields)
        {
            if (!fld.IsPrimaryKey)
            {
                var parName = "@" + fld.Name;
                sqlFieldNames.Add($"\"{fld.Name?.ToLower()}\"");
                var value = typeof(T).GetProperty(fld.Name!)?.GetValue(entity, null);
                if (fld.IsNullValue(value))
                {
                    sqlValuePlaceholders.Add("NULL");
                }
                else
                {
                    sqlValuePlaceholders.Add(parName);
                    queryParams.Add(parName, value, fld.SqlDatatype);
                }
            }
        }
        var sql = @$"
            INSERT INTO {typeof(T).Name} ({string.Join(",", sqlFieldNames)}) 
            VALUES ({string.Join(",", sqlValuePlaceholders)})
            RETURNING ID";
        return connection.ExecuteScalarAsync<int>(sql, queryParams, transaction: transaction);
    }


}

public sealed class ModelField
{
    public string? Name { get; set; }
    public DbType SqlDatatype { get; set; }
    public bool IsPrimaryKey { get; set; }

    public override string ToString()
    {
        return Name ?? string.Empty;
    }

    public bool IsNullValue<T>(T value)
    {
        if (SqlDatatype == DbType.DateTime2 && (DateTime)(object)value == DateTime.MinValue)
        {
            return true;
        }
        if (value == null)
        {
            return true;
        }
        return false;
    }

    public static List<ModelField> CreateModel<T>()
    {
        var list = new List<ModelField>();
        Type X = typeof(T);
        foreach (PropertyInfo p in X.GetProperties())
        {
            if (p.CanWrite && p.CanRead)
            {
                if (p.PropertyType?.FullName?.Contains("System.Collections.Generic.List") ?? false)
                {
                    continue;
                }

                var fi = ExtractFromProp(p);
                if (fi != null)
                {
                    fi.IsPrimaryKey = (list.Count == 0 && fi.Name?.ToUpper() == "ID");
                    list.Add(fi);
                }
            }
        }
        return list;
    }

    private static ModelField? ExtractFromProp(PropertyInfo info)
    {
        Type T = info.PropertyType;

        if (T?.FullName?.Contains("System.Nullable") ?? false)
        {
            T = Nullable.GetUnderlyingType(T) ?? T;
        }

        if (T.IsClass && T != typeof(string))
        {
            return null;
        }
        return new ModelField() 
        {
            Name = info.Name,
            SqlDatatype = NetType2SqlType(T)
        };
    }

    private static DbType NetType2SqlType(Type T)
    {
        switch (T.Name)
        {
            case "Int32":
                return DbType.Int32;
            case "Boolean":
                return DbType.Boolean;
            case "Decimal":
                return DbType.Decimal;
            case "DateTime":
                return DbType.DateTime2;
            case "String":
                return DbType.String;
        }
        if (T.BaseType?.FullName == "System.Enum")
            return DbType.Int32;

        return DbType.String;

    }
}
