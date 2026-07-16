using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i_chanded_IsForgotPasswordComplated_field_on_CheckForgotPasswordCode2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
          name: "ForgotPasswordId_Value",
          table: "Users",
          newName: "ForgotPasswordCode_Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.RenameColumn(
          name: "ForgotPasswordCode_Value",
          table: "Users",
          newName: "ForgotPasswordId_Value");
        }
    }
}
