using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebUI.Data.Models.Reports;
using WebUI.ViewModels;

namespace WebUI.Data
{
    public static class ApplicationDbContextExtension
    {
        /// <summary>
        /// Detach an entity from this context to stop tracking
        /// </summary>
        public static void Detach(this ApplicationDbContext<ApplicationUser> context, object entity)
        {
            if (entity == null) return;

            context.Entry(entity).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Extends ApplicationDbContext to register additional models as entities 
    /// in order to fill them easily with stored procedure results
    /// </summary>
    public partial class ApplicationDbContext<TUser> : IdentityDbContext<TUser, IdentityRole, string>
        where TUser : IdentityUser
    {
        // Define additional DBSets here, such as ViewModels that are not mapped to DB
        public virtual DbSet<ClientProjectRow> ClientProjectRows { get; set; }
        
        // use RegisterTablelessModel for things like ViewModels that aren't mapped to the DB
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            // register models with no key that are just for reading data
            RegisterTablelessModel<ClientProjectRow>(modelBuilder, nameof(ClientProjectRow));
        }

        /// <summary>
        /// Have the builder add an entity that does not require a key and should not be created as an actual table in the DB
        /// </summary>
        /// <typeparam name="TEntity">the entity type</typeparam>
        /// <param name="modelBuilder">the model builder for this context</param>
        /// <param name="dbSetName">the name of the DbSet that contains elements of this entity type. This should match the
        /// actual property names defined above in this class</param>
        private static void RegisterTablelessModel<TEntity>(ModelBuilder modelBuilder, string dbSetName) where TEntity : class
        {
            // The irony of having to call "ToTable" in order to set the flag that tells migration not to create a table...
            modelBuilder.Entity<TEntity>(entity => 
            { 
                entity.HasNoKey(); 
                entity.ToTable(dbSetName, x => x.ExcludeFromMigrations()); 
            });
        }
    }
}
