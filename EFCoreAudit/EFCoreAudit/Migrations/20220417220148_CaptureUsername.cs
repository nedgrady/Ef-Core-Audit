using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreAudit.Migrations
{
    public partial class CaptureUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Unknown User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Employee")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EmployeeHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null);
        }
    }
}
