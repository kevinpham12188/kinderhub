using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KinderHub.Enrollment.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "classrooms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    age_group = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_classrooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "children",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    enrollment_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    classroom_id = table.Column<Guid>(type: "uuid", nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_potty_trained = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_children", x => x.id);
                    table.ForeignKey(
                        name: "fk_children_classrooms_classroom_id",
                        column: x => x.classroom_id,
                        principalTable: "classrooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "classroom_teachers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    classroom_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_classroom_teachers", x => x.id);
                    table.ForeignKey(
                        name: "fk_classroom_teachers_classrooms_classroom_id",
                        column: x => x.classroom_id,
                        principalTable: "classrooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "waitlists",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    child_id = table.Column<Guid>(type: "uuid", nullable: false),
                    age_group = table.Column<int>(type: "integer", nullable: false),
                    requested_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_waitlists", x => x.id);
                    table.ForeignKey(
                        name: "fk_waitlists_children_child_id",
                        column: x => x.child_id,
                        principalTable: "children",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_children_classroom_id",
                table: "children",
                column: "classroom_id");

            migrationBuilder.CreateIndex(
                name: "ix_children_parent_id",
                table: "children",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_classroom_teachers_classroom_id_teacher_id",
                table: "classroom_teachers",
                columns: new[] { "classroom_id", "teacher_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_waitlists_age_group_status_requested_date",
                table: "waitlists",
                columns: new[] { "age_group", "status", "requested_date" });

            migrationBuilder.CreateIndex(
                name: "ix_waitlists_child_id",
                table: "waitlists",
                column: "child_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classroom_teachers");

            migrationBuilder.DropTable(
                name: "waitlists");

            migrationBuilder.DropTable(
                name: "children");

            migrationBuilder.DropTable(
                name: "classrooms");
        }
    }
}
