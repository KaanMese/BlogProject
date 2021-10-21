using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogProject.Migrations
{
    public partial class ContentAndCommentRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContentID",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ContentID",
                table: "Comments",
                column: "ContentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Contents_ContentID",
                table: "Comments",
                column: "ContentID",
                principalTable: "Contents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Contents_ContentID",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ContentID",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ContentID",
                table: "Comments");
        }
    }
}
