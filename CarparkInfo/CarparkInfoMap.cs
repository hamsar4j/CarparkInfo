using CsvHelper.Configuration;

namespace CarparkInfo;

public class CarparkInfoMap : ClassMap<CarparkInfo>
{
    public CarparkInfoMap()
    {
        Map(m => m.CarparkNumber).Name("car_park_no");
        Map(m => m.Address).Name("address");
        Map(m => m.XCoordinate).Name("x_coord");
        Map(m => m.YCoordinate).Name("y_coord");
        Map(m => m.CarparkType).Name("car_park_type");
        Map(m => m.TypeOfParkingSystem).Name("type_of_parking_system");
        Map(m => m.ShortTermParking).Name("short_term_parking");
        Map(m => m.FreeParking).Name("free_parking");
        Map(m => m.NightParking).Name("night_parking");
        Map(m => m.CarparkDecks).Name("car_park_decks");
        Map(m => m.GantryHeight).Name("gantry_height");
        Map(m => m.CarparkBasement).Name("car_park_basement");
    }
}