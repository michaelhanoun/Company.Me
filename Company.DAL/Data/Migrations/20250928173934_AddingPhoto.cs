using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Employee");
        }
    }
}
