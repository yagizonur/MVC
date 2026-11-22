using _06_Dependency_Injection.Models;
using _06_Dependency_Injection.Services.Abstract;

namespace _06_Dependency_Injection.Services.Concrete
{
    public class MyService : IMyService
    {
        public List<Student> GetStudents()
        {
            return new List<Student>
            {
                new Student { Id = 1, Name = "Efe Üstün", Department = "Yazılım Mühendisliği", Gpa = 3.50},
                new Student { Id = 2, Name = "Bestami Köğce", Department = "Bilgisayar Mühendisliği", Gpa = 1.40},
                new Student { Id = 3, Name = "Kaan Budak", Department = "Makine Mühendisliği", Gpa = 2.70},

            };
        }
    }
}
