using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace docker_compose_api_mssql.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Post",
                columns: new[] { "Id", "Content", "Title" },
                values: new object[,]
                {
                    { 1, "6f2f1cae-af15-40ec-99fe-630cec26628e", "本教程由Siegrain傾情奉獻?️" },
                    { 2, "f4f7ae03-7825-459c-93d5-a4b038609833", "感謝大家關註~" },
                    { 3, "2e12945e-12c0-40be-8ff1-5542007c885a", "博客地址為 http://siegrain.wang" },
                    { 4, "d3a9b305-00a4-4f9b-b1cc-428b379e9180", "本教程Github地址為 https://github.com/Seanwong933/.NET-Core-with-Docker" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Post");
        }
    }
}
