# Community.AspNetCore.DataProtection.DistributedCache
Use IDistributedCache in Microsoft.Extensions.Caching.Distributed to store keys of Microsoft.AspNetCore.DataProtection.

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddDataProtection().PersistKeysToDistributedStore();
}
```
