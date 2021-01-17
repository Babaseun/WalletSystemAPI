using System.Collections.Generic;
using System.Threading.Tasks;

namespace WalletSystem.Services.Data.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> FindAll();
#nullable enable
        Task<T> Find(string? id);
#nullable disable
        Task Update(T t);
        Task Save(T t);
        //Task Delete(int? id);
        //Task<bool> Exist(int? id);
        //Task<int> Count();
        //Task AddRange(T[] t);
    }
}
