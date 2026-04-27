using System.Data;
using System.Reflection;

namespace DNR26V2.Services.Reports;

public static class ReportDataTableFactory
{
    public const string DsHeader = "dsHeader";
    public const string DsLines  = "dsLines";

    public static DataTable ToDataTable<T>(IEnumerable<T> items)
    {
        var table = new DataTable(typeof(T).Name);
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            var colType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            table.Columns.Add(prop.Name, colType);
        }

        foreach (var item in items)
        {
            var row = table.NewRow();
            foreach (var prop in props)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }

        return table;
    }

    public static DataTable SingleRowTable<T>(T item) => ToDataTable(new[] { item });
}
