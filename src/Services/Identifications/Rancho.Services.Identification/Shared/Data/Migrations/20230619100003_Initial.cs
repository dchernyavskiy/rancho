using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rancho.Services.Identification.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identification");

            migrationBuilder.CreateTable(
                name: "farms",
                schema: "identification",
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
                schema: "identification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    species = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    birthdate = table.Column<DateTime>(name: "birth_date", type: "timestamp with time zone", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    eartagnumber = table.Column<string>(name: "ear_tag_number", type: "text", nullable: false),
                    farmid = table.Column<Guid>(name: "farm_id", type: "uuid", nullable: false),
                    rfidtagid = table.Column<Guid>(name: "rfid_tag_id", type: "uuid", nullable: true),
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
                        principalSchema: "identification",
                        principalTable: "farms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rfid_tags",
                schema: "identification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    animalid = table.Column<Guid>(name: "animal_id", type: "uuid", nullable: true),
                    farmid = table.Column<Guid>(name: "farm_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rfid_tags", x => x.id);
                    table.ForeignKey(
                        name: "fk_rfid_tags_animals_animal_id",
                        column: x => x.animalid,
                        principalSchema: "identification",
                        principalTable: "animals",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_rfid_tags_farms_farm_id",
                        column: x => x.farmid,
                        principalSchema: "identification",
                        principalTable: "farms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_animals_farm_id",
                schema: "identification",
                table: "animals",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "ix_animals_id",
                schema: "identification",
                table: "animals",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_farms_id",
                schema: "identification",
                table: "farms",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_rfid_tags_animal_id",
                schema: "identification",
                table: "rfid_tags",
                column: "animal_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_rfid_tags_farm_id",
                schema: "identification",
                table: "rfid_tags",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "ix_rfid_tags_id",
                schema: "identification",
                table: "rfid_tags",
                column: "id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rfid_tags",
                schema: "identification");

            migrationBuilder.DropTable(
                name: "animals",
                schema: "identification");

            migrationBuilder.DropTable(
                name: "farms",
                schema: "identification");
        }
    }
}
