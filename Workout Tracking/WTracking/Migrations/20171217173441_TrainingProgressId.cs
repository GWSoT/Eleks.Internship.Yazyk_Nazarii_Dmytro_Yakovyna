using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WTracking.Migrations
{
    public partial class TrainingProgressId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_TrainingProgress_TrainingProgressId",
                table: "Profile");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingProgressId",
                table: "Profile",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_TrainingProgress_TrainingProgressId",
                table: "Profile",
                column: "TrainingProgressId",
                principalTable: "TrainingProgress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_TrainingProgress_TrainingProgressId",
                table: "Profile");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingProgressId",
                table: "Profile",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_TrainingProgress_TrainingProgressId",
                table: "Profile",
                column: "TrainingProgressId",
                principalTable: "TrainingProgress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
