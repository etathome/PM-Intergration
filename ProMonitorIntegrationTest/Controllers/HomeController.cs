using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProMonitorIntegrationTest.ApiModels;
using ProMonitorIntegrationTest.ViewModels;

namespace ProMonitorIntegrationTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly Api _api = new Api();

        public HomeController()
        {
            //TODO:Remove this line when new SSL certificate is valid
            _api.DisableSslCertValidation = true;
            _api.AccessToken = _api.AquireToken();
        }

        public ActionResult Index()
        {
            var learnerId = "TEST012345A";
            var progress = _api.CallApiPost<List<CourseProgress>>(ApiEndPoints.LearnerProgress,
                new CourseProgressRequestModel() { LearnerId = learnerId });
            var qualifications = _api.CallApiGet<List<Qualification>>(ApiEndPoints.Qualification);
            return View(MapProgressQualification(progress, qualifications));
        }

        private static IEnumerable<LearnerProgressViewModel> MapProgressQualification(IEnumerable<CourseProgress> progress, List<Qualification> qualifications)
        {
            return progress.Select(item =>
            {
                var qualificationName = ((qualifications.SingleOrDefault(q => q.Id == item.QualificationId) == null)
                    ? "Unknown"
                    : qualifications.Single(q => q.Id == item.QualificationId).Name);
                return new LearnerProgressViewModel
                {
                    UnitId = item.QualificationId,
                    AssessedPCPercentage = (item.AssessedPCPercentage == null ? 0.0 : (double)item.AssessedPCPercentage),
                    AssessedPCTotal = item.AssessedPCTotal,
                    MappedPCPercentage = (item.MappedPCPercentage == null ? 0.0 : (double)item.MappedPCPercentage),
                    MappedPCTotal = (item.MappedPCTotal == null ? 0.0 : (double)item.MappedPCTotal),
                    TotalCourseCount = item.TotalCourseCount,
                    UnitType = item.QualificationType,
                    UserId = item.UserId,
                    QualificationName = qualificationName
                };
            }).ToList();
        }
    }
}