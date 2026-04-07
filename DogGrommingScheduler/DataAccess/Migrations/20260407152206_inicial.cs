using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetGroomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetGroomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetGroomers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PetGroomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_PetGroomers_PetGroomerId",
                        column: x => x.PetGroomerId,
                        principalTable: "PetGroomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TimeSlot = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    PetSize = table.Column<string>(type: "text", nullable: false),
                    IsCanceled = table.Column<bool>(type: "boolean", nullable: false),
                    ReminderJobId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserves_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserves_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "G-01", 0, "CONC_01", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), "ana@canina.com", true, false, null, "Ana Martínez", "ANA@CANINA.COM", "ANA@CANINA.COM", null, null, false, "STATIC_01", false, "ana@canina.com" },
                    { "G-02", 0, "CONC_02", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), "carlos@canina.com", true, false, null, "Carlos Pérez", "CARLOS@CANINA.COM", "CARLOS@CANINA.COM", null, null, false, "STATIC_02", false, "carlos@canina.com" },
                    { "G-03", 0, "CONC_03", new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), "lucia@canina.com", true, false, null, "Lucía Gómez", "LUCIA@CANINA.COM", "LUCIA@CANINA.COM", null, null, false, "STATIC_03", false, "lucia@canina.com" }
                });

            migrationBuilder.InsertData(
                table: "PetGroomers",
                columns: new[] { "Id", "UserId" },
                values: new object[,]
                {
                    { new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), "G-03" },
                    { new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), "G-02" },
                    { new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), "G-01" }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "Id", "Date", "EndTime", "PetGroomerId", "StartTime" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 7, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), new DateTime(2026, 4, 7, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 7, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), new DateTime(2026, 4, 7, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2026, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 7, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), new DateTime(2026, 4, 7, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000004"), new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 8, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), new DateTime(2026, 4, 8, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000005"), new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 8, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), new DateTime(2026, 4, 8, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000006"), new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 8, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), new DateTime(2026, 4, 8, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000007"), new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), new DateTime(2026, 4, 9, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000008"), new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), new DateTime(2026, 4, 9, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000009"), new DateTime(2026, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 9, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), new DateTime(2026, 4, 9, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 10, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), new DateTime(2026, 4, 10, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 10, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), new DateTime(2026, 4, 10, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 10, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), new DateTime(2026, 4, 10, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000013"), new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 11, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), new DateTime(2026, 4, 11, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000014"), new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 11, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), new DateTime(2026, 4, 11, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000015"), new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 11, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), new DateTime(2026, 4, 11, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000016"), new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 12, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), new DateTime(2026, 4, 12, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000017"), new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 12, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), new DateTime(2026, 4, 12, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000018"), new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 12, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), new DateTime(2026, 4, 12, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000019"), new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 13, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), new DateTime(2026, 4, 13, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000020"), new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 13, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), new DateTime(2026, 4, 13, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000021"), new DateTime(2026, 4, 13, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 13, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), new DateTime(2026, 4, 13, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000022"), new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 14, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), new DateTime(2026, 4, 14, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000023"), new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 14, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), new DateTime(2026, 4, 14, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000024"), new DateTime(2026, 4, 14, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 14, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), new DateTime(2026, 4, 14, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000025"), new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 15, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4fb87e5b-89e4-488d-8f20-49299c1c3d53"), new DateTime(2026, 4, 15, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000026"), new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 15, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("4d1d2047-8020-473b-a293-35f6d43b0314"), new DateTime(2026, 4, 15, 6, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000027"), new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 15, 14, 0, 0, 0, DateTimeKind.Utc), new Guid("2864b026-1c02-4e02-8a77-944aa4fa7e87"), new DateTime(2026, 4, 15, 6, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UserId",
                table: "Clients",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetGroomers_UserId",
                table: "PetGroomers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_ClientId",
                table: "Reserves",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserves_ScheduleId",
                table: "Reserves",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_PetGroomerId",
                table: "Schedules",
                column: "PetGroomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Reserves");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "PetGroomers");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
