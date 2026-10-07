using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentAmendmentTracker.Web.Data.Migrations
{
    public partial class AddMattersAndParties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatterId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityEmployer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ForumType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CourtTribunal = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CauseNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NamesOfParties = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    OurRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NatureOfMatter = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfCommencement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedDateOfCompletion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LawFirmWithConduct = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    InternalLawyerResponsible = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BriefOutline = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SumClaimed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LikelihoodOfAdverseOutcome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProvisionRecommended = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StatusOfMatter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NextDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextStepAction = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Judge = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ArbitratorMediatorConciliator = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EstimatedLegalFeesToFinalisation = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LegalFeesIncurredToDate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RiskRating = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RelatedConsolidatedWith = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LeadMatterInGroup = table.Column<bool>(type: "bit", nullable: false),
                    PrivilegedConfidential = table.Column<bool>(type: "bit", nullable: false),
                    DateClosed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OutcomeResult = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PostMatterReviewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostMatterReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreventabilityAssessment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartyNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatterId = table.Column<int>(type: "int", nullable: false),
                    Side = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartyOrder = table.Column<int>(type: "int", nullable: false),
                    PartyRoleTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PartyName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PartyType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsGroupEntity = table.Column<bool>(type: "bit", nullable: false),
                    EmployeePayrollNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NrcPassportRegNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RepresentedBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IndividualSumClaimed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateCeased = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartyStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ServiceAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parties_Matters_MatterId",
                        column: x => x.MatterId,
                        principalTable: "Matters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matters_MatterId",
                table: "Matters",
                column: "MatterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_MatterId",
                table: "Parties",
                column: "MatterId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_PartyNo",
                table: "Parties",
                column: "PartyNo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "Matters");
        }
    }
}
