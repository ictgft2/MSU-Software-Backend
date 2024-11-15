using MedicalUnitSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicalUnitSystem.Repositories.Contracts
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected HospitalContext Context { get; set; }
        public Repository(HospitalContext context)
        {
            Context = context;
        }
        public IQueryable<T> FindAll() => Context.Set<T>().AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            Context.Set<T>().Where(expression).AsNoTracking();
        public void Create(T entity) => Context.Set<T>().Add(entity);
        public void Update(T entity) => Context.Set<T>().Update(entity);
        public void Delete(T entity) => Context.Set<T>().Remove(entity);
    }
}
