using Backend.Tenancy;
using System.Web.Mvc;

namespace Backend.Controllers
{
    public class TenantDiagnosticsController : Controller
    {
        [HttpGet]
        public ActionResult Resolve()
        {
            var tenantContext = new TenantResolver().GetCurrentTenantContext();

            return Json(new
            {
                tenantContext.IsResolved,
                tenantContext.CurrentTenantId,
                tenantContext.CurrentTenantSlug,
                CurrentTenantName = tenantContext.CurrentTenant != null ? tenantContext.CurrentTenant.Name : null
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
