using Domain;

namespace Backend.Tenancy
{
    public class TenantContext : ITenantContext
    {
        public TenantContext(Tenant tenant)
        {
            CurrentTenant = tenant;
            CurrentTenantId = tenant != null ? (int?)tenant.TenantId : null;
            CurrentTenantSlug = tenant != null ? tenant.Slug : null;
            IsResolved = tenant != null;
        }

        public TenantContext(string tenantSlug)
        {
            CurrentTenantSlug = tenantSlug;
            IsResolved = false;
        }

        public bool IsResolved { get; private set; }

        public int? CurrentTenantId { get; private set; }

        public string CurrentTenantSlug { get; private set; }

        public Tenant CurrentTenant { get; private set; }
    }
}
