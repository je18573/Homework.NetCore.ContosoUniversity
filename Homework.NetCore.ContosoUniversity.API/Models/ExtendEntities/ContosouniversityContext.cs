using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Homework.NetCore.ContosoUniversity.API.Models
{
    public partial class ContosouniversityContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Department>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Course>().HasQueryFilter(p => !p.IsDeleted);
        }

        public override int SaveChanges()
        {
            UpdateDateModified();
            MarkAsSoftDeleted();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateDateModified();
            MarkAsSoftDeleted();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Updates the date modified.
        /// 更新 <see cref="EntityState"/> 為修改狀態的「DateModified」欄位
        /// </summary>
        private void UpdateDateModified()
        {
            var updateEntries = ChangeTracker.Entries().Where(w => w.State == EntityState.Modified);
            foreach (var entry in updateEntries)
            {
                entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
            }
        }

        /// <summary>
        /// Marks as soft deleted.
        /// 標記為軟刪除
        /// </summary>
        private void MarkAsSoftDeleted()
        {
            var deleteEntries = ChangeTracker.Entries().Where(w => w.State == EntityState.Deleted);

            foreach (var entry in deleteEntries)
            {
                if (entry.Entity is IIsDeleted)
                {
                    entry.State = EntityState.Unchanged;
                    entry.CurrentValues.SetValues(new { IsDeleted = true });
                }
            }
        }
    }
}