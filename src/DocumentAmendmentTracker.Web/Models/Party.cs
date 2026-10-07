using System.ComponentModel.DataAnnotations;

namespace DocumentAmendmentTracker.Web.Models;

public class Party
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string PartyNo { get; set; } = string.Empty;

    public int MatterId { get; set; }

    [Required, StringLength(100)]
    public string Side { get; set; } = string.Empty;

    public int PartyOrder { get; set; }

    [Required, StringLength(100)]
    public string PartyRoleTitle { get; set; } = string.Empty;

    [Required, StringLength(300)]
    public string PartyName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string PartyType { get; set; } = string.Empty;

    public bool IsGroupEntity { get; set; }

    [StringLength(100)]
    public string EmployeePayrollNo { get; set; } = string.Empty;

    [StringLength(100)]
    public string NrcPassportRegNo { get; set; } = string.Empty;

    [StringLength(300)]
    public string RepresentedBy { get; set; } = string.Empty;

    public decimal IndividualSumClaimed { get; set; }

    public DateTime DateJoined { get; set; }

    public DateTime? DateCeased { get; set; }

    [Required, StringLength(100)]
    public string PartyStatus { get; set; } = "Active";

    [StringLength(500)]
    public string ServiceAddress { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Remarks { get; set; } = string.Empty;

    /// <summary>Derived: e.g. "1st Lead Claimant". Computed from PartyOrder + PartyRoleTitle, never stored.</summary>
    public string Designation { get; set; } = string.Empty;
}
