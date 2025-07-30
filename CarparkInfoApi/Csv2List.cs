using System.Globalization;
using CsvHelper;
using CarparkInfoApi.Models;
using CarparkInfoApi.Mappings;

namespace CarparkInfoApi;

public class Csv2List
{
    public List<CarparkInfo> ReadCsv()
    {
        const string csvPath = "hdb-carpark-information-20220824010400.csv";
        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<CarparkInfoMap>();
        var records = csv.GetRecords<CarparkInfo>().ToList();

        return records;
    }
}