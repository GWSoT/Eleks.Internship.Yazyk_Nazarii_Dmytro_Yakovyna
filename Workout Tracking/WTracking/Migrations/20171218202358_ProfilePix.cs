using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WTracking.Migrations
{
    public partial class ProfilePix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "Profile",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "Profile",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
