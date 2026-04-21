using System.Threading.Tasks;
using DNR26V2.Domain.Entities.System;

namespace DNR26V2.Services.System;

public interface IAppSetupService
{
    Task<AppSetup> GetAsync();
    Task SaveAsync(AppSetup setup);
}