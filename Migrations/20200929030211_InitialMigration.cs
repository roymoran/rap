using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RAP.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RedditUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(nullable: false),
                    RedditUsername = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    Scope = table.Column<string>(nullable: true),
                    TokenExpiresAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedditUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostRecurrences",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Subreddits = table.Column<string>(nullable: true),
                    IntervalSeconds = table.Column<ulong>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    RedditUserId = table.Column<Guid>(nullable: false),
                    LastPost = table.Column<DateTimeOffset>(nullable: false),
                    NextPost = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostRecurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostRecurrences_RedditUsers_RedditUserId",
                        column: x => x.RedditUserId,
                        principalTable: "RedditUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostRecurrences_RedditUserId",
                table: "PostRecurrences",
                column: "RedditUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostRecurrences");

            migrationBuilder.DropTable(
                name: "RedditUsers");
        }
    }
}
