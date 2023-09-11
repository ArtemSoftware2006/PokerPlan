using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserGroup5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Groups_GroupId1",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_GroupId1",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Votes");

            migrationBuilder.AlterColumn<Guid>(
                name: "GroupId",
                table: "Votes",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_GroupId",
                table: "Votes",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Groups_GroupId",
                table: "Votes",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Groups_GroupId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_GroupId",
                table: "Votes");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Votes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId1",
                table: "Votes",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Votes_GroupId1",
                table: "Votes",
                column: "GroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Groups_GroupId1",
                table: "Votes",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
