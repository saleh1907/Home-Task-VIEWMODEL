using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProniaTwo.Migrations
{
    /// <inheritdoc />
    public partial class addedProductImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Productts");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Shippings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "HoverImagePath",
                table: "Productts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainImagePath",
                table: "Productts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProducttId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Productts_ProducttId",
                        column: x => x.ProducttId,
                        principalTable: "Productts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProducttId",
                table: "ProductImages",
                column: "ProducttId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropColumn(
                name: "HoverImagePath",
                table: "Productts");

            migrationBuilder.DropColumn(
                name: "MainImagePath",
                table: "Productts");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Shippings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Productts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
