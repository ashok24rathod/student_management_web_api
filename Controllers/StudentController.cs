using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StudentManagementApi;

namespace StudentManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString = string.Empty;
        public StudentController(IConfiguration config)
        {
            configuration = config;
            connectionString = config.GetConnectionString("DefaultConnection");
        }

        // GET: api/Student
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            DataTable dataTable = DBHelper.GetTableFromSP(connectionString, "GetAllStudents", null);
            List<Student> students = (List<Student>)dataTable.ToList<Student>();
            return students;
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            SqlParameter[] sqlParameters = new SqlParameter[1];
            SqlParameter st = new SqlParameter();
            st.ParameterName = "StudentId";
            st.Value = id;
            st.SqlDbType = SqlDbType.Int;

            sqlParameters.Append(st);

            DataTable dataTable = DBHelper.GetTableFromSP(connectionString, "GetStudentById", sqlParameters);
            List<Student> students = (List<Student>)dataTable.ToList<Student>();
            if (students != null && students.Count > 0)
            {
                return students.First();
            }
            else
            {
                return NoContent();
            }
        }

        // PUT: api/Student/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id < 0 || student == null)
            {
                return BadRequest();
            }

            SqlParameter[] sqlParameters = new SqlParameter[7];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "StudentId";
            sqlParameters[0].Value = id;
            sqlParameters[1] = new SqlParameter();
            sqlParameters[1].ParameterName = "FirstName";
            sqlParameters[1].Value = student.FirstName;
            sqlParameters[2] = new SqlParameter();
            sqlParameters[2].ParameterName = "LastName";
            sqlParameters[2].Value = student.LastName;
            sqlParameters[3] = new SqlParameter();
            sqlParameters[3].ParameterName = "DateOfBirth";
            sqlParameters[3].Value = student.DateOfBirth;
            sqlParameters[4] = new SqlParameter();
            sqlParameters[4].ParameterName = "Mobile";
            sqlParameters[4].Value = student.Mobile;
            sqlParameters[5] = new SqlParameter();
            sqlParameters[5].ParameterName = "Email";
            sqlParameters[5].Value = student.Email;

            sqlParameters[6] = new SqlParameter();
            sqlParameters[6].ParameterName = "Departments";
            sqlParameters[6].Value = student.Departments;

            if (DBHelper.ExecuteNonQuery(connectionString, "UpdateStudent", sqlParameters) > 0)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }


        }

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            if (student == null)
            {
                return BadRequest();
            }
            SqlParameter[] sqlParameters = new SqlParameter[6];
            sqlParameters[0] = new SqlParameter();
            sqlParameters[0].ParameterName = "FirstName";
            sqlParameters[0].Value = student.FirstName;
            sqlParameters[1] = new SqlParameter();
            sqlParameters[1].ParameterName = "LastName";
            sqlParameters[1].Value = student.LastName;
            sqlParameters[2] = new SqlParameter();
            sqlParameters[2].ParameterName = "DateOfBirth";
            sqlParameters[2].Value = student.DateOfBirth;
            sqlParameters[3] = new SqlParameter();
            sqlParameters[3].ParameterName = "Mobile";
            sqlParameters[3].Value = student.Mobile;
            sqlParameters[4] = new SqlParameter();
            sqlParameters[4].ParameterName = "Email";
            sqlParameters[4].Value = student.Email;

            sqlParameters[5] = new SqlParameter();
            sqlParameters[5].ParameterName = "Departments";
            sqlParameters[5].Value = student.Departments;

            string id = DBHelper.ExecuteScalarForSP(connectionString, "AddStudent", sqlParameters);
            student.StudentId = Convert.ToInt32(id);
            return CreatedAtAction("PostStudent", new { StudentId = Convert.ToInt32(id) }, student);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            SqlParameter param = new SqlParameter();
            param.ParameterName = "StudentId";
            param.Value = id;
            if (DBHelper.ExecuteNonQuery(connectionString, "DeleteStudentDetails", param) > 0)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
