using Full_Application_Blazor.Domain.Mappers;
using Full_Application_Blazor.Utils.Helpers.Classes;
using GraphQL;

namespace Full_Application_Blazor.Test.Domain.Mappers
{
    public class FilterMapperTest
    {
        private readonly IFilterMapper _filterMapper;
        private readonly List<Filter<string>> _filterList1;
        private readonly List<Filter<string>> _filterList2;
        private readonly List<Filter<string>> _filterList3;
        private readonly List<Filter<string>> _filterList4;

        public FilterMapperTest() 
        {
            _filterMapper = new FilterMapper();

            _filterList1 = new List<Filter<string>> { new Filter<string> 
            {
                Property = "aaa", 
                Operator = Full_Application_Blazor.Utils.Helpers.Enums.Operator.EQ, 
                Value = "aaa"
            }};

            _filterList2 = new List<Filter<string>> { new Filter<string> 
            {
                Property = "true",
                Operator = Full_Application_Blazor.Utils.Helpers.Enums.Operator.EQ,
                Value = "true"
            }};
            
            _filterList3 = new List<Filter<string>> { new Filter<string> 
            {
                Property = "4",
                Operator = Full_Application_Blazor.Utils.Helpers.Enums.Operator.EQ,
                Value = "4"
            }};
            
            _filterList4 = new List<Filter<string>> { new Filter<string> 
            {
                Property = "7,77",
                Operator = Full_Application_Blazor.Utils.Helpers.Enums.Operator.EQ,
                Value = "7,77"
            }};
        }

        [Fact]
        public async Task FilterMapperTest1()
        {
            var map = _filterMapper.MapFilters(_filterList1);
            Assert.True(map[0].TypeCode == TypeCode.String);
        }

        [Fact]
        public async Task FilterMapperTest2()
        {
            var map = _filterMapper.MapFilters(_filterList2);
            Assert.True(map[0].TypeCode == TypeCode.Boolean);
        }

        [Fact]
        public async Task FilterMapperTest3()
        {
            var map = _filterMapper.MapFilters(_filterList3);
            Assert.True(map[0].TypeCode == TypeCode.Int32);
        }

        [Fact]
        public async Task FilterMapperTest4()
        {
            var map = _filterMapper.MapFilters(_filterList4);
            Assert.True(map[0].TypeCode == TypeCode.Double);
        }
    }
}
