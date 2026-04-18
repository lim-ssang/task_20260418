using Lim_BE_Assignment.Dtos;
using Lim_BE_Assignment.Dtos.Request;
using Lim_BE_Assignment.Dtos.Response;
using Lim_BE_Assignment.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Linq;

namespace Lim_BE_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService employeeService;

        private readonly ILogger<EmployeeController> logger;

        public EmployeeController(EmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            this.employeeService = employeeService;

            this.logger = logger;
        }

        /// <summary>
        /// 직원정보 조회
        /// </summary>
        /// <param name="page">페이지 번호</param>
        /// <param name="pageSize">페이지 사이즈</param>
        /// <returns>직원 리스트</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PageResponse<EmployeeResponse>), StatusCodes.Status200OK, "application/json")]
        public IActionResult Get(int page = 1, int pageSize = 30)
        {
            page = Math.Max(1, page);
            pageSize = Math.Max(1, pageSize);
            var respObj = employeeService.GetEmployeeList(page, pageSize);
            
            return Ok(respObj);
        }

        /// <summary>
        /// 직원 조회
        /// </summary>
        /// <param name="name">직원 성명</param>
        /// <returns>해당 직원 데이터</returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK, "application/json")]
        public IActionResult GetByName(string name)
        {
            var respObj = employeeService.GetEmployeeByName(name);

            if (respObj == null) return NotFound("해당하는 이름이 없습니다.");

            return Ok(respObj);
        }

        /// <summary>
        /// 직원데이터 입력
        /// </summary>
        /// <remarks>
        /// * contentType : application/json
        /// * contentType : text/csv
        /// * contentType : multipart/form-data[.json, .csv]
        /// </remarks>
        /// <returns></returns>
        [Consumes("application/json","text/csv","multipart/form-data")]
        [ProducesResponseType(typeof(List<AddEmployeeRequest>), StatusCodes.Status201Created, "application/json")]
        [HttpPost]
        public async Task<IActionResult> EmployeePost()
        {
            var contentType = Request.ContentType;
            var body = Request.Body;
            var token = HttpContext.RequestAborted;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var reqJsonBody = await JsonSerializer.DeserializeAsync<List<AddEmployeeRequest>>(body, options, token);
                    if (reqJsonBody is null)
                    {
                        return BadRequest("잘못된 Json 포맷입니다.");
                    }

                    employeeService.InsertEmployee(reqJsonBody);
                    return Created("", reqJsonBody);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return NotFound(ex.Message);
                }
            }
            else if (contentType.StartsWith("text/csv", StringComparison.OrdinalIgnoreCase))
            {
                var reqResult = await employeeService.InsertEmployeeCSVAsync(body);
                return Created("", reqResult);
            }
            else if (contentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            {
                List<AddEmployeeRequest> resultList = new();

                var files = Request.Form.Files;

                foreach(var file in files)
                {
                    var ext = Path.GetExtension(file.FileName);

                    switch (ext)
                    {
                        case ".json":
                            try
                            {
                                var reqJsonBody = await JsonSerializer.DeserializeAsync<List<AddEmployeeRequest>>(file.OpenReadStream(), options, token);
                                if (reqJsonBody is not null)
                                {
                                    employeeService.InsertEmployee(reqJsonBody);
                                    resultList.AddRange(reqJsonBody);
                                }
                            }
                            catch { }
                            break;
                        case ".csv":
                            try
                            {
                                var reqCSVResult = await employeeService.InsertEmployeeCSVAsync(file.OpenReadStream());
                                resultList.AddRange(reqCSVResult);
                            }
                            catch { }
                            break;
                    }
                }

                return Created("", resultList);
            }

            return BadRequest("지원하지 않는 ContentType입니다.");
        }
    }
}
