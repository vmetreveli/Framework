#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Framework.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "outbox_messages",
            table => new
            {
                id = table.Column<Guid>("uuid", nullable: false),
                data = table.Column<string>("text", nullable: false),
                type = table.Column<string>("text", nullable: false),
                event_id = table.Column<Guid>("uuid", nullable: false),
                event_date = table.Column<DateTime>("timestamp with time zone", nullable: false),
                state = table.Column<int>("integer", nullable: false),
                modified_date = table.Column<DateTime>("timestamp with time zone", nullable: false),
                content = table.Column<string>("text", nullable: false)
            },
            constraints: table => { table.PrimaryKey("pk_outbox_messages", x => x.id); });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "outbox_messages");
    }
}