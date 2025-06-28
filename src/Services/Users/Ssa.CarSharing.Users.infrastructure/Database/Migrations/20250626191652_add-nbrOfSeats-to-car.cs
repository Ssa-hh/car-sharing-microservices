using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ssa.CarSharing.Users.infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class addnbrOfSeatstocar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "NumberOfSeats",
                table: "Cars",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfSeats",
                table: "Cars");
        }
    }
}
