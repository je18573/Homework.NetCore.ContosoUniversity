using System;

namespace Homework.NetCore.ContosoUniversity.API.Models
{
    public partial class Person : IDateModified
    {
        public DateTime? DateModified { get; set; }
    }
}