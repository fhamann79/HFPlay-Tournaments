using Domain;

namespace Backend.Tenancy
{
    public interface ITenantContext
    {
        bool IsResolved { get; }

        int? CurrentTenantId { get; }

        string CurrentTenantSlug { get; }

        Tenant CurrentTenant { get; }
    }
}
