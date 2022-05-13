using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Minibank.Data.Migrations
{
    public partial class Minibank1_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bank_accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount_of_money = table.Column<decimal>(type: "numeric", nullable: false),
                    currency = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    opening_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    closing_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bank_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    to_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount_of_money = table.Column<decimal>(type: "numeric", nullable: false),
                    currency = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bank_accounts");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
