using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WTracking.Migrations
{
    public partial class TrainingPorgressId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_TrainingProgress_TrainingProgressId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_TrainingProgressId",
                table: "Profile");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingProgressId",
                table: "Profile",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Profile_TrainingProgressId",
                table: "Profile",
                column: "TrainingProgressId",
                unique: true,
                filter: "[TrainingProgressId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_TrainingProgress_TrainingProgressId",
                table: "Profile",
                column: "TrainingProgressId",
                principalTable: "TrainingProgress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_TrainingProgress_TrainingProgressId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_TrainingProgressId",
                table: "Profile");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingProgressId",
                table: "Profile",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profile_TrainingProgressId",
                table: "Profile",
                column: "TrainingProgressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_TrainingProgress_TrainingProgressId",
                table: "Profile",
                column: "TrainingProgressId",
                principalTable: "TrainingProgress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
