using Prometheus.Client.Collectors;
using Prometheus.Client.MetricsWriter;

namespace Full_Application_Blazor.Test.MockData.Classes
{
    public class MockPrometheusRegistry : ICollectorRegistry
    {
        public Dictionary<string, dynamic> Collectors { get; set; }

        public MockPrometheusRegistry()
        {
            Collectors = new Dictionary<string, dynamic>();
        }

        public void Add(ICollector collector)
        {
            Collectors.Add(collector.Configuration.Name, collector);
        }

        public async Task CollectToAsync(IMetricsWriter writer, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ICollector Remove(string name)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ICollector collector)
        {
            throw new NotImplementedException();
        }

        public bool TryGet(string name, out ICollector collector)
        {
            Collectors.TryGetValue(name, out dynamic? collect);

            if (collect != null)
            {
                collector = (ICollector)collect;

                if (collector != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            collector = null;
            return false;
        }

        TCollector ICollectorRegistry.GetOrAdd<TCollector, TConfig>(TConfig config, Func<TConfig, TCollector> collectorFactory)
        {
            var get = TryGet(config.Name, out ICollector collector);
            if (get == true)
            {
                return (TCollector)collector;
            }
            else
            {
                var c = collectorFactory(config);
                Add(c);

                return c;
            }
        }
    }
}