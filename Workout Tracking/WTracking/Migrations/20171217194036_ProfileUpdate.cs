using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WTracking.Migrations
{
    public partial class ProfileUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_TrainingProgress_TrainingProgressId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_TrainingProgressId",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "TrainingProgressId",
                table: "Profile");

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "TrainingProgress",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FifthDayProgress",
                table: "Profile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstDayProgress",
                table: "Profile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FourthDayProgress",
                table: "Profile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FridaySchedule",
                table: "Profile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "MondaySchedule",
                table: "Profile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SaturdaySchedule",
                table: "Profile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SecondDayProgress",
                table: "Profile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeventhDayProgress",
                table: "Profile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SixthDayProgress",
                table: "Profile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SundaySchedule",
                table: "Profile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ThirdDayProgress",
                table: "Profile",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThursdaySchedule",
                table: "Profile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TuesdaySchedule",
                table: "Profile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WednesdaySchedule",
                table: "Profile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProgress_ProfileId",
                table: "TrainingProgress",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingProgress_Profile_ProfileId",
                table: "TrainingProgress",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingProgress_Profile_ProfileId",
                table: "TrainingProgress");

            migrationBuilder.DropIndex(
                name: "IX_TrainingProgress_ProfileId",
                table: "TrainingProgress");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "TrainingProgress");

            migrationBuilder.DropColumn(
                name: "FifthDayProgress",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "FirstDayProgress",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "FourthDayProgress",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "FridaySchedule",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "MondaySchedule",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "SaturdaySchedule",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "SecondDayProgress",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "SeventhDayProgress",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "SixthDayProgress",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "SundaySchedule",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "ThirdDayProgress",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "ThursdaySchedule",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "TuesdaySchedule",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "WednesdaySchedule",
                table: "Profile");

            migrationBuilder.AddColumn<int>(
                name: "TrainingProgressId",
                table: "Profile",
                nullable: true);

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
    }
}
