using Payments.DAL.Entities;
using System;

namespace Payments.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(ClientProfile item);
    }
}
