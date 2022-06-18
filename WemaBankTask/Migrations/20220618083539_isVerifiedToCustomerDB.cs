using Microsoft.EntityFrameworkCore.Migrations;

namespace WemaBankTask.Migrations
{
    public partial class isVerifiedToCustomerDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Customers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Customers");
        }
    }
}
