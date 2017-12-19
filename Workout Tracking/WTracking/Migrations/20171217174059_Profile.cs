using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WTracking.Migrations
{
    public partial class Profile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profile_TrainingProgressId",
                table: "Profile");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_TrainingProgressId",
                table: "Profile",
                column: "TrainingProgressId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profile_TrainingProgressId",
                table: "Profile");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_TrainingProgressId",
                table: "Profile",
                column: "TrainingProgressId");
        }
    }
}
