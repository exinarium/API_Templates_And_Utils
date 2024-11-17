using Xunit.Abstractions;
using Xunit.Sdk;

namespace Full_Application_Blazor.Test.Helpers.Services
{
    public class DisplayNameOrderer : ITestCollectionOrderer
    {
        public IEnumerable<ITestCollection> OrderTestCollections(
            IEnumerable<ITestCollection> testCollections) =>
            testCollections.OrderBy(collection => collection.DisplayName);
    }
}
