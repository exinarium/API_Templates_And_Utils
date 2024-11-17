using Full_Application_Blazor.Test.MockData.Repository;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Full_Application_Blazor.Utils.Repositories;

namespace Full_Application_Blazor.Test.Utils.Services
{
    public class PlanServiceTest : IDisposable
    {
        private IRepository<Plan> _repository;
        private IRepository<Plan> _repository1;
        private readonly Plan _Plan;
        private readonly Plan _Plan1;
        private readonly IPlanService _planService;
        private readonly IPlanService _planService1;

        public PlanServiceTest()
        {
            _repository = new MockRepository<Plan>();
            _repository1 = new MockRepository<Plan>();

            _Plan = new Plan
            {
                Id = "1",
                Name= "1234567890",
                Description = "Test",
                PlanType = PlanType.ENTERPRISE
            };

            _Plan1 = new Plan
            {
                Id = "1",
                Name = "Test1234567890",
                Description = null,
                PlanType = PlanType.FREE
            };

            _repository = new MockRepository<Plan>
            {
                Value = _Plan,
            };

            _repository1 = new MockRepository<Plan>
            {
                Value = _Plan1,
            };

            _planService = new PlanService(_repository);
            _planService1 = new PlanService(_repository1);
        }
        public void Dispose()
        {
            _repository = null;
            _repository1 = null;
        }

        [Fact]
        public async Task SavePlanAsync()
        {
            await _planService.CreateAsync(_Plan);
            var test = await _planService.GetAsync(_Plan.Id);
            Assert.True(test.Name == "1234567890");
        }

        [Fact]
        public async Task SavePlanAsyncSomeNull()
        {
            await _planService1.CreateAsync(_Plan1);
            var test = await _planService1.GetAsync(_Plan1.Id);
            Assert.True(test.Name == "Test1234567890");
        }

        [Fact]
        public async void UpdateDetails()
        {

            _Plan1.Description = "0000007634";

            _repository = new MockRepository<Plan>
            {
                Value = _Plan1,
            };

            await _planService1.UpdateAsync(_Plan1);
            var test = await _planService1.GetAsync(_Plan1.Id);
            Assert.True(test.Description == "0000007634");
        }

        [Fact]
        public async void ListPlanDetails()
        {
            var list = await _planService1.GetAllAsync();
            Assert.True(list.Any());
        }

        [Fact]
        public async void GetPlanDetails()
        {
            var value = await _planService.GetAsync(_Plan.Id);
            Assert.True(value.Description != null);
        }

        [Fact]
        public async void DeletePlanDetails()
        {
            var val = await _planService1.DeleteAsync(_Plan.Id);
            Assert.Equal(State.DELETED, val.IsDeleted);
        }
    }
}
