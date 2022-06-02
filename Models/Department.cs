using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
    }

}