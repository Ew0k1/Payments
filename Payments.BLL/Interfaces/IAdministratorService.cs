using Payments.BLL.Infrastructure;

namespace Payments.BLL.Interfaces
{
    public interface IAdministratorService
    {
        OperationDetails BlockUser(string id);

        OperationDetails UnblockUser(string id);
    }
}
