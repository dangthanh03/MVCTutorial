using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunGroopWebApp.Migrations
{
    /// <inheritdoc />
    public partial class userIdAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
              name: "FK_AspNetUsers_Addresses_AddressId",
              table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
           name: "AddressId",
           table: "AspNetUsers",
           type: "int",
           nullable: true,
           defaultValue: 0,
           oldClrType: typeof(int),
           oldType: "int",
           oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
