using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rancho.Services.Feeding.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "feeding");

            migrationBuilder.CreateTable(
                name: "farms",
                schema: "feeding",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ownerid = table.Column<Guid>(name: "owner_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_farms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "animals",
                schema: "feeding",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    species = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    birthdate = table.Column<DateTime>(name: "birth_date", type: "timestamp with time zone", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    eartagnumber = table.Column<string>(name: "ear_tag_number", type: "text", nullable: false),
                    farmid = table.Column<Guid>(name: "farm_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_animals", x => x.id);
                    table.ForeignKey(
                        name: "fk_animals_farms_farm_id",
                        column: x => x.farmid,
                        principalSchema: "feeding",
                        principalTable: "farms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feeds",
                schema: "feeding",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    nutritionprotein = table.Column<decimal>(name: "nutrition_protein", type: "numeric", nullable: false),
                    nutritionfat = table.Column<decimal>(name: "nutrition_fat", type: "numeric", nullable: false),
                    nutritioncarbohydrate = table.Column<decimal>(name: "nutrition_carbohydrate", type: "numeric", nullable: false),
                    nutritioncalories = table.Column<decimal>(name: "nutrition_calories", type: "numeric", nullable: false),
                    weightinstock = table.Column<decimal>(name: "weight_in_stock", type: "numeric", nullable: false),
                    farmid = table.Column<Guid>(name: "farm_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_feeds_farms_farm_id",
                        column: x => x.farmid,
                        principalSchema: "feeding",
                        principalTable: "farms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feed_plans",
                schema: "feeding",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    weightdispensed = table.Column<decimal>(name: "weight_dispensed", type: "numeric(18,2)", nullable: false),
                    weighteaten = table.Column<decimal>(name: "weight_eaten", type: "numeric(18,2)", nullable: false),
                    dispensedate = table.Column<DateTime>(name: "dispense_date", type: "timestamp with time zone", nullable: false),
                    fixationdate = table.Column<DateTime>(name: "fixation_date", type: "timestamp with time zone", nullable: false),
                    animalid = table.Column<Guid>(name: "animal_id", type: "uuid", nullable: false),
                    feedid = table.Column<Guid>(name: "feed_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feed_plans", x => x.id);
                    table.ForeignKey(
                        name: "fk_feed_plans_animals_animal_id",
                        column: x => x.animalid,
                        principalSchema: "feeding",
                        principalTable: "animals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_feed_plans_feeds_feed_id",
                        column: x => x.feedid,
                        principalSchema: "feeding",
                        principalTable: "feeds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_animals_farm_id",
                schema: "feeding",
                table: "animals",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "ix_animals_id",
                schema: "feeding",
                table: "animals",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_farms_id",
                schema: "feeding",
                table: "farms",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_feed_plans_animal_id",
                schema: "feeding",
                table: "feed_plans",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "ix_feed_plans_feed_id",
                schema: "feeding",
                table: "feed_plans",
                column: "feed_id");

            migrationBuilder.CreateIndex(
                name: "ix_feeds_farm_id",
                schema: "feeding",
                table: "feeds",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "ix_feeds_id",
                schema: "feeding",
                table: "feeds",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feed_plans",
                schema: "feeding");

            migrationBuilder.DropTable(
                name: "animals",
                schema: "feeding");

            migrationBuilder.DropTable(
                name: "feeds",
                schema: "feeding");

            migrationBuilder.DropTable(
                name: "farms",
                schema: "feeding");
        }
    }
}
