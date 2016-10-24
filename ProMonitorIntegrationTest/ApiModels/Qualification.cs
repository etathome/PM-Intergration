using System;

namespace ProMonitorIntegrationTest.ApiModels
{
    public class Qualification 
    {
        public Guid Id { get; set; }
        public Guid OrganisationId { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
        public Guid? GroupId { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string AimNo { get; set; }
        public int? AwardingBody { get; set; }
        public string UnitGuidance { get; set; }
        public string Framework { get; set; }
        public bool CourseUpdateInProgress { get; set; }
        public string InteralCourseCode { get; set; }
        public bool IsActive { get; set; }
        public string QualificationType { get; set; }
        public string UnitType { get; set; }
        public int? UnitLevel { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string LadCode { get; set; }
        public string Comments { get; set; }
        public Guid? AdminId { get; set; }
    }
}