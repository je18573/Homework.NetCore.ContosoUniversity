using System;

namespace Homework.NetCore.ContosoUniversity.API.Models
{
    public partial class Course : IDateModified, IIsDeleted
    {
        public DateTime? DateModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}