using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentAmendmentTracker.Web.Data.Migrations
{
    public partial class AddDocumentDiaryFeesLessonsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaryEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatterId = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VenueRegistry = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Judge = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ArbitratorMediatorConciliator = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PartiesRequiredToAttend = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InternalAttendee = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExternalCounselAttending = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PurposeIssue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EventStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OutcomeRulingDirection = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    NextActionArising = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ActionOwner = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ActionDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaryEvents_Matters_MatterId",
                        column: x => x.MatterId,
                        principalTable: "Matters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatterId = table.Column<int>(type: "int", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartyReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DocumentTitle = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateOfDocument = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFiledServed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FilingResponseDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreparedFiledBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ServedOn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ModeOfService = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhysicalFileBoxRef = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ElectronicFolderLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Confidentiality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OriginalHeldBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Matters_MatterId",
                        column: x => x.MatterId,
                        principalTable: "Matters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatterId = table.Column<int>(type: "int", nullable: false),
                    LawFirmServiceProvider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FeeNoteInvoiceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodCovered = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DescriptionOfWork = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ProfessionalFees = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Disbursements = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    WithholdingTax = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ApprovedBudgetForStage = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PoRequisitionNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeNotes_Matters_MatterId",
                        column: x => x.MatterId,
                        principalTable: "Matters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatterId = table.Column<int>(type: "int", nullable: false),
                    DateLogged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoggedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LessonType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StageInLifecycle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrimaryRootCause = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SecondaryRootCause = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WhatHappened = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    ConsequenceWhyItMattered = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Avoidability = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CostAttributable = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InstrumentToChange = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SpecificPolicyAffected = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    RecommendedChange = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ActionOwnerFunction = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ActionOwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetImplementationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateImplemented = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EvidenceOfImplementation = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EffectivenessReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectivenessVerdict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HasIssueRecurred = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OtherMattersShowingSameIssue = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ApprovedByGovernanceForum = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Confidentiality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Matters_MatterId",
                        column: x => x.MatterId,
                        principalTable: "Matters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiaryEvents_EventNo",
                table: "DiaryEvents",
                column: "EventNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiaryEvents_MatterId",
                table: "DiaryEvents",
                column: "MatterId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocNo",
                table: "Documents",
                column: "DocNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_MatterId",
                table: "Documents",
                column: "MatterId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeNotes_MatterId",
                table: "FeeNotes",
                column: "MatterId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeNotes_RefNo",
                table: "FeeNotes",
                column: "RefNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LessonNo",
                table: "Lessons",
                column: "LessonNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_MatterId",
                table: "Lessons",
                column: "MatterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaryEvents");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "FeeNotes");

            migrationBuilder.DropTable(
                name: "Lessons");
        }
    }
}
