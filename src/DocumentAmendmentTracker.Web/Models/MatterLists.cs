namespace DocumentAmendmentTracker.Web.Models;

/// <summary>Dropdown option lists for the Matter/Party Register, taken from the source spreadsheet's Lists sheet.</summary>
public static class MatterLists
{
    public static readonly string[] ForumTypes =
    {
        "Litigation", "Arbitration", "Mediation", "Conciliation", "Tribunal", "Internal Hearing", "Other",
    };

    public static readonly string[] Forums =
    {
        "High Court", "Supreme Court", "Constitutional Court", "Industrial Relations Court", "Local Court",
        "Arbitration Tribunal", "Other",
    };

    public static readonly string[] Roles =
    {
        "Claimant/Plaintiff", "Defendant/Respondent", "Third Party", "Intervener", "Other",
    };

    public static readonly string[] Natures =
    {
        "Employment", "Contract", "Debt Recovery", "Property", "Tort", "Regulatory", "Tax", "Intellectual Property",
        "Other",
    };

    public static readonly string[] Statuses =
    {
        "New", "Pleadings", "Discovery", "Pre-Trial", "Trial/Hearing", "Judgment/Award Pending", "Judgment/Award Issued",
        "Settlement Negotiation", "Settled", "Appeal", "Enforcement", "Closed - Won", "Closed - Lost",
        "Closed - Settled", "Closed - Withdrawn", "Stayed", "Dormant",
    };

    public static readonly string[] Likelihoods = { "Remote", "Possible", "Probable", "Virtually Certain" };

    public static readonly string[] Risks = { "Low", "Medium", "High", "Critical" };

    public static readonly string[] YesNo = { "Yes", "No" };

    public static readonly string[] Currencies = { "ZMW", "USD", "GBP", "EUR", "ZAR" };

    public static readonly string[] Sides = { "Claimant / Plaintiff", "Defendant / Respondent" };

    public static readonly string[] PartyRoles =
    {
        "Lead Claimant", "Co-Claimant", "Lead Respondent", "Co-Respondent", "Third Party", "Intervener", "Other",
    };

    public static readonly string[] PartyTypes = { "Individual", "Company", "Government Entity", "Group/Class", "Other" };

    public static readonly string[] PartyStatuses = { "Active", "Struck Out", "Discontinued Against", "Settled Out" };

    public static readonly string[] ReviewStatuses = { "Not yet due", "Due", "In Progress", "Completed" };

    public static readonly string[] Avoidabilities = { "Avoidable", "Unavoidable", "Partly Avoidable", "Not Assessed" };

    public static readonly string[] Entities = { "Group", "Subsidiary A", "Subsidiary B", "Subsidiary C" };

    public static readonly string[] InactivePartyStatuses = { "Struck Out", "Discontinued Against" };
}
