using CarparkInfoApi;
using Microsoft.EntityFrameworkCore;
using CarparkInfoApi.Data;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("CarparkInfoDb");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CarparkInfoContext>(options =>
    options.UseSqlite(connString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Insert data from csv into db
app.MapPost("/carparks/insert-carparks", async (CarparkInfoContext context) =>
{
    try
    {
        var csvProcessor = new Csv2List();
        var records = csvProcessor.ReadCsv();

        if (records == null || !records.Any())
        {
            return Results.BadRequest("No records found in the CSV file.");
        }

        await context.CarparkInfos.AddRangeAsync(records);
        await context.SaveChangesAsync();

        return Results.Ok(new { Message = $"Inserted {records.Count} records successfully.", Count = records.Count() });
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error inserting records: {ex.Message}");
    }
})
    .WithName("InsertCarparks")
    .WithOpenApi();

// Get all carparks
app.MapGet("/carparks", async (CarparkInfoContext context) =>
{
    try
    {
        var allRecords = await context.CarparkInfos.ToListAsync();
        return Results.Ok(allRecords);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving records: {ex.Message}");
    }
})
    .WithName("GetAllCarparks")
    .WithOpenApi();

// Get carpark by carpark number
app.MapGet("/carparks/{id}", async (CarparkInfoContext context, string id) =>
{
    try
    {
        if (string.IsNullOrEmpty(id))
        {
            return Results.BadRequest("Carpark number cannot be null or empty.");
        }

        var carparkRecord = await context.CarparkInfos
            .Where(x => x.CarparkNumber == id)
            .FirstOrDefaultAsync();

        if (carparkRecord == null)
        {
            return Results.NotFound($"Carpark with number {id} not found.");
        }

        return Results.Ok(carparkRecord);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error retrieving carpark: {ex.Message}");
    }
})
    .WithName("GetCarparkByNumber")
    .WithOpenApi();

// Get carparks that have free parking
app.MapGet("/carparks/free-parking", async (CarparkInfoContext context) =>
    {
        try
        {
            var freeParkingRecords = await context.CarparkInfos
                .Where(x => x.FreeParking != "NO")
                .ToListAsync();

            return Results.Ok(freeParkingRecords);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error retrieving free parking records: {ex.Message}");
        }
    })
    .WithName("GetFreeParking")
    .WithOpenApi();

// Get carparks that have night parking
app.MapGet("/carparks/night-parking", async (CarparkInfoContext context) =>
    {
        try
        {
            var nightParkingRecords = await context.CarparkInfos
                .Where(x => x.NightParking == "YES")
                .ToListAsync();

            return Results.Ok(nightParkingRecords);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error retrieving night parking records: {ex.Message}");
        }
    })
    .WithName("GetNightParking")
    .WithOpenApi();

// Get carparks that are above the minimum gantry height
app.MapGet("/carparks/gantry-height/{minGantryHeight}", async (CarparkInfoContext context, float minGantryHeight) =>
    {
        try
        {
            if (minGantryHeight <= 0)
            {
                return Results.BadRequest("Minimum gantry height must be greater than zero.");
            }

            var gantryHeightRecords = await context.CarparkInfos
                .Where(x => x.GantryHeight >= minGantryHeight)
                .ToListAsync();

            return Results.Ok(gantryHeightRecords);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error retrieving carparks by gantry height: {ex.Message}");
        }
    })
    .WithName("GetCarparksAboveMinGantryHeight")
    .WithOpenApi();

app.Run();