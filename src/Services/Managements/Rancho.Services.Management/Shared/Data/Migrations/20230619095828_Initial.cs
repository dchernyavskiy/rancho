using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rancho.Services.Management.Shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "management");

            migrationBuilder.CreateTable(
                name: "farms",
                schema: "management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
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
                schema: "management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    species = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                        principalSchema: "management",
                        principalTable: "farms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "farmers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstname = table.Column<string>(name: "first_name", type: "character varying(30)", maxLength: 30, nullable: false),
                    lastname = table.Column<string>(name: "last_name", type: "character varying(30)", maxLength: 30, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phonenumber = table.Column<string>(name: "phone_number", type: "text", nullable: false),
                    farmid = table.Column<Guid>(name: "farm_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_farmers", x => x.id);
                    table.ForeignKey(
                        name: "fk_farmers_farms_farm_id",
                        column: x => x.farmid,
                        principalSchema: "management",
                        principalTable: "farms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "works",
                schema: "management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    farmerid = table.Column<Guid>(name: "farmer_id", type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createdby = table.Column<int>(name: "created_by", type: "integer", nullable: true),
                    originalversion = table.Column<long>(name: "original_version", type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_works", x => x.id);
                    table.ForeignKey(
                        name: "fk_works_farmers_farmer_id",
                        column: x => x.farmerid,
                        principalTable: "farmers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_animals_farm_id",
                schema: "management",
                table: "animals",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "ix_animals_id",
                schema: "management",
                table: "animals",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_animals_id_ear_tag_number_farm_id",
                schema: "management",
                table: "animals",
                columns: new[] { "id", "ear_tag_number", "farm_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_farmers_farm_id",
                table: "farmers",
                column: "farm_id");

            migrationBuilder.CreateIndex(
                name: "ix_farmers_id",
                table: "farmers",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_farms_id",
                schema: "management",
                table: "farms",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_works_farmer_id",
                schema: "management",
                table: "works",
                column: "farmer_id");

            migrationBuilder.CreateIndex(
                name: "ix_works_id",
                schema: "management",
                table: "works",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "animals",
                schema: "management");

            migrationBuilder.DropTable(
                name: "works",
                schema: "management");

            migrationBuilder.DropTable(
                name: "farmers");

            migrationBuilder.DropTable(
                name: "farms",
                schema: "management");
        }
    }
}
