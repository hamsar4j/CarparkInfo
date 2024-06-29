using CarparkInfo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CarparkInfoContext>(options =>
    options.UseSqlite("Data Source=carparkInfo.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/allcarparks", async (CarparkInfoContext dbContext) =>
    {
        var CsvProcessor = new Csv2List();
        var records = CsvProcessor.ReadCsv();

        await dbContext.CarparkInfos.AddRangeAsync(records);
        await dbContext.SaveChangesAsync();

        return records;
    })
    .WithName("GetAllCarparks")
    .WithOpenApi();

app.MapGet("/freeparking", async (CarparkInfoContext dbContext) =>
{
    var freeParkingRecords = await dbContext.CarparkInfos
        .Where(x => x.FreeParking != "NO")
        .ToListAsync();
    return freeParkingRecords;
});

app.MapGet("/nightparking", async (CarparkInfoContext dbContext) =>
    {
        var nightParkingRecords = await dbContext.CarparkInfos
            .Where(x => x.NightParking == "YES")
            .ToListAsync();
        return nightParkingRecords;
    })
    .WithName("GetNightParking")
    .WithOpenApi();

var requestMinGantryHeight = 3;

app.MapGet("/gantryheight", async (CarparkInfoContext dbContext) =>
{
    var gantryHeightRecords = await dbContext.CarparkInfos
        .Where(x => x.GantryHeight >= requestMinGantryHeight)
        .ToListAsync();
    return gantryHeightRecords;
});

var requestCarparkNumber = "AM43";

app.MapGet("/carpark", async (CarparkInfoContext dbContext) =>
{
    var carparkRecord = await dbContext.CarparkInfos
        .Where(x => x.CarparkNumber == requestCarparkNumber)
        .ToListAsync();
    return carparkRecord;
});

app.Run();