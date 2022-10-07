using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebUI.Data.Models;
using WebUI.Data.Models.Reports;

namespace WebUI.Data
{
    public partial class ApplicationDbContext<TUser> : IdentityDbContext<TUser, IdentityRole, string>
        where TUser : IdentityUser
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext<TUser>> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<CompanyProfile> CompanyProfile { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // call base.OnModelCreating so Identity Models are configured properly
            base.OnModelCreating(builder);
            // EF core generally lets the last called win, so override after calling the base method

            builder.Entity<Project>( entity =>
                {
                    entity.Property(x => x.BillingRate).HasColumnType("money");
                    entity.Property(x => x.Deposit).HasColumnType("money");
                    entity.Property(x => x.EstimatedRevenue).HasColumnType("money");
                    entity.Property(x => x.ReferralPercent).HasColumnType("money");

                    entity.HasOne(d => d.CreatedBy)
                        .WithMany(p => p.ProjectCreatedBy)
                        .HasForeignKey(d => d.CreatedById)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Project$tblAspNetUsersCreatedBy");

                    entity.HasOne(d => d.ChangedBy)
                        .WithMany(p => p.ProjectChangedBy)
                        .HasForeignKey(d => d.ChangedById)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Project$tblAspNetUsersChangedBy");
                });

            builder.Entity<Client>( entity =>
            {
                entity.Property(x => x.BillingRate).HasColumnType("money");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.ClientCreatedBy)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Client$tblAspNetUsersCreatedBy");

                entity.HasOne(d => d.ChangedBy)
                    .WithMany(p => p.ClientChangedBy)
                    .HasForeignKey(d => d.ChangedById)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Client$tblAspNetUsersChangedBy");
            });
            OnModelCreatingPartial(builder);
        }

        /// <summary>
        /// Detach an entity from this context to stop tracking
        /// </summary>
        public void Detach(object entity)
        {
            if (entity == null) return;

            Entry(entity).State = EntityState.Detached;
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
