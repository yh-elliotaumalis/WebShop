using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FraktOmbud",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Namn = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Pris = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FraktOmbud", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategorier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Namn = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kunder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Namn = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Stad = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Postnummer = table.Column<int>(type: "int", nullable: false),
                    MobilNummer = table.Column<int>(type: "int", nullable: false),
                    Epost = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kunder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leverantörer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Namn = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leverantörer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ordrar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPris = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Betalsätt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ÄrBetald = table.Column<bool>(type: "bit", nullable: false),
                    FraktOmbudId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordrar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ordrar_FraktOmbud_FraktOmbudId",
                        column: x => x.FraktOmbudId,
                        principalTable: "FraktOmbud",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ordrar_Kunder_KundId",
                        column: x => x.KundId,
                        principalTable: "Kunder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produkter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Namn = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Beskrivning = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Pris = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Färg = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Storlek = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LagerAntal = table.Column<int>(type: "int", nullable: false),
                    LeverantörId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KategoriId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ÄrUtvald = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produkter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produkter_Kategorier_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategorier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Produkter_Leverantörer_LeverantörId",
                        column: x => x.LeverantörId,
                        principalTable: "Leverantörer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProduktOrdrar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Antal = table.Column<int>(type: "int", nullable: false),
                    PrisvidKöp = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProduktId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduktOrdrar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProduktOrdrar_Ordrar_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Ordrar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProduktOrdrar_Produkter_ProduktId",
                        column: x => x.ProduktId,
                        principalTable: "Produkter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ordrar_FraktOmbudId",
                table: "Ordrar",
                column: "FraktOmbudId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordrar_KundId",
                table: "Ordrar",
                column: "KundId");

            migrationBuilder.CreateIndex(
                name: "IX_Produkter_KategoriId",
                table: "Produkter",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Produkter_LeverantörId",
                table: "Produkter",
                column: "LeverantörId");

            migrationBuilder.CreateIndex(
                name: "IX_ProduktOrdrar_OrderId",
                table: "ProduktOrdrar",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProduktOrdrar_ProduktId",
                table: "ProduktOrdrar",
                column: "ProduktId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProduktOrdrar");

            migrationBuilder.DropTable(
                name: "Ordrar");

            migrationBuilder.DropTable(
                name: "Produkter");

            migrationBuilder.DropTable(
                name: "FraktOmbud");

            migrationBuilder.DropTable(
                name: "Kunder");

            migrationBuilder.DropTable(
                name: "Kategorier");

            migrationBuilder.DropTable(
                name: "Leverantörer");
        }
    }
}
