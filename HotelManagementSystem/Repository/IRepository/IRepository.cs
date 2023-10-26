using System.Linq.Expressions;

namespace HotelManagementSystem.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<bool> AddAsync(T entity);    //Create
        Task<T> AddAsyncAndGet(T entity);    //Create

        Task<bool> AddRangeAsync(ICollection<T> entities);
        Task<bool> UpdateAsync(T entity);  //Update
        Task<bool> UpdateRangeAsync(ICollection<T> entities);
        Task<bool> RemoveAsync(T entity);  //Delete by entity
        Task<bool> RemoveAsync(int id);    //Delete by Id
        Task<bool> SaveAsync();
        Task<T> GetAsync(int id);       //Find by Id

        //Display Code
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,      //Filter Data
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, //Order By
            string includeProperties = null    //To get data from Multiple Tables
                       );

        //To find with non primary key or from multiple tables
        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );
    }
}
