using CarparkInfo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CarparkInfoContext>(options =>
    options.UseSqlite("Data Source=carparkInfo.db"));
builder.Services.AddScoped<UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Insert data from csv into db
app.MapPost("/insertcarparks", async (UnitOfWork unitOfWork) =>
    {
        var csvProcessor = new Csv2List();
        var records = csvProcessor.ReadCsv();

        foreach (var r in records)
        {
            unitOfWork.CarparkInfoRepository.Insert(r);
        }

        unitOfWork.Save();
        return records;
    })
    .WithName("InsertCarparks")
    .WithOpenApi();

// Get all carparks
app.MapGet("/carparks", async (UnitOfWork unitOfWork) =>
    {
        var allRecords = unitOfWork.CarparkInfoRepository.Get();
        return allRecords;
    })
    .WithName("GetAllCarparks")
    .WithOpenApi();

// Get carpark by carpark number
app.MapGet("/carpark/{id}", async (UnitOfWork unitOfWork, string id) =>
    {
        var carparkRecord = unitOfWork.CarparkInfoRepository.Get(x => x.CarparkNumber == id);
        return carparkRecord;
    })
    .WithName("GetCarparkByNumber")
    .WithOpenApi();

// Get carparks that have free parking
app.MapGet("/freeparking", async (UnitOfWork unitOfWork) =>
    {
        var freeParkingRecords = unitOfWork.CarparkInfoRepository.Get(x => x.FreeParking != "NO");
        return freeParkingRecords;
    })
    .WithName("GetFreeParking")
    .WithOpenApi();

// Get carparks that have night parking
app.MapGet("/nightparking", async (UnitOfWork unitOfWork) =>
    {
        var nightParkingRecords = unitOfWork.CarparkInfoRepository.Get(x => x.NightParking == "YES");
        return nightParkingRecords;
    })
    .WithName("GetNightParking")
    .WithOpenApi();

var requestMinGantryHeight = 3;

// Get carparks that are above the minimum gantry height
app.MapGet("/gantryheight", async (UnitOfWork unitOfWork) =>
    {
        var gantryHeightRecords = unitOfWork.CarparkInfoRepository.Get(x => x.GantryHeight >= requestMinGantryHeight);
        return gantryHeightRecords;
    })
    .WithName("GetCarparksAboveMinGantryHeight")
    .WithOpenApi();

app.Run();