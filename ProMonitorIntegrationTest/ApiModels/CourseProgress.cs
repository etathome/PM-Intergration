using System;

namespace ProMonitorIntegrationTest.ApiModels
{
    public class CourseProgress
    {
        public Guid? UserId { get; set; }
        public Guid? QualificationId { get; set; }
        public string QualificationType { get; set; }
        public int? TotalCourseCount { get; set; }
        public double? MappedPCTotal { get; set; }
        public double? AssessedPCTotal { get; set; }
        public double? AssessedPCPercentage { get; set; }
        public double? MappedPCPercentage { get; set; }
    }
}