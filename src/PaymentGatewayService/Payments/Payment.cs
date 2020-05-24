using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentGatewayService.Payments
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CardNumber { get; set; }

        public string Amount { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public int Cvv { get; set; }

        public string Currency { get; set; }

        public DateTime Created { get; set; }

        public PaymentStatus Status { get; set; }
    }
}