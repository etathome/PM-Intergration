using System;
using System.Collections.Generic;
using ProMonitorIntegrationTest.ApiModels;

namespace ProMonitorIntegrationTest.ViewModels
{
    public class LearnerProgressViewModel
    {
        public IEnumerable<Qualification> Qualifications { get; set; }
        public Guid? UserId { get; set; }
        public Guid? UnitId { get; set; }
        public string QualificationName { get; set; }
        public string UnitType { get; set; }
        public int? TotalCourseCount { get; set; }
        public double MappedPCTotal { get; set; }
        public double? AssessedPCTotal { get; set; }
        public double AssessedPCPercentage { get; set; }
        public double MappedPCPercentage { get; set; }
}
}