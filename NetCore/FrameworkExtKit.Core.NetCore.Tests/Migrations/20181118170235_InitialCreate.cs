using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FrameworkExtKit.Core.NetCore.Tests.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManyToManyEntityAs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true, maxLength: 56)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManyToManyEntityAs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManyToManyEntityBs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true, maxLength: 56),
                    Category = table.Column<string>(nullable: true, maxLength: 56)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManyToManyEntityBs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManyToManyEntityABMappings",
                columns: table => new
                {
                    ManyToManyEntityAId = table.Column<int>(nullable: false),
                    ManyToManyEntityBId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManyToManyEntityABMappings", x => new { x.ManyToManyEntityAId, x.ManyToManyEntityBId });
                    table.ForeignKey(
                        name: "FK_ManyToManyEntityABMappings_ManyToManyEntityAs_ManyToManyEntityAId",
                        column: x => x.ManyToManyEntityAId,
                        principalTable: "ManyToManyEntityAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManyToManyEntityABMappings_ManyToManyEntityBs_ManyToManyEntityBId",
                        column: x => x.ManyToManyEntityBId,
                        principalTable: "ManyToManyEntityBs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManyToManyEntityABMappings_ManyToManyEntityBId",
                table: "ManyToManyEntityABMappings",
                column: "ManyToManyEntityBId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManyToManyEntityABMappings");

            migrationBuilder.DropTable(
                name: "ManyToManyEntityAs");

            migrationBuilder.DropTable(
                name: "ManyToManyEntityBs");
        }
    }
}
