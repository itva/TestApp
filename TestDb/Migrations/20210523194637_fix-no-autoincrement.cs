using Microsoft.EntityFrameworkCore.Migrations;

namespace TestDbModel.Migrations
{
    public partial class fixnoautoincrement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "dbo.Subdivisions",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "dbo.Subdivisions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
