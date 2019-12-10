using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Homework.NetCore.ContosoUniversity.API.Models
{
    public partial class ContosouniversityContext
    {
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity Name: {entry.Entity.GetType().FullName}");
                Console.WriteLine($"Status: {entry.State}");

                UpdateDateModified(entry);
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity Name: {entry.Entity.GetType().FullName}");
                Console.WriteLine($"Status: {entry.State}");

                UpdateDateModified(entry);
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateDateModified(EntityEntry entry)
        {
            if (entry.State == EntityState.Modified)
                entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });

        }
    }
}