using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaqServices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FAQ_GRADE",
                columns: table => new
                {
                    PK = table.Column<string>(type: "varchar(255)", nullable: false),
                    GradeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQ_GRADE", x => x.PK);
                });

            migrationBuilder.CreateTable(
                name: "FAQ_QUESTION",
                columns: table => new
                {
                    PK = table.Column<string>(type: "varchar(255)", nullable: false),
                    GradeId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    QuestionText = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    QuestionTextAr = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ImageBlobUrl = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQ_QUESTION", x => x.PK);
                    table.ForeignKey(
                        name: "FK_FAQ_QUESTION_FAQ_GRADE_GradeId",
                        column: x => x.GradeId,
                        principalTable: "FAQ_GRADE",
                        principalColumn: "PK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FAQ_ANSWER",
                columns: table => new
                {
                    PK = table.Column<string>(type: "varchar(255)", nullable: false),
                    QuestionId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    AnswerText = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    AnswerTextAr = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ImageBlobUrl = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQ_ANSWER", x => x.PK);
                    table.ForeignKey(
                        name: "FK_FAQ_ANSWER_FAQ_QUESTION_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "FAQ_QUESTION",
                        principalColumn: "PK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_ANSWER_IsActive",
                table: "FAQ_ANSWER",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_ANSWER_IsCorrect",
                table: "FAQ_ANSWER",
                column: "IsCorrect");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_ANSWER_QuestionId",
                table: "FAQ_ANSWER",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_ANSWER_SortOrder",
                table: "FAQ_ANSWER",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_GRADE_IsActive",
                table: "FAQ_GRADE",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_GRADE_SortOrder",
                table: "FAQ_GRADE",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_QUESTION_GradeId",
                table: "FAQ_QUESTION",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_QUESTION_IsActive",
                table: "FAQ_QUESTION",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_QUESTION_SortOrder",
                table: "FAQ_QUESTION",
                column: "SortOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQ_ANSWER");

            migrationBuilder.DropTable(
                name: "FAQ_QUESTION");

            migrationBuilder.DropTable(
                name: "FAQ_GRADE");
        }
    }
}
