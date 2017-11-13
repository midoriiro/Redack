using Redack.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Redack.DatabaseLayer.DataAccess
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
	{
		private readonly IDbContext _context;
		private readonly DbSet<TEntity> _entities;

		private bool Disposable { get;} = true;
		public bool Disposed { get; private set; } = false;

		public Repository(IDbContext context = null)
		{
			if (context is null)
				this._context = new RedackDbContext();
			else
				this._context = context;

			this._entities = this._context.Set<TEntity>();
		}

		public Repository(IDbContext context, bool disposable)
		{
			this.Disposable = disposable;
			this._context = context;

			this._entities = this._context.Set<TEntity>();
		}

		public IQueryable<TEntity> All()
		{
			try
			{
				var entity = (TEntity) Activator.CreateInstance<TEntity>();

				return entity.Filter(this._entities) as IQueryable<TEntity>;
			}
			catch (NotImplementedException)
			{
				return this._entities;
			}
		}

		public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
		{
			return this.All().Where(predicate);
		}

		public List<TEntity> GetAll()
		{
			return this.All().ToList<TEntity>();
		}

		public async Task<List<TEntity>> GetAllAsync()
		{
			return await this.All().ToListAsync<TEntity>();
		}

		public TEntity GetById(int id)
		{
			var s = this.All().ToList();

			return this.All().Where(e => e.Id == id).ToList().Find(e => e.Id == id);
		}

		public async Task<TEntity> GetByIdAsync(int id)
		{
			var q = await this.All().Where(e => e.Id == id).ToListAsync();

			return q.Find(e => e.Id == id);
		}

		public TEntity GetOrInsert(TEntity entity)
		{
			TEntity obj = this.GetById(entity.Id);

			if (obj != null) return obj;

			this.Insert(entity);

			return entity;
		}

		public void InsertOrUpdate(TEntity entity)
		{
			if (!this.Exists(entity))
				this.Insert(entity);
			else
				this.Update(entity);
		}

		public void Insert(TEntity entity)
		{
			this._entities.Add(entity);
		}

		public void Update(TEntity entity)
		{
			if (this._context.Entry(entity).State == EntityState.Detached)
				this._entities.Attach(entity);

			this._context.SetEntityState(entity, EntityState.Modified);
		}

		public void Delete(TEntity entity)
		{
			if(this._context.Entry(entity).State == EntityState.Detached)
				this._entities.Attach(entity);
			try
			{
				List<Entity> entities = entity.Delete();

				if (entities != null)
				{
					foreach (var e in entities)
					{
						this._context.Entry(e).State = EntityState.Unchanged;
						this._context.Entry(e).State = EntityState.Deleted;
					}
				}
			}
			catch (NotImplementedException) { }

			if(entity.CanBeDeleted)
				this._entities.Remove(entity);
		}

		public void Commit()
		{
			this._context.SaveChanges();
		}

		public async Task<int> CommitAsync()
		{
			return await this._context.SaveChangesAsync();
		}

		public void Rollback()
		{
			this._context.Rollback();
		}

		public bool Exists(TEntity entity)
		{
			return this._entities.Any(e => e.Id == entity.Id);
		}

		public async Task<bool> ExistsAsync(TEntity entity)
		{
			return await this._entities.AnyAsync(e => e.Id == entity.Id);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.Disposed)
			{
				if (disposing)
					this._context.Dispose();

				this.Disposed = true;
			}
		}

		public void Dispose()
		{
			if (this.Disposable)
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
		}
	}
}
