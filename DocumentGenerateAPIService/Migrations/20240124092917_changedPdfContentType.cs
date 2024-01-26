using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentGenerateAPIService.Migrations
{
    /// <inheritdoc />
    public partial class changedPdfContentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HtmlContent",
                table: "Files");

            migrationBuilder.AddColumn<byte[]>(
                name: "PdfContent",
                table: "Files",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfContent",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "HtmlContent",
                table: "Files",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
