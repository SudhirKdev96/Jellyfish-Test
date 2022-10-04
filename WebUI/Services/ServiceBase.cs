using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebUI.Data;
using WebUI.Data.Interfaces;
using WebUI.Data.Models;
using WebUI.ViewModels;

namespace WebUI.Services
{
    public class ServiceBase
    {
        // protected field set in constructor
        protected readonly string currentUserId;

        protected readonly ApplicationDbContext<ApplicationUser> context;

        public ServiceBase(ApplicationDbContext<ApplicationUser> context, string currentUserId)
        {
            this.context = context;
            this.currentUserId = currentUserId;
        }

        #region IAuditable Methods
        /// <summary>
        /// Mark an entity created for auditing
        /// </summary>
        /// <param name="entity">the entity to mark as created</param>
        public void MarkCreated(IAuditable entity)
        {
            entity.CreatedById = currentUserId;
            entity.CreatedDateTime = DateTime.Now;
        }

        /// <summary>
        /// Mark an entity as modified for auditing
        /// </summary>
        /// <param name="entity">the entity to mark as modified</param>
        public void MarkModified(IAuditable entity)
        {
            entity.ChangedById = currentUserId;
            entity.ChangedDateTime = DateTime.Now;
        }

        /// <summary>
        /// Reset the modified fields and mark an entity as created
        /// </summary>
        /// <param name="entity">the item to reset</param>
        public void Reset(IAuditable entity)
        {
            // reset modified fields
            entity.ChangedById = null;
            entity.ChangedDateTime = null;
            // set created fields
            MarkCreated(entity);
        }
        #endregion

        #region ISoftDeletable Methods
        /// <summary>
        /// Soft-delete an item
        /// </summary>
        /// <param name="entity">the entity to delete</param>
        public void MarkDeleted(ISoftDeletable entity)
        {
            entity.Deleted = true;
            entity.DeletedById = currentUserId;
            entity.DeletedDateTime = DateTime.Now;
        }

        /// <summary>
        /// Un-delete a soft-deleted item, updating the IAuditable fields
        /// </summary>
        /// <param name="entity">the item to restore</param>
        /// <returns>-1 if <c>entity</c> is null; -2 if it's not currently deleted; otherwise, the number of records affected</returns>
        public virtual int Restore(ISoftDeletable entity)
        {
            if (entity == null) return -1;

            if (!entity.Deleted) return -2;

            Track(entity);

            entity.Deleted = false;
            MarkModified(entity);

            return context.SaveChanges();
        }

        /// <summary>
        /// (async) Un-delete a soft-deleted item, updating the IAuditable fields
        /// </summary>
        /// <param name="entity">the item to restore</param>
        /// <returns>-1 if <c>entity</c> is null; -2 if it's not currently deleted; otherwise, the number of records affected</returns>
        public async virtual Task<int> RestoreAsync(ISoftDeletable entity)
        {
            if (entity == null) return -1;

            if (!entity.Deleted) return -2;

            Track(entity);

            entity.Deleted = false;
            MarkModified(entity);

            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Unset the ISoftDeletable fields, don't touch the IAuditable fields
        /// </summary>
        /// <param name="entity">the item to unset</param>
        public void Unset(ISoftDeletable entity)
        {
            entity.Deleted = false;
            entity.DeletedById = null;
            entity.DeletedDateTime = null;
        }
        #endregion

        /// <summary>
        /// Attach an entity for explicit change tracking
        /// </summary>
        public void Track<TEntity>(TEntity entity) where TEntity : class
        {
            context.Attach(entity);
        }

        /// <summary>
        /// Attach a collection of entities for explicit change tracking
        /// </summary>
        public void TrackAll<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                Track(entity);
            }
        }

        /// <summary>
        ///  Detach an entity to stop change tracking
        /// </summary>
        public void StopTracking(object entity)
        {
            context.Detach(entity);
        }

        /// <summary>
        /// Get the active DB context for this service
        /// </summary>
        public ApplicationDbContext<ApplicationUser> GetContext()
        {
            return context;
        }

        /// <summary>
        /// Get a single entity of type T by primary key.
        /// </summary>
        /// <typeparam name="T">The type of entity to get.</typeparam>
        /// <param name="pk">The primary key of the entity to get.</param>
        /// <returns>the entity of type T with primary key pk or null if not found</returns>
        public T Find<T>(object pk) where T : class
        {
            return context.Set<T>().Find(pk);
        }

        /// <summary>
        /// Get a single entity of type T by primary key asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of entity to get.</typeparam>
        /// <param name="pk">The primary key of the entity to get.</param>
        /// <returns>a Task that returns the entity of type T with primary key pk or null if not found</returns>
        public async Task<T> FindAsync<T>(object pk) where T : class
        {
            return await context.Set<T>().FindAsync(pk);
        }

        /// <summary>
        /// Prepare a sequence of the given type, filtered by optional filter param.
        /// Optionally include soft-deleted items of the given type, as well.
        /// </summary>
        /// <param name="asUntracked">should the output of this sequence be excluded from tracking by the DB context?</param>
        /// <param name="filter">a function that filters, includes subtypes, or otherwise builds on or
        ///     manipulates the queryable before fetching the set of data. Null is a valid value if no filtering is required.</param>
        /// <param name="includeDeleted">If <c>T</c> is a soft-deletable type, should deleted items be returned in the output?</param>
        public virtual IQueryable<T> GetQueryable<T>(bool asUntracked, Func<IQueryable<T>, IQueryable<T>> filter, bool includeDeleted)
            where T : class
        {
            IQueryable<T> queryable = context.Set<T>();

            // apply optional filter param if it is not null
            if (filter != null)
            {
                queryable = filter(queryable);
            }

            // return untracked entities if the asReadOnly is true
            if (asUntracked)
            {
                queryable = queryable.AsNoTracking();
            }

            // unless requested otherwise, exclude deleted records for soft deletable entities
            if (!includeDeleted && queryable is IQueryable<ISoftDeletable> deletable)
            {
                // if T implements ISoftDeletable, filter out records marked deleted and cast back to T
                queryable = deletable.Where(x => x.Deleted == false).Select(x => (T)x);
            }

            return queryable;
        }

        /// <summary>
        /// Prepare a sequence of all non-deleted items of the given type, and optionally including the soft-deleted items, as well
        /// </summary>
        /// <param name="asUntracked">should the output of this sequence be excluded from tracking by the DB context?</param>
        /// <param name="includeDeleted">If <c>T</c> is a soft-deletable type, should deleted items be returned in the output?</param>
        public virtual IQueryable<T> GetQueryable<T>(bool asUntracked, bool includeDeleted)
            where T : class
        {
            return GetQueryable<T>(asUntracked, null, includeDeleted);
        }

        /// <summary>
        /// Prepare a sequence of the given type, filtered by optional filter param.
        /// If <c>T</c> is a soft-deletable type, this sequence will exclude deleted items.
        /// </summary>
        /// <param name="asUntracked">should the output of this sequence be excluded from tracking by the DB context?</param>
        /// <param name="filter">a function that filters, includes subtypes, or otherwise builds on or
        ///     manipulates the queryable before fetching the set of data</param>
        public virtual IQueryable<T> GetQueryable<T>(bool asUntracked, Func<IQueryable<T>, IQueryable<T>> filter = null)
            where T : class
        {
            return GetQueryable(asUntracked, filter, false);
        }

        /// <summary>
        /// Prepare a sequence of the given type, filtered by optional filter param.
        /// If <c>T</c> is a soft-deletable type, this sequence will include deleted items.
        /// </summary>
        /// <param name="asUntracked">should the output of this sequence be excluded from tracking by the DB context?</param>
        /// <param name="filter">a function that filters, includes subtypes, or otherwise builds on or
        ///     manipulates the queryable before fetching the set of data</param>
        public virtual IQueryable<T> GetQueryableIncludingDeleted<T>(bool asUntracked, Func<IQueryable<T>, IQueryable<T>> filter = null)
            where T : class
        {
            return GetQueryable(asUntracked, filter, true);
        }

        /// <summary>
        /// List all records of one type, in no particular order
        /// </summary>
        /// <param name="includeDeleted">optional arg to include records marked as deleted, false by default</param>
        public virtual List<T> ListAll<T>(bool asUntracked = false) where T : class
        {
            return GetQueryable<T>(asUntracked: asUntracked).ToList();
        }

        /// <summary>
        /// ASync List all records of one type, in no particular order
        /// </summary>
        /// <param name="includeDeleted">optional arg to include records marked as deleted, false by default</param>
        public virtual async Task<List<T>> ListAllAsync<T>(bool asUntracked = false) where T : class
        {
            return await GetQueryable<T>(asUntracked: asUntracked).ToListAsync();
        }

        /// <summary>
        /// List all records of the given type as SelectOptions, ordered by display name
        /// </summary>
        public async Task<IOrderedEnumerable<SelectOption<TKey>>> GetSelectOptions<TItem, TKey>(Func<IQueryable<TItem>, IQueryable<TItem>> filter = null)
            where TItem : class, ISelectOption<TKey>
        {
            var q = GetQueryable(asUntracked: true, filter: filter);
            var list = await q.AsSelectOptions<TItem, TKey>().ToListAsync();
            return list.OrderBy(o => o.DisplayName);
        }

        /// <summary>
        /// Get a sorted list of one field from all the entities of the given type, optionally filtered
        /// </summary>
        /// <typeparam name="TItem">the entity type to lookup</typeparam>
        /// <typeparam name="TValue">the type of the members of the returned list</typeparam>
        /// <param name="outputField">the field expression for the members of the returned list</param>
        /// <param name="filter">a predicate for filtering members of the requested type</param>
        /// <returns>A sorted list of <c>TValue</c>s. If they're strings, this sort ignores case.
        /// Otherwise, it uses the natural sort order of <c>TValue</c></returns>
        public async Task<List<TValue>> GetFieldValues<TItem, TValue>(Expression<Func<TItem, TValue>> outputField,
            bool distinctValuesOnly, Func<IQueryable<TItem>, IQueryable<TItem>> filter = null)
            where TItem : class
        {
            var q = GetQueryable(asUntracked: true, filter: filter).Select(outputField);

            if (distinctValuesOnly)
            {
                q = q.Distinct();
            }

            var list = await q.ToListAsync();

            if (list is List<string> stringList)
            {
                stringList.Sort(StringComparer.CurrentCultureIgnoreCase);
            }
            else
            {
                list.Sort();
            }

            return list;
        }

        /// <summary>
        /// Do the necessary steps to mark an entity deleted before persisting changes to the DB.
        /// </summary>
        protected virtual void PrepareDelete<T>(T entity) where T : class
        {
            Track(entity);
            if (entity is ISoftDeletable deletable)
            {
                // T implements ISoftDeletable, mark as deleted
                MarkDeleted(deletable);
            }
            else
            {
                // T does not implement ISoftDeletable, [hard] delete
                context.Set<T>().Remove(entity);
            }
        }

        /// <summary>
        /// Delete an entity from the database, or mark it as deleted if it implements ISoftDeletable
        /// </summary>
        /// <typeparam name="T">Generic Type <c>T</c>, may implement ISoftDeletable</typeparam>
        /// <param name="entity">the entity to be deleted, or marked as deleted</param>
        /// <param name="doPersist">Should the delete be persisted to the DB now? Defaults to yes</param>
        public virtual void Delete<T>(T entity, bool doPersist) where T : class
        {
            PrepareDelete(entity);
            if (doPersist)
            {
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete an entity from the database, or mark it as deleted if it implements ISoftDeletable.
        /// Persists changes to the DB immediately.
        /// </summary>
        /// <typeparam name="T">Generic Type <c>T</c>, may implement ISoftDeletable</typeparam>
        /// <param name="entity">the entity to be deleted, or marked as deleted</param>
        public virtual void Delete<T>(T entity) where T : class
        {
            Delete(entity, true);
        }

        /// <summary>
        /// (async) Delete an entity from the database, or mark it as deleted if it implements ISoftDeletable
        /// </summary>
        /// <typeparam name="T">Generic Type <c>T</c>, may implement ISoftDeletable</typeparam>
        /// <param name="entity">the entity to be deleted, or marked as deleted</param>
        /// <param name="doPersist">Should the delete be persisted to the DB now? Defaults to yes</param>
        public async virtual Task DeleteAsync<T>(T entity, bool doPersist) where T : class
        {
            PrepareDelete(entity);
            if (doPersist)
            {
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// (async) Delete an entity from the database, or mark it as deleted if it implements ISoftDeletable.
        /// Persists changes to the DB immediately.
        /// </summary>
        /// <typeparam name="T">Generic Type <c>T</c>, may implement ISoftDeletable</typeparam>
        /// <param name="entity">the entity to be deleted, or marked as deleted</param>
        public async virtual Task DeleteAsync<T>(T entity) where T : class
        {
            await DeleteAsync(entity, true);
        }

        /// <summary>
        /// Clone an entity
        /// </summary>
        /// <param name="entity">the entity to clone</param>
        /// <returns>a new entity with values copied from entity</returns>
        public T CloneEntity<T>(T entity, bool keepAuditValues) where T : new()
        {
            if (entity == null) return default;

            var clone = new T();
            CopyEntityValues(entity, clone);
            // if we are not keeping the audit values, try to remove them
            if (!keepAuditValues)
            {
                // if T implements IAuditable, clear those fields
                if (clone is IAuditable auditable)
                {
                    // unset IAuditable fields
                    Reset(auditable);
                }
                // if T implements ISoftDeletable, clear those fields
                if (clone is ISoftDeletable deletable)
                {
                    // unset ISoftDeletable fields
                    Unset(deletable);
                }
            }

            return clone;
        }

        /// <summary>
        /// Copy values from one entity into another of the same type
        /// </summary>
        /// <param name="from">the source entity</param>
        /// <param name="to">the entity to populate</param>
        public void CopyEntityValues<T>(T from, T to)
        {
            if (from == null || to == null) return;

            context.Entry(to).CurrentValues.SetValues(from);
        }

        /// <summary>
        /// create a new instance of T, marking as created if T implements IAuditable
        /// </summary>
        /// <typeparam name="T">the EF entity type to create</typeparam>
        /// <returns></returns>
        public T New<T>() where T : class, new()
        {
            T entity = new T();
            if (entity is IAuditable auditable)
            {
                MarkCreated(auditable);
            }
            return entity;
        }

        /// <summary>
        /// load a single entity of type T, by passing in an expression to filter
        /// </summary>
        /// <typeparam name="T">the EF entity type to load</typeparam>
        /// <param name="predicate">the expression should match 0 or 1 records by id field, or another unique field
        /// (essentially a sql "where" condition)</param>
        /// <param name="canCreateNew">a flag indicating whether a new object of type T should be returned if DB retrieval fails</param>
        /// <returns>A single record of type T, retrieved from DB or a new record if filter doesn't match any record</returns>
        public T Load<T>(Expression<Func<T, bool>> predicate, bool canCreateNew = true) where T : class, new()
        {
            var query = context.Set<T>().Where(predicate);
            T entity = canCreateNew ? query.FirstOrNew() : query.FirstOrDefault();
            if (entity is null)
            {
                throw new Exception("Faled to load entity and cannot create a new one");
            }
            else
            {
                return entity;
            }
        }

        /// <summary>
        /// persist a single entity of type T to the database
        /// </summary>
        /// <typeparam name="T">the type of EF entity to be persisted</typeparam>
        /// <param name="entity">the entity to persist</param>
        /// <param name="idField">expression to expose the id field of this entity</param>
        /// <returns>The number of state entries written to the database</returns>
        public int Save<T>(T entity, Expression<Func<T, int>> idField) where T : class
        {
            var auditable = entity as IAuditable;

            try
            {
                if ((idField?.Compile()?.Invoke(entity) ?? 0) == 0)
                {
                    // new T
                    if (auditable != null)
                    {
                        MarkCreated(auditable);
                    }
                    context.Add(entity);
                }
                else
                {
                    if (auditable != null)
                    {
                        MarkModified(auditable);
                    }
                }

                return context.SaveChanges();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                Debug.WriteLine($"ServiceBase.Save exception: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// persist a single entity of type T to the database
        /// </summary>
        /// <typeparam name="T">the type of EF entity to be persisted</typeparam>
        /// <param name="entity">the entity to persist</param>
        /// <param name="idField">expression to expose the id field of this entity</param>
        /// <returns>The number of state entries written to the database</returns>
        public virtual async Task<int> SaveAsync<T>(T entity, Expression<Func<T, int>> idField) where T : class
        {
            var auditable = entity as IAuditable;

            try
            {
                if ((idField?.Compile()?.Invoke(entity) ?? 0) == 0)
                {
                    // new T
                    if (auditable != null)
                    {
                        MarkCreated(auditable);
                    }
                    context.Add(entity);
                }
                else
                {
                    if (auditable != null)
                    {
                        MarkModified(auditable);
                    }
                }

                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                Debug.WriteLine($"KamsService.Save exception: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Persist any pending changes
        /// </summary>
        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        /// <summary>
        /// Persist any pending changes asynchronously
        /// </summary>
        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        /// <summary>
        /// helper method - get user by guid
        /// </summary>
        /// <param name="guid">The id of the user to fetch</param>
        /// <returns></returns>
        public ApplicationUser GetUserByGuid(string guid)
        {
            return Load<ApplicationUser>(x => x.Id == guid);
        }

        /// <summary>
        /// helper method - get current application user, 
        /// using the currentUserId that ServiceBase is instantiated with.
        /// </summary>
        /// <returns>The current user</returns>
        public ApplicationUser GetCurrentUser()
        {
            return Load<ApplicationUser>(x => x.Id == currentUserId);
        }
    }
}
