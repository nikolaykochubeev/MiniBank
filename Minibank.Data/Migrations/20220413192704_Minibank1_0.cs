using Microsoft.EntityFrameworkCore.Migrations;

namespace Minibank.Data.Migrations
{
    public partial class Minibank1_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transaction",
                table: "transaction");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bank_account",
                table: "bankAccount");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "transaction",
                newName: "transactions");

            migrationBuilder.RenameTable(
                name: "bankAccount",
                newName: "bank_accounts");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transactions",
                table: "transactions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bank_accounts",
                table: "bank_accounts",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_transactions",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bank_accounts",
                table: "bank_accounts");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "transaction");

            migrationBuilder.RenameTable(
                name: "bank_accounts",
                newName: "bankAccount");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_transaction",
                table: "transaction",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bank_account",
                table: "bankAccount",
                column: "id");
        }
    }
}
