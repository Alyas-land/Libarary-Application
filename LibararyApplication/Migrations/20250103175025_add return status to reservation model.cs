using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibararyApplication.Migrations
{
    /// <inheritdoc />
    public partial class addreturnstatustoreservationmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReturnStatus",
                table: "Reservations",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnStatus",
                table: "Reservations");
        }
    }
}
