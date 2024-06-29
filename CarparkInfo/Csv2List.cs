using System.Globalization;
using CsvHelper;

namespace CarparkInfo;

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
        // foreach (var r in records)
        // {
        //     Console.WriteLine($"{r.CarparkNumber}, {r.Address}, {r.XCoordinate}, {r.YCoordinate}, {r.CarparkType}, {r.TypeOfParkingSystem}, {r.ShortTermParking}, {r.FreeParking}, {r.NightParking}, {r.CarparkDecks}, {r.GantryHeight}, {r.CarparkBasement}");
        // }
    }
}