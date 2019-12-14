using System.ComponentModel.DataAnnotations;

namespace Homework.NetCore.ContosoUniversity.API.Models.ViewModels
{
    public class PatchCourseViewModel
    {
        [StringLength(50)]
        public string Title { get; set; }

        [Range(1,5)]
        public int Credits { get; set; }
    }
}