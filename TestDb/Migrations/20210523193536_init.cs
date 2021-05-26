using Microsoft.EntityFrameworkCore.Migrations;

namespace TestDbModel.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbo.Subdivisions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Subdivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.Subdivisions_dbo.Subdivisions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "dbo.Subdivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dbo.Subdivisions_ParentId",
                table: "dbo.Subdivisions",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbo.Subdivisions");
        }
    }
}
