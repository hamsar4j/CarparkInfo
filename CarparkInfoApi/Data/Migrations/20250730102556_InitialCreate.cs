using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarparkInfoApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarparkInfos",
                columns: table => new
                {
                    CarparkNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    XCoordinate = table.Column<float>(type: "REAL", nullable: true),
                    YCoordinate = table.Column<float>(type: "REAL", nullable: true),
                    CarparkType = table.Column<string>(type: "TEXT", nullable: true),
                    TypeOfParkingSystem = table.Column<string>(type: "TEXT", nullable: true),
                    ShortTermParking = table.Column<string>(type: "TEXT", nullable: true),
                    FreeParking = table.Column<string>(type: "TEXT", nullable: true),
                    NightParking = table.Column<string>(type: "TEXT", nullable: true),
                    CarparkDecks = table.Column<int>(type: "INTEGER", nullable: true),
                    GantryHeight = table.Column<float>(type: "REAL", nullable: true),
                    CarparkBasement = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarparkInfos", x => x.CarparkNumber);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarparkInfos");
        }
    }
}
