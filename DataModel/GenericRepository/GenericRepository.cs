using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.GenericRepository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal WebApiDBEntities Context;
        internal DbSet<TEntity> Dbset;

        public GenericRepository(WebApiDBEntities context)
        {
            this.Context = context;
            this.Dbset = context.Set<TEntity>();
        }


        /// <summary>         
        /// generic Get method for Entities  
        /// </summary>         
        /// <returns></returns> 
        public virtual IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = Dbset;
            return query.ToList();
        }

        /// <summary>        
        /// Generic get method on the basis of id for Entities.        
        /// </summary>         
        /// <param name="id"></param> 
        /// <returns></returns> 
        public virtual TEntity GetById(object id)
        {
            return Dbset.Find(id);
        }

        /// <summary>         \
        /// generic Insert method for the entities        
        /// </summary>         
        /// <param name="entity"></param> 
        public virtual void Insert(TEntity entity)
        {
            Dbset.Add(entity);
        }

        /// <summary>        
        /// Generic Delete method for the entities         
        /// </summary>         
        /// <param name="id"></param> 
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = Dbset.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>         
        /// Generic Delete method for the entities  
        /// </summary>
        /// <param name="entityToDelete"></param> 
        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                Dbset.Attach(entityToDelete);
            }
            Dbset.Remove(entityToDelete);
        }

        /// <summary> 
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            Dbset.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary> 
        /// generic method to get many record on the basis of a condition.  
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns> 
        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return Dbset.Where(where).ToList();
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary> 
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            return Dbset.Where(where).AsQueryable();
        }
        /// <summary>  
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>         
        /// <returns></returns>
        public TEntity Get(Func<TEntity, Boolean> where)
        {
            return Dbset.Where(where).FirstOrDefault<TEntity>();
        }
        /// <summary> 
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>       
        /// <param name="where"></param>
        /// <returns></returns> 
        public void Delete(Func<TEntity, Boolean> where)
        {
            IQueryable<TEntity> objects = Dbset.Where<TEntity>(where).AsQueryable();
            foreach (TEntity obj in objects)
                Dbset.Remove(obj);
        }

        /// <summary>  
        /// generic method to fetch all the records from db 
        /// </summary>    
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return Dbset.ToList();
        }
        /// <summary> 
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param> 
        /// <returns></returns>
        public IQueryable<TEntity> GetWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.Dbset;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }
        /// <summary> 
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public bool Exists(object primaryKey)
        {
            return Dbset.Find(primaryKey) != null;
        }

        /// <summary> 
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public TEntity GetSingle(Func<TEntity, bool> predicate)
        {
            return Dbset.Single<TEntity>(predicate);
        }

        /// <summary> 
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public TEntity GetFirst(Func<TEntity, bool> predicate)
        {
            return Dbset.First<TEntity>(predicate);
        } 










    }
}
