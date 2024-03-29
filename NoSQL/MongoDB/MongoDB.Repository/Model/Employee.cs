﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB.Repository.Models
{
    public class Employee
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; } = "";
        public string CardNumber { get; set; } = "";
        public decimal Salary { get; set; } = 0;
        public byte[] Photo { get; set; } = Array.Empty<byte>();
    }
}