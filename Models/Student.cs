using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi
{
    public class Student
    {
        [Key]
        public int StudentId {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string DateOfBirth {get;set;}
        public string Mobile {get;set;}
        public string Email {get;set;}

        public string Departments {get;set;}

    }

}