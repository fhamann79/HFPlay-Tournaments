using Backend.Models;
using System;
using System.Linq;
using System.Web;

namespace Backend.Tenancy
{
    public class TenantResolver
    {
        private const string TenantHeaderName = "X-Tenant";
        private const string TenantQueryStringKey = "tenant";
        private const string TenantContextHttpItemsKey = "HFPlay.CurrentTenantContext";

        public ITenantContext GetCurrentTenantContext()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return new TenantContext((string)null);
            }

            var existingContext = httpContext.Items[TenantContextHttpItemsKey] as ITenantContext;
            if (existingContext != null)
            {
                return existingContext;
            }

            var tenantSlug = GetTenantSlug(httpContext.Request);
            if (string.IsNullOrWhiteSpace(tenantSlug))
            {
                var unresolvedContext = new TenantContext((string)null);
                httpContext.Items[TenantContextHttpItemsKey] = unresolvedContext;
                return unresolvedContext;
            }

            tenantSlug = tenantSlug.Trim().ToLowerInvariant();

            using (var db = new DataContextLocal())
            {
                var tenant = db.Tenants
                    .FirstOrDefault(t => t.Slug == tenantSlug && t.IsActive);

                ITenantContext tenantContext = tenant != null
                    ? (ITenantContext)new TenantContext(tenant)
                    : new TenantContext(tenantSlug);

                httpContext.Items[TenantContextHttpItemsKey] = tenantContext;
                return tenantContext;
            }
        }

        private static string GetTenantSlug(HttpRequest request)
        {
            var tenantHeader = request.Headers[TenantHeaderName];
            if (!string.IsNullOrWhiteSpace(tenantHeader))
            {
                return tenantHeader;
            }

            var tenantQueryString = request.QueryString[TenantQueryStringKey];
            if (!string.IsNullOrWhiteSpace(tenantQueryString))
            {
                return tenantQueryString;
            }

            return null;
        }
    }
}
