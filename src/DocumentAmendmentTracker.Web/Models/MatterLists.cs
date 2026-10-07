namespace DocumentAmendmentTracker.Web.Models;

/// <summary>Dropdown option lists for the Matter/Party/Document/Diary/Fees/Lessons registers, taken verbatim from the source spreadsheet's Lists sheet.</summary>
public static class MatterLists
{
    public static readonly string[] ForumTypes =
    {
        "Court", "Tribunal", "Conciliation", "Mediation", "Arbitration", "Regulatory / Administrative",
        "Internal Appeal / Grievance",
    };

    public static readonly string[] Forums =
    {
        "Supreme Court of Zambia", "Constitutional Court", "Court of Appeal", "High Court - General List",
        "High Court - Commercial Division", "High Court - Industrial Relations Division",
        "High Court - Family & Children", "Subordinate Court", "Local Court", "Small Claims Court",
        "Labour Commissioner (Conciliation)", "Ministry of Labour - Conciliation",
        "CIArb Zambia Arbitration Tribunal", "Ad hoc Arbitration Tribunal", "Tax Appeals Tribunal",
        "Lands Tribunal", "Competition & Consumer Protection Tribunal", "NAPSA Appeals", "NHIMA Appeals",
        "WCFCB", "ZEMA Appeals", "Mines Safety / Regulatory", "Private Mediation", "Other",
    };

    public static readonly string[] Roles =
    {
        "Plaintiff", "Defendant", "Applicant", "Respondent", "Appellant", "Complainant", "Claimant",
        "Interested Party", "Third Party",
    };

    public static readonly string[] Natures =
    {
        "Employment & Labour", "Unfair Dismissal", "Terminal Benefits", "Collective / Union Dispute",
        "Commercial / Contract", "Debt Recovery", "Land & Property", "Tax", "Regulatory / Compliance",
        "Environmental", "Personal Injury / Fatality", "Statutory Benefits (NAPSA/NHIMA/WCFCB)", "Insurance",
        "Intellectual Property", "Criminal", "Judicial Review", "Other",
    };

    public static readonly string[] Statuses =
    {
        "Not Yet Filed", "Filed - Awaiting Service", "Pleadings Stage", "Discovery / Disclosure",
        "Awaiting Hearing Date", "Part-Heard", "Trial / Hearing in Progress", "Awaiting Judgment / Award",
        "Judgment / Award Delivered", "Under Appeal", "In Conciliation", "In Mediation", "In Arbitration",
        "Settlement Negotiations", "Settled - Awaiting Payment", "Concluded - Closed",
        "Withdrawn / Discontinued", "Dormant / Stayed",
    };

    public static readonly string[] Likelihoods = { "Probable", "Possible", "Remote", "Not Yet Assessed" };

    public static readonly string[] Risks = { "High", "Medium", "Low" };

    public static readonly string[] YesNo = { "Yes", "No" };

    public static readonly string[] Currencies = { "ZMW", "USD", "ZAR", "GBP", "EUR" };

    public static readonly string[] Sides = { "Claimant / Plaintiff", "Defendant / Respondent", "Third Party / Other" };

    public static readonly string[] PartyRoles =
    {
        "Plaintiff", "Claimant", "Complainant", "Applicant", "Appellant", "Petitioner", "Defendant",
        "Respondent", "Cross-Appellant", "Third Party", "Interested Party", "Intervener", "Co-Defendant",
        "Nominal Party",
    };

    public static readonly string[] PartyTypes =
    {
        "Individual", "Individual - Employee", "Individual - Former Employee", "Company", "Partnership",
        "Sole Trader", "Trade Union", "Statutory Body", "Government Ministry / Agency", "Local Authority",
        "Estate of Deceased", "Minor (through next friend)", "Association / NGO", "Group / Related Entity",
    };

    public static readonly string[] PartyStatuses =
    {
        "Active", "Joined Later", "Struck Out", "Discontinued Against", "Settled", "Judgment Entered",
        "Award Made", "Deceased", "Substituted", "Not Served",
    };

    public static readonly string[] InactivePartyStatuses =
    {
        "Struck Out", "Discontinued Against", "Settled", "Judgment Entered", "Award Made", "Deceased",
        "Substituted", "Not Served",
    };

    public static readonly string[] ReviewStatuses =
    {
        "Not yet due", "Due", "In progress", "Completed", "Waived - no learning value",
    };

    public static readonly string[] Avoidabilities =
    {
        "Wholly avoidable", "Partly avoidable", "Unavoidable", "Avoidable at disproportionate cost",
        "Not yet assessed",
    };

    public static readonly string[] Entities =
    {
        "FQM Zambia Ltd", "Kansanshi Mining Plc", "Kalumbila Minerals Ltd", "FQM Trident Ltd",
        "Mopani / Other Subsidiary", "Group / Shared Services", "Other Entity",
    };

    // --- Document Register ---

    public static readonly string[] DocTypes =
    {
        "Writ of Summons", "Statement of Claim", "Notice of Motion", "Originating Summons",
        "Affidavit in Support", "Affidavit in Opposition", "Memorandum of Appearance", "Defence",
        "Counterclaim", "Reply", "Notice of Joinder", "Application to Strike Out", "Consolidation Order",
        "List of Documents", "Bundle of Documents", "Witness Statement", "Skeleton Arguments / Submissions",
        "Court Order", "Ruling", "Judgment", "Notice of Appeal", "Record of Appeal", "Consent Order",
        "Settlement Agreement", "Deed of Release", "Notice of Arbitration", "Terms of Reference",
        "Points of Claim", "Points of Defence", "Arbitral Award", "Conciliation Record",
        "Certificate of Non-Settlement", "Mediation Agreement", "Instruction / Retainer Letter",
        "Legal Opinion", "Fee Note / Invoice", "Correspondence", "Other",
    };

    public static readonly string[] Directions =
    {
        "Filed by us", "Served on us", "Issued by Court / Tribunal", "Received from opposing party",
        "Outgoing correspondence", "Internal",
    };

    public static readonly string[] DocStatuses =
    {
        "Draft", "Under Review", "Final - Not Filed", "Filed", "Served", "Executed", "Superseded",
    };

    public static readonly string[] Confidentialities = { "Public", "Internal", "Confidential", "Legally Privileged" };

    public static readonly string[] ServiceModes =
    {
        "Personal Service", "Bailiff / Sheriff", "Registered Post", "Email", "Hand Delivery",
        "Substituted Service", "Court Portal", "Not Applicable",
    };

    // --- Diary & Events ---

    public static readonly string[] EventTypes =
    {
        "Mention", "Hearing", "Trial", "Status Conference", "Scheduling Conference", "Preliminary Meeting",
        "Conciliation Session", "Mediation Session", "Arbitration Hearing", "Delivery of Judgment",
        "Delivery of Award", "Filing Deadline", "Settlement Meeting", "Internal Review", "Other",
    };

    public static readonly string[] EventStatuses = { "Scheduled", "Held", "Adjourned", "Vacated", "Completed", "Missed" };

    // --- Fees & Costs ---

    public static readonly string[] PayStatuses =
    {
        "Not Submitted", "Submitted", "Approved", "Partially Paid", "Paid", "Disputed", "On Hold",
    };

    // --- Lessons Learnt ---

    public static readonly string[] LessonTypes =
    {
        "What went badly", "What went well", "Near miss / avoided claim", "Recurring pattern",
        "Systemic issue", "External factor - no fault",
    };

    public static readonly string[] Stages =
    {
        "Recruitment & Selection", "Contracting / Terms of Engagement", "Onboarding",
        "During Employment - Performance", "During Employment - Conduct", "Remuneration & Benefits",
        "Working Time / Overtime / Leave", "Health, Safety & Environment", "Disciplinary Process",
        "Grievance Process", "Redundancy / Restructuring", "Termination / Separation",
        "Post-Termination (benefits, references)", "Contract Formation - Commercial",
        "Contract Performance - Commercial", "Procurement / Tender", "Regulatory Interaction",
        "Land / Community", "Conduct of the Litigation Itself",
    };

    public static readonly string[] RootCauses =
    {
        "Policy gap - no policy on the point", "Policy unclear or contradictory",
        "Policy exists but was not followed", "Procedural defect - fair hearing",
        "Procedural defect - notice or timelines", "Inadequate documentation / record keeping",
        "Evidence lost or unavailable", "Contract drafting defect",
        "Statutory non-compliance (ECA / NAPSA / NHIMA / WCFCB)", "Delegated authority exceeded",
        "Delegated authority not exercised", "Line manager competence / training gap",
        "HR advice inadequate or late", "Communication failure with employee or party",
        "System / data error (payroll, HRIS, time records)", "Calculation error - terminal benefits or pay",
        "Delay in decision or response", "Inconsistent treatment / precedent breached",
        "Third party or contractor failure", "Commercial term breached by us",
        "Commercial term breached by counterparty", "External counsel instruction or management",
        "Business decision - risk knowingly accepted", "No fault - claim without merit", "Other",
    };

    public static readonly string[] Instruments =
    {
        "HR Policy amendment", "Procedure / SOP amendment", "Contract template", "Letter / form template",
        "Delegation of authority", "Training / briefing", "System configuration change",
        "Data quality remediation", "Job design / role clarity", "Communication or consultation practice",
        "Governance / committee terms of reference", "External counsel instruction protocol",
        "Record retention practice", "No change required", "Other",
    };

    public static readonly string[] Priorities = { "Critical", "High", "Medium", "Low" };

    public static readonly string[] ActionStatuses =
    {
        "Logged - not yet reviewed", "Under review", "Approved for action", "In progress", "Implemented",
        "Verified effective", "Closed - no action required", "Rejected / not pursued", "Deferred",
    };

    public static readonly string[] Effectivenesses =
    {
        "Too early to assess", "Effective", "Partially effective", "Not effective", "Issue has recurred",
    };

    // --- Assumptions / settings (Lists sheet, yellow cells) ---

    public const decimal VatRate = 0.16m;
    public const decimal WithholdingTaxRateOnProfessionalFees = 0.15m;
    public const int DiaryLookAheadDays = 30;
    public const int DormancyThresholdDays = 60;
}
