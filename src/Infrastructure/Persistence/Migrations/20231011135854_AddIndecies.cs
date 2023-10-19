using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIndecies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                table: "Vendor",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1L,
                column: "HashedPassword",
                value: "Uiz32xHtFM7Iti+XVOavOKHB5zzbPMQIFeiClRbUdJs=");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_ClientId",
                table: "Vendor",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_ClientSecret",
                table: "Vendor",
                column: "ClientSecret",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_UserName",
                table: "Vendor",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_VendorGuid",
                table: "Vendor",
                column: "VendorGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_VendorId",
                table: "Vendor",
                column: "VendorId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendor_ClientId",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_Vendor_ClientSecret",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_Vendor_UserName",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_Vendor_VendorGuid",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_Vendor_VendorId",
                table: "Vendor");

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                table: "Vendor",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: -1L,
                column: "HashedPassword",
                value: "OSTBU3ZzKm/bfDCVwzQFJieL9hB54c00qfmjdxJBIoM=");
        }
    }
}
