using RailEmu.Auth.Database.Models;
using System;
using System.Threading.Tasks;

namespace RailEmu.Auth.Database.Interfaces
{
    public interface IAccountRepository
    {
        void Initialize();
        Task<Account> GetAccountByName(string name);
        Task<Account> GetAccountByUId(Guid uId);
        Task SaveAccount(Account account);
        Task<long> GetAccountCount();
    }
}
