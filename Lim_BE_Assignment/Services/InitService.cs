using Lim_BE_Assignment.Dtos.Request;
using Lim_BE_Assignment.Entites;

namespace Lim_BE_Assignment.Services
{
    /// <summary>
    /// 서비스 기동시 초기화
    /// </summary>
    public class InitService : IHostedService
    {
        private readonly ILogger<InitService> logger;

        private readonly IServiceScopeFactory scopeFactory;

        public InitService(IServiceScopeFactory scopeFactory, ILogger<InitService> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        /// <summary>
        /// 서버 기동시 초기화.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Service Start.");
            logger.LogWarning("Service Start.");
            logger.LogError("Service Start.");

            using var scope = scopeFactory.CreateScope();
            var employserivce = scope.ServiceProvider.GetRequiredService<EmployeeService>();

            var initDataList = new List<AddEmployeeRequest>();

            initDataList.Add(new AddEmployeeRequest { Name = "김사범", Email="aaa@email.com", Tel="010-2224-1231", Joined = "2018.02.14"});
            initDataList.Add(new AddEmployeeRequest { Name = "김오범", Email = "bbb@email.com", Tel = "010-1123-1231", Joined = "2020.04.14" });
            initDataList.Add(new AddEmployeeRequest { Name = "김육범", Email = "ccc@email.com", Tel = "010-6654-7877", Joined = "2013.12.10" });

            employserivce.InsertEmployee(initDataList);
        }

        /// <summary>
        /// 서버 종료시 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Server Stop...");
            logger.LogWarning("Server Stop...");
            logger.LogError("Server Stop...");

            return Task.CompletedTask;
        }
    }
}
