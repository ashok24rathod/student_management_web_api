using System.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace StudentManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllHeaders")]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString = string.Empty;

        public DepartmentController(IConfiguration config)
        {
            configuration = config;
            connectionString = config.GetConnectionString("DefaultConnection");
        }

        // GET: api/Department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {
            DataTable dataTable = DBHelper.GetTableFromSP(connectionString, "GetAllDepartments", null);
            List<Department> departments = (List<Department>)dataTable.ToList<Department>();

            return departments; ;
        }
    }
}
