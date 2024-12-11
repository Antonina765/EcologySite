using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecology.Data.Migrations
{
    /// <inheritdoc />
    public partial class LikesForPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEcologyLikesData",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EcologyDataId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEcologyLikesData", x => new { x.UserId, x.EcologyDataId });
                    table.ForeignKey(
                        name: "FK_UserEcologyLikesData_Ecologies_EcologyDataId",
                        column: x => x.EcologyDataId,
                        principalTable: "Ecologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEcologyLikesData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEcologyLikesData_EcologyDataId",
                table: "UserEcologyLikesData",
                column: "EcologyDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEcologyLikesData");
        }
    }
}
