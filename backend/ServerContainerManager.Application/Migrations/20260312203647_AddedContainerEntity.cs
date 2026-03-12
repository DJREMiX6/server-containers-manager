using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerContainerManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddedContainerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContainerNamespace",
                columns: table => new
                {
                    ContainerId = table.Column<string>(type: "TEXT", nullable: false),
                    NamespacesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerNamespace", x => new { x.ContainerId, x.NamespacesId });
                    table.ForeignKey(
                        name: "FK_ContainerNamespace_Containers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Containers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContainerNamespace_Namespaces_NamespacesId",
                        column: x => x.NamespacesId,
                        principalTable: "Namespaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContainerNamespace_NamespacesId",
                table: "ContainerNamespace",
                column: "NamespacesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContainerNamespace");

            migrationBuilder.DropTable(
                name: "Containers");
        }
    }
}
