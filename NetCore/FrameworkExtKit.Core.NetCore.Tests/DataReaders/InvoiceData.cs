using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Core.NetCore.Tests.DataReaders {
    public class InvoiceData {
        public string CircuitName { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime BillingPeriod { get; set; }
        public DateTime InvoiceFrom { get; set; }
        public DateTime InvoiceTo { get; set; }
        public string CompanyNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountingUnit { get; set; }
        public string ActivityCode { get; set; }
        public string URN { get; set; }
        
        
        public decimal OtherRecurrentCost { get; set; }
        public decimal AccessInstallationCharge { get; set; }
        public decimal TerminationCharge { get; set; }
        public decimal OtherOnetimeCharge { get; set; }
        public decimal SalesTax { get; set; }
        public decimal VAT { get; set; }
        public decimal LateFee { get; set; }
        public decimal SLACredit { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxCredit { get; set; }
    }

    public class LineInvoiceData : InvoiceData {
        public decimal PortCharge { get; set; }
        public decimal AccessCharge { get; set; }
        public decimal QosCharge { get; set; }
        public decimal PortInstallationCharge { get; set; }
    }

    public class FieldInvoiceData : InvoiceData {
        public decimal TeleportCharge { get; set; }
        public decimal ServiceLevelCharge { get; set; }
    }
}
