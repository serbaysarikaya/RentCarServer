using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class i_chanded_IsForgotPasswordComplated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName_value",
                table: "Users",
                newName: "UserName_Value");

            migrationBuilder.RenameColumn(
                name: "LastName_value",
                table: "Users",
                newName: "LastName_Value");

            migrationBuilder.RenameColumn(
                name: "FullName_value",
                table: "Users",
                newName: "FullName_Value");

            migrationBuilder.RenameColumn(
                name: "FirstName_value",
                table: "Users",
                newName: "FirstName_Value");

            migrationBuilder.RenameColumn(
                name: "Email_value",
                table: "Users",
                newName: "Email_Value");

            migrationBuilder.AlterColumn<bool>(
                name: "IsForgotPasswordComplated_Value",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName_Value",
                table: "Users",
                newName: "UserName_value");

            migrationBuilder.RenameColumn(
                name: "LastName_Value",
                table: "Users",
                newName: "LastName_value");

            migrationBuilder.RenameColumn(
                name: "FullName_Value",
                table: "Users",
                newName: "FullName_value");

            migrationBuilder.RenameColumn(
                name: "FirstName_Value",
                table: "Users",
                newName: "FirstName_value");

            migrationBuilder.RenameColumn(
                name: "Email_Value",
                table: "Users",
                newName: "Email_value");

            migrationBuilder.AlterColumn<bool>(
                name: "IsForgotPasswordComplated_Value",
                table: "Users",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
