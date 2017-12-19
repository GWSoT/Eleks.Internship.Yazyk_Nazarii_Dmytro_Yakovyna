using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WTracking.Migrations
{
    public partial class TrainingProgressIdRenew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TrainingProgress",
                newName: "TrainingProgressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainingProgressId",
                table: "TrainingProgress",
                newName: "Id");
        }
    }
}
