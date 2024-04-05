using DLMS.Core.Constants;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DLMS.EF.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] {Guid.NewGuid().ToString(), RoleTypes.Admin,
                    RoleTypes.Admin.ToUpper(), Guid.NewGuid().ToString()});

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] {Guid.NewGuid().ToString(), RoleTypes.User,
                    RoleTypes.User.ToUpper(), Guid.NewGuid().ToString()});
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Drop from [AspNetRoles]");
        }
    }
}
