using System;

namespace Homework.NetCore.ContosoUniversity.API.Models
{
    public partial class Department : IDateModified, IIsDeleted
    {
        public DateTime? DateModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}