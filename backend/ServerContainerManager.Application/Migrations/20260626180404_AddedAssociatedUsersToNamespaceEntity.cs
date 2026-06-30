using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerContainerManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedAssociatedUsersToNamespaceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserNamespace_AspNetUsers_AppUserId",
                table: "AppUserNamespace");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "AppUserNamespace",
                newName: "AppUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserNamespace_AspNetUsers_AppUsersId",
                table: "AppUserNamespace",
                column: "AppUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserNamespace_AspNetUsers_AppUsersId",
                table: "AppUserNamespace");

            migrationBuilder.RenameColumn(
                name: "AppUsersId",
                table: "AppUserNamespace",
                newName: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserNamespace_AspNetUsers_AppUserId",
                table: "AppUserNamespace",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
