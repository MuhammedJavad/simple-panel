using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeveralIndecies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1L,
                column: "HashedPassword",
                value: "VrPzWJQ3r3zGM6oAACWpyK4qIq4mGZFDrATKf2bO9wg=");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1L,
                column: "HashedPassword",
                value: "Uiz32xHtFM7Iti+XVOavOKHB5zzbPMQIFeiClRbUdJs=");
        }
    }
}
