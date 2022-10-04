using WebUI.Services;

namespace WebUI.Components
{
    public partial class ManagementPage<TItem, TService> 
        where TItem : class, new() 
        where TService : ServiceBase { }
}
