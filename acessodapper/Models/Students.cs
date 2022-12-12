using System;

namespace Acessodaper.Models
{
    public class Students
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Document { get; set; }

        public string? Phone { get; set; }

        public string? BirthDate { get; set; }

        public string? CreateDate { get; set; }
    }
}