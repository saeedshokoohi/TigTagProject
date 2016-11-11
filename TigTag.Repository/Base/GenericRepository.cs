using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TigTag.Common.Enumeration;

namespace TiTag.Repository.Base{

    public abstract class GenericRepository<C, T> :

        IGenericRepository<T> where T : class where C : DbContext, new() {

        public int profileTypeCode = enmPageTypes.PROFILE.GetHashCode();
        public int postTypeCode = enmPageTypes.POST.GetHashCode();
        public int pageTypeCode = enmPageTypes.PAGE.GetHashCode();
        public int teamTypeCode = enmPageTypes.TEAM.GetHashCode();
        public bool contextChanged = false;
        private C _entities = new C();
        public C Context {

            get { return _entities; }
            set { _entities = value; contextChanged = true; }
        }
  

        public virtual T GetSingle(Guid Id)
        {
           
            return null;
        }
        public virtual IQueryable<T> GetAll() {

            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate) {

            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity) {
            _entities.Set<T>().Add(entity);
        }
        public virtual void AddOrUpdate(T entity)
        {
            T e= GetSingle(getIdField(entity));
            if (e != null)
                Edit(entity);
            else
                Add(entity);
        }

        private Guid getIdField(T entity)
        {
            try
            {
                return Guid.Parse(entity.GetType().GetProperty("Id").GetValue(entity).ToString());
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public virtual void Delete(T entity) {

            _entities.Set<T>().Remove(entity);
        }
    
        public void DeleteList(List<T> delList)
        {
            foreach (var item in delList)
            {
                Delete(item);
            }
        }
        
        public virtual void Edit(T entity) {

            _entities.Entry(entity).State =EntityState.Modified;
        }
     public virtual void Detach(T entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }
        public virtual void Save() {
            if(!contextChanged)
            _entities.SaveChanges();
        }
        public virtual IQueryable<T> query()
        {
            return _entities.Set<T>().AsQueryable();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {

            if (!this.disposed)
                if (disposing)
                    _entities.Dispose();

            this.disposed = true;
        }


        public void Dispose() {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool isValidId(Guid id)
        {
            if (id == null) return false;
            var o= GetSingle(id);
            if (o != null) return true;
            else return false;
        }
    }
}