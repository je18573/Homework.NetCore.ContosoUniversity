﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homework.NetCore.ContosoUniversity.API.Models
{
    public partial class VwCourseStudentCount
    {
        [Column("DepartmentID")]
        public int? DepartmentId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Column("CourseID")]
        public int CourseId { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        public int? StudentCount { get; set; }
    }
}