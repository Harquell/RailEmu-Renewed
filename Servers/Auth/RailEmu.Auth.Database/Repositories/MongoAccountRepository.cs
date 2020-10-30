using MongoDB.Driver;
using RailEmu.Auth.Database.Interfaces;
using RailEmu.Auth.Database.Models;
using RailEmu.Database.Services;
using System;
using System.Threading.Tasks;

namespace RailEmu.Auth.Database.Repositories
{
    public class MongoAccountRepository : IAccountRepository
    {
        private readonly IMongoDatabase database;
        private IMongoCollection<Account> accountCollection;


        public MongoAccountRepository(MongoDatabaseManager mongoDatabaseManager)
        {
            database = mongoDatabaseManager.GetDatabase();
        }

        public void Initialize()
        {
            accountCollection = database.GetCollection<Account>("Account");
        }

        public async Task<Account> GetAccountByName(string name)
        {
            return await accountCollection.Find(x => x.Username == name).SingleAsync();
        }

        public async Task<Account> GetAccountByUId(Guid uId)
        {
            return await accountCollection.Find(x => x.UId == uId).SingleAsync();
        }

        public async Task SaveAccount(Account account)
        {
            await accountCollection.ReplaceOneAsync(x => x.UId == account.UId, account, new ReplaceOptions() { IsUpsert = true });
        }

        public async Task<long> GetAccountCount()
        {
            return await accountCollection.CountDocumentsAsync(x => true);
        }
    }
}
