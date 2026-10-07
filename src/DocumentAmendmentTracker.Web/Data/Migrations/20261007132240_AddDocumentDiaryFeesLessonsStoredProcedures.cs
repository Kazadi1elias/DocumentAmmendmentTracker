using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentAmendmentTracker.Web.Data.Migrations
{
    public partial class AddDocumentDiaryFeesLessonsStoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ================= Documents =================

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Documents_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, DocNo, MatterId, DocumentType, Direction, PartyReference, DocumentTitle, DateOfDocument,
           DateFiledServed, FilingResponseDeadline, PreparedFiledBy, ServedOn, ModeOfService, DocumentStatus,
           Version, PhysicalFileBoxRef, ElectronicFolderLink, Confidentiality, OriginalHeldBy, Remarks
    FROM dbo.Documents
    ORDER BY DateOfDocument DESC;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Documents_GetByMatterId
    @MatterId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, DocNo, MatterId, DocumentType, Direction, PartyReference, DocumentTitle, DateOfDocument,
           DateFiledServed, FilingResponseDeadline, PreparedFiledBy, ServedOn, ModeOfService, DocumentStatus,
           Version, PhysicalFileBoxRef, ElectronicFolderLink, Confidentiality, OriginalHeldBy, Remarks
    FROM dbo.Documents
    WHERE MatterId = @MatterId
    ORDER BY DateOfDocument DESC;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Documents_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, DocNo, MatterId, DocumentType, Direction, PartyReference, DocumentTitle, DateOfDocument,
           DateFiledServed, FilingResponseDeadline, PreparedFiledBy, ServedOn, ModeOfService, DocumentStatus,
           Version, PhysicalFileBoxRef, ElectronicFolderLink, Confidentiality, OriginalHeldBy, Remarks
    FROM dbo.Documents
    WHERE Id = @Id;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Documents_Create
    @MatterId INT,
    @DocumentType NVARCHAR(100),
    @Direction NVARCHAR(100),
    @PartyReference NVARCHAR(200),
    @DocumentTitle NVARCHAR(500),
    @DateOfDocument DATETIME2,
    @DateFiledServed DATETIME2 = NULL,
    @FilingResponseDeadline DATETIME2 = NULL,
    @PreparedFiledBy NVARCHAR(200),
    @ServedOn NVARCHAR(200),
    @ModeOfService NVARCHAR(100),
    @DocumentStatus NVARCHAR(50),
    @Version NVARCHAR(50),
    @PhysicalFileBoxRef NVARCHAR(200),
    @ElectronicFolderLink NVARCHAR(500),
    @Confidentiality NVARCHAR(50),
    @OriginalHeldBy NVARCHAR(200),
    @Remarks NVARCHAR(2000),
    @Id INT OUTPUT,
    @DocNo NVARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    DECLARE @NextNo INT;
    SELECT @NextNo = ISNULL(MAX(CAST(SUBSTRING(DocNo, 3, 10) AS INT)), 0) + 1
    FROM dbo.Documents WITH (UPDLOCK, HOLDLOCK);
    SET @DocNo = 'D-' + RIGHT('0000' + CAST(@NextNo AS NVARCHAR(4)), 4);

    INSERT INTO dbo.Documents
        (DocNo, MatterId, DocumentType, Direction, PartyReference, DocumentTitle, DateOfDocument,
         DateFiledServed, FilingResponseDeadline, PreparedFiledBy, ServedOn, ModeOfService, DocumentStatus,
         Version, PhysicalFileBoxRef, ElectronicFolderLink, Confidentiality, OriginalHeldBy, Remarks)
    VALUES
        (@DocNo, @MatterId, @DocumentType, @Direction, @PartyReference, @DocumentTitle, @DateOfDocument,
         @DateFiledServed, @FilingResponseDeadline, @PreparedFiledBy, @ServedOn, @ModeOfService, @DocumentStatus,
         @Version, @PhysicalFileBoxRef, @ElectronicFolderLink, @Confidentiality, @OriginalHeldBy, @Remarks);

    SET @Id = SCOPE_IDENTITY();

    COMMIT TRANSACTION;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Documents_Update
    @Id INT,
    @DocumentType NVARCHAR(100),
    @Direction NVARCHAR(100),
    @PartyReference NVARCHAR(200),
    @DocumentTitle NVARCHAR(500),
    @DateOfDocument DATETIME2,
    @DateFiledServed DATETIME2 = NULL,
    @FilingResponseDeadline DATETIME2 = NULL,
    @PreparedFiledBy NVARCHAR(200),
    @ServedOn NVARCHAR(200),
    @ModeOfService NVARCHAR(100),
    @DocumentStatus NVARCHAR(50),
    @Version NVARCHAR(50),
    @PhysicalFileBoxRef NVARCHAR(200),
    @ElectronicFolderLink NVARCHAR(500),
    @Confidentiality NVARCHAR(50),
    @OriginalHeldBy NVARCHAR(200),
    @Remarks NVARCHAR(2000)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Documents
    SET DocumentType = @DocumentType,
        Direction = @Direction,
        PartyReference = @PartyReference,
        DocumentTitle = @DocumentTitle,
        DateOfDocument = @DateOfDocument,
        DateFiledServed = @DateFiledServed,
        FilingResponseDeadline = @FilingResponseDeadline,
        PreparedFiledBy = @PreparedFiledBy,
        ServedOn = @ServedOn,
        ModeOfService = @ModeOfService,
        DocumentStatus = @DocumentStatus,
        Version = @Version,
        PhysicalFileBoxRef = @PhysicalFileBoxRef,
        ElectronicFolderLink = @ElectronicFolderLink,
        Confidentiality = @Confidentiality,
        OriginalHeldBy = @OriginalHeldBy,
        Remarks = @Remarks
    WHERE Id = @Id;
END");

            // ================= Diary & Events =================

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_DiaryEvents_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EventNo, MatterId, EventType, EventDate, Time, VenueRegistry, Judge,
           ArbitratorMediatorConciliator, PartiesRequiredToAttend, InternalAttendee, ExternalCounselAttending,
           PurposeIssue, EventStatus, OutcomeRulingDirection, NextActionArising, ActionOwner, ActionDueDate,
           Remarks
    FROM dbo.DiaryEvents
    ORDER BY EventDate;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_DiaryEvents_GetByMatterId
    @MatterId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EventNo, MatterId, EventType, EventDate, Time, VenueRegistry, Judge,
           ArbitratorMediatorConciliator, PartiesRequiredToAttend, InternalAttendee, ExternalCounselAttending,
           PurposeIssue, EventStatus, OutcomeRulingDirection, NextActionArising, ActionOwner, ActionDueDate,
           Remarks
    FROM dbo.DiaryEvents
    WHERE MatterId = @MatterId
    ORDER BY EventDate;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_DiaryEvents_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EventNo, MatterId, EventType, EventDate, Time, VenueRegistry, Judge,
           ArbitratorMediatorConciliator, PartiesRequiredToAttend, InternalAttendee, ExternalCounselAttending,
           PurposeIssue, EventStatus, OutcomeRulingDirection, NextActionArising, ActionOwner, ActionDueDate,
           Remarks
    FROM dbo.DiaryEvents
    WHERE Id = @Id;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_DiaryEvents_Create
    @MatterId INT,
    @EventType NVARCHAR(100),
    @EventDate DATETIME2,
    @Time NVARCHAR(20),
    @VenueRegistry NVARCHAR(300),
    @Judge NVARCHAR(200),
    @ArbitratorMediatorConciliator NVARCHAR(200),
    @PartiesRequiredToAttend NVARCHAR(500),
    @InternalAttendee NVARCHAR(200),
    @ExternalCounselAttending NVARCHAR(200),
    @PurposeIssue NVARCHAR(1000),
    @EventStatus NVARCHAR(50),
    @OutcomeRulingDirection NVARCHAR(2000),
    @NextActionArising NVARCHAR(1000),
    @ActionOwner NVARCHAR(200),
    @ActionDueDate DATETIME2 = NULL,
    @Remarks NVARCHAR(2000),
    @Id INT OUTPUT,
    @EventNo NVARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    DECLARE @NextNo INT;
    SELECT @NextNo = ISNULL(MAX(CAST(SUBSTRING(EventNo, 3, 10) AS INT)), 0) + 1
    FROM dbo.DiaryEvents WITH (UPDLOCK, HOLDLOCK);
    SET @EventNo = 'E-' + RIGHT('0000' + CAST(@NextNo AS NVARCHAR(4)), 4);

    INSERT INTO dbo.DiaryEvents
        (EventNo, MatterId, EventType, EventDate, Time, VenueRegistry, Judge, ArbitratorMediatorConciliator,
         PartiesRequiredToAttend, InternalAttendee, ExternalCounselAttending, PurposeIssue, EventStatus,
         OutcomeRulingDirection, NextActionArising, ActionOwner, ActionDueDate, Remarks)
    VALUES
        (@EventNo, @MatterId, @EventType, @EventDate, @Time, @VenueRegistry, @Judge, @ArbitratorMediatorConciliator,
         @PartiesRequiredToAttend, @InternalAttendee, @ExternalCounselAttending, @PurposeIssue, @EventStatus,
         @OutcomeRulingDirection, @NextActionArising, @ActionOwner, @ActionDueDate, @Remarks);

    SET @Id = SCOPE_IDENTITY();

    COMMIT TRANSACTION;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_DiaryEvents_Update
    @Id INT,
    @EventType NVARCHAR(100),
    @EventDate DATETIME2,
    @Time NVARCHAR(20),
    @VenueRegistry NVARCHAR(300),
    @Judge NVARCHAR(200),
    @ArbitratorMediatorConciliator NVARCHAR(200),
    @PartiesRequiredToAttend NVARCHAR(500),
    @InternalAttendee NVARCHAR(200),
    @ExternalCounselAttending NVARCHAR(200),
    @PurposeIssue NVARCHAR(1000),
    @EventStatus NVARCHAR(50),
    @OutcomeRulingDirection NVARCHAR(2000),
    @NextActionArising NVARCHAR(1000),
    @ActionOwner NVARCHAR(200),
    @ActionDueDate DATETIME2 = NULL,
    @Remarks NVARCHAR(2000)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.DiaryEvents
    SET EventType = @EventType,
        EventDate = @EventDate,
        Time = @Time,
        VenueRegistry = @VenueRegistry,
        Judge = @Judge,
        ArbitratorMediatorConciliator = @ArbitratorMediatorConciliator,
        PartiesRequiredToAttend = @PartiesRequiredToAttend,
        InternalAttendee = @InternalAttendee,
        ExternalCounselAttending = @ExternalCounselAttending,
        PurposeIssue = @PurposeIssue,
        EventStatus = @EventStatus,
        OutcomeRulingDirection = @OutcomeRulingDirection,
        NextActionArising = @NextActionArising,
        ActionOwner = @ActionOwner,
        ActionDueDate = @ActionDueDate,
        Remarks = @Remarks
    WHERE Id = @Id;
END");

            // ================= Fees & Costs =================

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_FeeNotes_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, RefNo, MatterId, LawFirmServiceProvider, FeeNoteInvoiceNo, InvoiceDate, PeriodCovered,
           DescriptionOfWork, ProfessionalFees, Disbursements, Vat, WithholdingTax, ApprovedBudgetForStage,
           ApprovedBy, PoRequisitionNo, PaymentStatus, PaymentDate, Remarks
    FROM dbo.FeeNotes
    ORDER BY InvoiceDate DESC;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_FeeNotes_GetByMatterId
    @MatterId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, RefNo, MatterId, LawFirmServiceProvider, FeeNoteInvoiceNo, InvoiceDate, PeriodCovered,
           DescriptionOfWork, ProfessionalFees, Disbursements, Vat, WithholdingTax, ApprovedBudgetForStage,
           ApprovedBy, PoRequisitionNo, PaymentStatus, PaymentDate, Remarks
    FROM dbo.FeeNotes
    WHERE MatterId = @MatterId
    ORDER BY InvoiceDate DESC;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_FeeNotes_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, RefNo, MatterId, LawFirmServiceProvider, FeeNoteInvoiceNo, InvoiceDate, PeriodCovered,
           DescriptionOfWork, ProfessionalFees, Disbursements, Vat, WithholdingTax, ApprovedBudgetForStage,
           ApprovedBy, PoRequisitionNo, PaymentStatus, PaymentDate, Remarks
    FROM dbo.FeeNotes
    WHERE Id = @Id;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_FeeNotes_Create
    @MatterId INT,
    @LawFirmServiceProvider NVARCHAR(200),
    @FeeNoteInvoiceNo NVARCHAR(100),
    @InvoiceDate DATETIME2,
    @PeriodCovered NVARCHAR(100),
    @DescriptionOfWork NVARCHAR(2000),
    @ProfessionalFees DECIMAL(18,2),
    @Disbursements DECIMAL(18,2),
    @Vat DECIMAL(18,2),
    @WithholdingTax DECIMAL(18,2),
    @ApprovedBudgetForStage DECIMAL(18,2),
    @ApprovedBy NVARCHAR(200),
    @PoRequisitionNo NVARCHAR(100),
    @PaymentStatus NVARCHAR(50),
    @PaymentDate DATETIME2 = NULL,
    @Remarks NVARCHAR(2000),
    @Id INT OUTPUT,
    @RefNo NVARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    DECLARE @NextNo INT;
    SELECT @NextNo = ISNULL(MAX(CAST(SUBSTRING(RefNo, 3, 10) AS INT)), 0) + 1
    FROM dbo.FeeNotes WITH (UPDLOCK, HOLDLOCK);
    SET @RefNo = 'F-' + RIGHT('0000' + CAST(@NextNo AS NVARCHAR(4)), 4);

    INSERT INTO dbo.FeeNotes
        (RefNo, MatterId, LawFirmServiceProvider, FeeNoteInvoiceNo, InvoiceDate, PeriodCovered,
         DescriptionOfWork, ProfessionalFees, Disbursements, Vat, WithholdingTax, ApprovedBudgetForStage,
         ApprovedBy, PoRequisitionNo, PaymentStatus, PaymentDate, Remarks)
    VALUES
        (@RefNo, @MatterId, @LawFirmServiceProvider, @FeeNoteInvoiceNo, @InvoiceDate, @PeriodCovered,
         @DescriptionOfWork, @ProfessionalFees, @Disbursements, @Vat, @WithholdingTax, @ApprovedBudgetForStage,
         @ApprovedBy, @PoRequisitionNo, @PaymentStatus, @PaymentDate, @Remarks);

    SET @Id = SCOPE_IDENTITY();

    COMMIT TRANSACTION;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_FeeNotes_Update
    @Id INT,
    @LawFirmServiceProvider NVARCHAR(200),
    @FeeNoteInvoiceNo NVARCHAR(100),
    @InvoiceDate DATETIME2,
    @PeriodCovered NVARCHAR(100),
    @DescriptionOfWork NVARCHAR(2000),
    @ProfessionalFees DECIMAL(18,2),
    @Disbursements DECIMAL(18,2),
    @Vat DECIMAL(18,2),
    @WithholdingTax DECIMAL(18,2),
    @ApprovedBudgetForStage DECIMAL(18,2),
    @ApprovedBy NVARCHAR(200),
    @PoRequisitionNo NVARCHAR(100),
    @PaymentStatus NVARCHAR(50),
    @PaymentDate DATETIME2 = NULL,
    @Remarks NVARCHAR(2000)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.FeeNotes
    SET LawFirmServiceProvider = @LawFirmServiceProvider,
        FeeNoteInvoiceNo = @FeeNoteInvoiceNo,
        InvoiceDate = @InvoiceDate,
        PeriodCovered = @PeriodCovered,
        DescriptionOfWork = @DescriptionOfWork,
        ProfessionalFees = @ProfessionalFees,
        Disbursements = @Disbursements,
        Vat = @Vat,
        WithholdingTax = @WithholdingTax,
        ApprovedBudgetForStage = @ApprovedBudgetForStage,
        ApprovedBy = @ApprovedBy,
        PoRequisitionNo = @PoRequisitionNo,
        PaymentStatus = @PaymentStatus,
        PaymentDate = @PaymentDate,
        Remarks = @Remarks
    WHERE Id = @Id;
END");

            // ================= Lessons Learnt =================

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Lessons_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, LessonNo, MatterId, DateLogged, LoggedBy, LessonType, StageInLifecycle, PrimaryRootCause,
           SecondaryRootCause, WhatHappened, ConsequenceWhyItMattered, Avoidability, CostAttributable,
           InstrumentToChange, SpecificPolicyAffected, RecommendedChange, ActionOwnerFunction, ActionOwnerName,
           Priority, TargetImplementationDate, ActionStatus, DateImplemented, EvidenceOfImplementation,
           EffectivenessReviewDate, EffectivenessVerdict, HasIssueRecurred, OtherMattersShowingSameIssue,
           ApprovedByGovernanceForum, Confidentiality, Remarks
    FROM dbo.Lessons
    ORDER BY DateLogged DESC;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Lessons_GetByMatterId
    @MatterId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, LessonNo, MatterId, DateLogged, LoggedBy, LessonType, StageInLifecycle, PrimaryRootCause,
           SecondaryRootCause, WhatHappened, ConsequenceWhyItMattered, Avoidability, CostAttributable,
           InstrumentToChange, SpecificPolicyAffected, RecommendedChange, ActionOwnerFunction, ActionOwnerName,
           Priority, TargetImplementationDate, ActionStatus, DateImplemented, EvidenceOfImplementation,
           EffectivenessReviewDate, EffectivenessVerdict, HasIssueRecurred, OtherMattersShowingSameIssue,
           ApprovedByGovernanceForum, Confidentiality, Remarks
    FROM dbo.Lessons
    WHERE MatterId = @MatterId
    ORDER BY DateLogged DESC;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Lessons_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, LessonNo, MatterId, DateLogged, LoggedBy, LessonType, StageInLifecycle, PrimaryRootCause,
           SecondaryRootCause, WhatHappened, ConsequenceWhyItMattered, Avoidability, CostAttributable,
           InstrumentToChange, SpecificPolicyAffected, RecommendedChange, ActionOwnerFunction, ActionOwnerName,
           Priority, TargetImplementationDate, ActionStatus, DateImplemented, EvidenceOfImplementation,
           EffectivenessReviewDate, EffectivenessVerdict, HasIssueRecurred, OtherMattersShowingSameIssue,
           ApprovedByGovernanceForum, Confidentiality, Remarks
    FROM dbo.Lessons
    WHERE Id = @Id;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Lessons_Create
    @MatterId INT,
    @DateLogged DATETIME2,
    @LoggedBy NVARCHAR(200),
    @LessonType NVARCHAR(100),
    @StageInLifecycle NVARCHAR(100),
    @PrimaryRootCause NVARCHAR(200),
    @SecondaryRootCause NVARCHAR(200),
    @WhatHappened NVARCHAR(4000),
    @ConsequenceWhyItMattered NVARCHAR(2000),
    @Avoidability NVARCHAR(50),
    @CostAttributable DECIMAL(18,2),
    @InstrumentToChange NVARCHAR(100),
    @SpecificPolicyAffected NVARCHAR(300),
    @RecommendedChange NVARCHAR(2000),
    @ActionOwnerFunction NVARCHAR(200),
    @ActionOwnerName NVARCHAR(200),
    @Priority NVARCHAR(50),
    @TargetImplementationDate DATETIME2 = NULL,
    @ActionStatus NVARCHAR(50),
    @ApprovedByGovernanceForum NVARCHAR(200),
    @Confidentiality NVARCHAR(50),
    @Remarks NVARCHAR(2000),
    @Id INT OUTPUT,
    @LessonNo NVARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    DECLARE @NextNo INT;
    SELECT @NextNo = ISNULL(MAX(CAST(SUBSTRING(LessonNo, 3, 10) AS INT)), 0) + 1
    FROM dbo.Lessons WITH (UPDLOCK, HOLDLOCK);
    SET @LessonNo = 'L-' + RIGHT('0000' + CAST(@NextNo AS NVARCHAR(4)), 4);

    INSERT INTO dbo.Lessons
        (LessonNo, MatterId, DateLogged, LoggedBy, LessonType, StageInLifecycle, PrimaryRootCause,
         SecondaryRootCause, WhatHappened, ConsequenceWhyItMattered, Avoidability, CostAttributable,
         InstrumentToChange, SpecificPolicyAffected, RecommendedChange, ActionOwnerFunction, ActionOwnerName,
         Priority, TargetImplementationDate, ActionStatus, DateImplemented, EvidenceOfImplementation,
         EffectivenessReviewDate, EffectivenessVerdict, HasIssueRecurred, OtherMattersShowingSameIssue,
         ApprovedByGovernanceForum, Confidentiality, Remarks)
    VALUES
        (@LessonNo, @MatterId, @DateLogged, @LoggedBy, @LessonType, @StageInLifecycle, @PrimaryRootCause,
         @SecondaryRootCause, @WhatHappened, @ConsequenceWhyItMattered, @Avoidability, @CostAttributable,
         @InstrumentToChange, @SpecificPolicyAffected, @RecommendedChange, @ActionOwnerFunction, @ActionOwnerName,
         @Priority, @TargetImplementationDate, @ActionStatus, NULL, '',
         NULL, '', 'No', '',
         @ApprovedByGovernanceForum, @Confidentiality, @Remarks);

    SET @Id = SCOPE_IDENTITY();

    COMMIT TRANSACTION;
END");

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Lessons_Update
    @Id INT,
    @LessonType NVARCHAR(100),
    @StageInLifecycle NVARCHAR(100),
    @PrimaryRootCause NVARCHAR(200),
    @SecondaryRootCause NVARCHAR(200),
    @WhatHappened NVARCHAR(4000),
    @ConsequenceWhyItMattered NVARCHAR(2000),
    @Avoidability NVARCHAR(50),
    @CostAttributable DECIMAL(18,2),
    @InstrumentToChange NVARCHAR(100),
    @SpecificPolicyAffected NVARCHAR(300),
    @RecommendedChange NVARCHAR(2000),
    @ActionOwnerFunction NVARCHAR(200),
    @ActionOwnerName NVARCHAR(200),
    @Priority NVARCHAR(50),
    @TargetImplementationDate DATETIME2 = NULL,
    @ActionStatus NVARCHAR(50),
    @DateImplemented DATETIME2 = NULL,
    @EvidenceOfImplementation NVARCHAR(1000),
    @EffectivenessReviewDate DATETIME2 = NULL,
    @EffectivenessVerdict NVARCHAR(50),
    @HasIssueRecurred NVARCHAR(10),
    @OtherMattersShowingSameIssue NVARCHAR(500),
    @ApprovedByGovernanceForum NVARCHAR(200),
    @Confidentiality NVARCHAR(50),
    @Remarks NVARCHAR(2000)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Lessons
    SET LessonType = @LessonType,
        StageInLifecycle = @StageInLifecycle,
        PrimaryRootCause = @PrimaryRootCause,
        SecondaryRootCause = @SecondaryRootCause,
        WhatHappened = @WhatHappened,
        ConsequenceWhyItMattered = @ConsequenceWhyItMattered,
        Avoidability = @Avoidability,
        CostAttributable = @CostAttributable,
        InstrumentToChange = @InstrumentToChange,
        SpecificPolicyAffected = @SpecificPolicyAffected,
        RecommendedChange = @RecommendedChange,
        ActionOwnerFunction = @ActionOwnerFunction,
        ActionOwnerName = @ActionOwnerName,
        Priority = @Priority,
        TargetImplementationDate = @TargetImplementationDate,
        ActionStatus = @ActionStatus,
        DateImplemented = @DateImplemented,
        EvidenceOfImplementation = @EvidenceOfImplementation,
        EffectivenessReviewDate = @EffectivenessReviewDate,
        EffectivenessVerdict = @EffectivenessVerdict,
        HasIssueRecurred = @HasIssueRecurred,
        OtherMattersShowingSameIssue = @OtherMattersShowingSameIssue,
        ApprovedByGovernanceForum = @ApprovedByGovernanceForum,
        Confidentiality = @Confidentiality,
        Remarks = @Remarks
    WHERE Id = @Id;
END");

            // ================= Reporting: Insight Analysis =================

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Insights_RootCauseAnalysis
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Causes AS (
        SELECT PrimaryRootCause AS RootCause, 1 AS IsPrimary, CostAttributable, ActionStatus
        FROM dbo.Lessons
        WHERE PrimaryRootCause <> ''
        UNION ALL
        SELECT SecondaryRootCause AS RootCause, 0 AS IsPrimary, CostAttributable, ActionStatus
        FROM dbo.Lessons
        WHERE SecondaryRootCause <> ''
    )
    SELECT
        RootCause,
        SUM(CASE WHEN IsPrimary = 1 THEN 1 ELSE 0 END) AS CitedAsPrimary,
        SUM(CASE WHEN IsPrimary = 0 THEN 1 ELSE 0 END) AS CitedAsSecondary,
        COUNT(*) AS TotalCitations,
        SUM(CostAttributable) AS CostAttributable,
        SUM(CASE WHEN ActionStatus NOT IN ('Implemented', 'Verified effective', 'Closed - no action required', 'Rejected / not pursued') THEN 1 ELSE 0 END) AS OpenActions,
        CASE WHEN COUNT(*) >= 3 THEN 'Recurring pattern - process failure' ELSE 'Isolated' END AS Pattern
    FROM Causes
    GROUP BY RootCause
    ORDER BY TotalCitations DESC, CostAttributable DESC;
END");

            // ================= Reporting: Dashboard =================

            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.sp_Dashboard_PortfolioSummary
    @DiaryLookAheadDays INT = 30,
    @DormancyThresholdDays INT = 60
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Now DATETIME2 = SYSUTCDATETIME();

    SELECT
        (SELECT COUNT(*) FROM dbo.Matters) AS TotalMatters,
        (SELECT COUNT(*) FROM dbo.Matters WHERE StatusOfMatter NOT IN ('Concluded - Closed', 'Withdrawn / Discontinued')) AS OpenMatters,
        (SELECT COUNT(*) FROM dbo.Matters WHERE StatusOfMatter IN ('Concluded - Closed', 'Withdrawn / Discontinued')) AS ClosedMatters,
        (SELECT ISNULL(SUM(SumClaimed + EstimatedLegalFeesToFinalisation + LegalFeesIncurredToDate), 0) FROM dbo.Matters) AS TotalFinancialExposure,
        (SELECT ISNULL(SUM(LegalFeesIncurredToDate), 0) FROM dbo.Matters) AS TotalLegalFeesIncurredToDate,
        (SELECT COUNT(*) FROM dbo.DiaryEvents WHERE EventDate BETWEEN @Now AND DATEADD(DAY, @DiaryLookAheadDays, @Now) AND EventStatus = 'Scheduled') AS UpcomingEventsCount,
        (SELECT COUNT(*) FROM dbo.Matters WHERE StatusOfMatter NOT IN ('Concluded - Closed', 'Withdrawn / Discontinued') AND LastActivityDate < DATEADD(DAY, -@DormancyThresholdDays, @Now)) AS DormantMattersCount;

    SELECT StatusOfMatter, COUNT(*) AS MatterCount
    FROM dbo.Matters
    GROUP BY StatusOfMatter
    ORDER BY MatterCount DESC;

    SELECT RiskRating, COUNT(*) AS MatterCount
    FROM dbo.Matters
    GROUP BY RiskRating
    ORDER BY MatterCount DESC;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Dashboard_PortfolioSummary;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Insights_RootCauseAnalysis;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Lessons_Update;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Lessons_Create;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Lessons_GetById;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Lessons_GetByMatterId;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Lessons_GetAll;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_FeeNotes_Update;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_FeeNotes_Create;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_FeeNotes_GetById;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_FeeNotes_GetByMatterId;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_FeeNotes_GetAll;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_DiaryEvents_Update;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_DiaryEvents_Create;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_DiaryEvents_GetById;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_DiaryEvents_GetByMatterId;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_DiaryEvents_GetAll;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Documents_Update;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Documents_Create;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Documents_GetById;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Documents_GetByMatterId;");
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_Documents_GetAll;");
        }
    }
}
