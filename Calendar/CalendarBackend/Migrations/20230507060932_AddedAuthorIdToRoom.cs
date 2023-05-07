using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedAuthorIdToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "rooms",
                keyColumn: "id",
                keyValue: 1,
                column: "AuthorId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "rooms",
                keyColumn: "id",
                keyValue: 2,
                column: "AuthorId",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "rooms");
        }
    }
}
