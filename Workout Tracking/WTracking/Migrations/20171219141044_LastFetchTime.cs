using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WTracking.Migrations
{
    public partial class LastFetchTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageStepCountForThisWeek",
                table: "Profile");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastFetchTime",
                table: "Profile",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFetchTime",
                table: "Profile");

            migrationBuilder.AddColumn<int>(
                name: "AverageStepCountForThisWeek",
                table: "Profile",
                nullable: false,
                defaultValue: 0);
        }
    }
}
