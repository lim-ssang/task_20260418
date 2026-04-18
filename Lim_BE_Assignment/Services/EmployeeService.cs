using CsvHelper;
using CsvHelper.Configuration;
using Lim_BE_Assignment.Dtos;
using Lim_BE_Assignment.Dtos.Request;
using Lim_BE_Assignment.Dtos.Response;
using Lim_BE_Assignment.Entites;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Text.Json;

namespace Lim_BE_Assignment.Services
{
    public class EmployeeService
    {
        private readonly ILogger<EmployeeService> logger;

        private readonly IMemoryCache memoryCache;

        private const string EMPLOYEE_CACHE_KEY = "CACHE:employee_list";
        public EmployeeService(IMemoryCache memoryCache, ILogger<EmployeeService> logger)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
        }

        public PageResponse<EmployeeResponse> GetEmployeeList(int page = 1, int pageSize = 30)
        {
            var totalCount = Employees is null ? 0 : Employees.Count;
            page = Math.Max(1, page);
            pageSize = Math.Max(1, pageSize);

            if (totalCount < 1)
            {
                return new PageResponse<EmployeeResponse>();
            }

            var respList = Employees?.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new EmployeeResponse(x));

            return new PageResponse<EmployeeResponse>(page, pageSize, totalCount, respList);
        }

        public EmployeeResponse? GetEmployeeByName(string name)
        {
            return Employees?.Select(x => new EmployeeResponse(x)).FirstOrDefault(x => x.Name == name); 
        }

        public void InsertEmployee(IEnumerable<AddEmployeeRequest> req)
        {
            AddEmployee(req.Select(x => x.ToEntity()));
        }

        public async Task<IEnumerable<AddEmployeeRequest>> InsertEmployeeCSVAsync(Stream body, CancellationToken ck = default)
        {
            using var reader = new StreamReader(body);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,     // 첫 줄 헤더 여부
                MissingFieldFound = null,   // 없는 필드 무시
                HeaderValidated = null,     // 헤더 검증 무시
            });

           var list = new List<AddEmployeeRequest>();

            await foreach (var record in csv.GetRecordsAsync<AddEmployeeRequest>(ck))
            {
                list.Add(record);
            }

            InsertEmployee(list);

            return list;
        }

        private List<Employee>? Employees => memoryCache.Get<List<Employee>>(EMPLOYEE_CACHE_KEY);

        private void AddEmployee(IEnumerable<Employee> employee)
        {
            var list = Employees;

            if (list == null) list = new List<Employee>();

            list.AddRange(employee);
            
            memoryCache.Set<List<Employee>>(EMPLOYEE_CACHE_KEY, list);

            logger.LogInformation($"Add Employee - {JsonSerializer.Serialize(employee)}");
        }

        private void AddEmployee(Employee employee) {
            AddEmployee(new[] { employee });
        }
    }
}
