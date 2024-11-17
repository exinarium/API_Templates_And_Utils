using System;
using System.Collections.Generic;
using Full_Application_Blazor.Utils.Helpers.Classes;
using Full_Application_Blazor.Utils.Helpers.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Models
{
    public class Invoice : BaseModel
    {
        [BsonElement("invoiceNumber")]
        public string InvoiceNumber { get; set; }

        [BsonElement("invoiceDescription")]
        public string InvoiceDescription { get; set; }

        [BsonElement("dueDate")]
        public DateTime DueDate { get; set; }

        [BsonElement("invoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [BsonElement("isSentPrinted")]
        public bool IsSentPrinted { get; set; }

        [BsonElement("bankDetailsId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BankDetailsId { get; set; }

        [BsonElement("entityID")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EntityID { get; set; }

        [BsonElement("status")]
        public InvoiceStatus Status { get; set; }

        [BsonElement("invoiceType")]
        public InvoiceType InvoiceType { get; set; }

        [BsonElement("totalAmount")]
        public double TotalAmount { get; set; }

        [BsonElement("totalVATAmount")]
        public double TotalVATAmount { get; set; }

        [BsonElement("paidAmount")]
        public double PaidAmount { get; set; }

        [BsonElement("invoiceVATPercentage")]
        public double VATPercentage { get; set; }

        [BsonElement("paymentDate")]
        public DateTime? PaymentDate { get; set; }

        [BsonElement("manuallyProcessed")]
        public bool ManuallyProcessed { get; set; }

        [BsonElement("error")]
        public string Error { get; set; }

        [BsonElement("errorCode")]
        public int? ErrorCode { get; set; }

        [BsonElement("lineItems")]
        public List<LineItem> LineItems { get; set; }
    }
}

