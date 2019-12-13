using System.ComponentModel.DataAnnotations;

namespace Homework.NetCore.ContosoUniversity.API.Models
{
    /// <summary>
    /// 預存程序-部門新增後回傳的結果
    /// </summary>
    public class SpDepartmentInsertResult
    {
        [Key]
        public int DepartmentId { get; set; }

        public byte[] RowVersion { get; set; }
    }
}