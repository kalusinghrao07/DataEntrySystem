using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntrySystem.Contracts
{
    public class Document
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
    }
}
