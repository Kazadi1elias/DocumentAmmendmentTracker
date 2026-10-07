using System.ComponentModel.DataAnnotations;

namespace DocumentAmendmentTracker.Web.Models;

public class FeeNote
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string RefNo { get; set; } = string.Empty;

    public int MatterId { get; set; }

    [Required, StringLength(200)]
    public string LawFirmServiceProvider { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string FeeNoteInvoiceNo { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; }

    [StringLength(100)]
    public string PeriodCovered { get; set; } = string.Empty;

    [StringLength(2000)]
    public string DescriptionOfWork { get; set; } = string.Empty;

    public decimal ProfessionalFees { get; set; }

    public decimal Disbursements { get; set; }

    public decimal Vat { get; set; }

    public decimal WithholdingTax { get; set; }

    public decimal ApprovedBudgetForStage { get; set; }

    [StringLength(200)]
    public string ApprovedBy { get; set; } = string.Empty;

    [StringLength(100)]
    public string PoRequisitionNo { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string PaymentStatus { get; set; } = "Not Submitted";

    public DateTime? PaymentDate { get; set; }

    [StringLength(2000)]
    public string Remarks { get; set; } = string.Empty;

    // --- Derived (computed server-side, never stored) ---

    public decimal GrossInvoiceValue { get; set; }

    public decimal NetPayableToFirm { get; set; }

    public decimal VarianceToBudget { get; set; }
}
