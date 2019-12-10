using System;

namespace Homework.NetCore.ContosoUniversity.API.Models
{
    public partial class Course : IDateModified
    {
        public DateTime? DateModified { get; set; }
    }
}