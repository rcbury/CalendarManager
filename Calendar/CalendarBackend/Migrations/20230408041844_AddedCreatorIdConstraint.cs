using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalendarBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatorIdConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tasks_CreatorId",
                table: "tasks",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "creator_id",
                table: "tasks",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "creator_id",
                table: "tasks");

            migrationBuilder.DropIndex(
                name: "IX_tasks_CreatorId",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "tasks");
        }
    }
}
