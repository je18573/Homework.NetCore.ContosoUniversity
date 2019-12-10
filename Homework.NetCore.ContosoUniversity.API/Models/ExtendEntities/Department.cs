using System;

namespace Homework.NetCore.ContosoUniversity.API.Models
{
    public partial class Department : IDateModified
    {
        public DateTime? DateModified { get; set; }
    }
}