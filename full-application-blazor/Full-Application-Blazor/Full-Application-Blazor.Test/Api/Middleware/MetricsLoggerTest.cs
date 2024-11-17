using System;
using System.Collections;
using System.Dynamic;
using Full_Application_Blazor.API.Middleware;
using Full_Application_Blazor.Test.MockData.Classes;
using Microsoft.AspNetCore.Http;
using Prometheus.Client;

namespace Full_Application_Blazor.Test.Api.Middleware
{
    public class MetricsLoggerTest : IDisposable
    {
        private MockPrometheusRegistry _registry;
        private MetricFactory _factory;
        private MetricsLogger _metricsLogger;
        private DefaultHttpContext _defaultContext;

        public MetricsLoggerTest()
        {
            _registry = new MockPrometheusRegistry();
            _factory = new MetricFactory(_registry);

            _metricsLogger = new MetricsLogger(next: (innerContext) =>
            {
                Thread.Sleep(10);
                return Task.CompletedTask;
            }, _factory);

            _defaultContext = new DefaultHttpContext();
            _defaultContext.Response.Body = new MemoryStream();
            _defaultContext.Request.Body = new MemoryStream();
            _defaultContext.Request.Path = "/";
        }

        public void Dispose()
        {
            _registry = null;
            _factory = null;
            _metricsLogger = null;
        }

        [Theory, MemberData(nameof(ListData))]
        public void TestMetricLoggerRequestCountIncrease(string statusKey, int statusCode, string controller = null, string action = null)
        {
            _defaultContext.Response.StatusCode = statusCode;
            _defaultContext.Request.RouteValues["controller"] = controller;
            _defaultContext.Request.RouteValues["action"] = action;
            _metricsLogger.Invoke(_defaultContext).GetAwaiter().GetResult();

            var statusFamily = (MetricFamily<ICounter, Counter, (string, string, string), MetricConfiguration>)(_registry.Collectors[statusKey]);
            var requestCountFamily = (MetricFamily<ICounter, Counter, (string, string, string), MetricConfiguration>)(_registry.Collectors[_totalRequestCount]);
            var responseDurationFamily = (MetricFamily<IGauge, Gauge, (string, string, string), MetricConfiguration>)(_registry.Collectors[_responseDuration]);

            Assert.True(statusFamily != null);
            Assert.True(statusFamily.Labelled.FirstOrDefault().Value.Value == 1);
            Assert.True(requestCountFamily != null);
            Assert.True(requestCountFamily.Labelled.FirstOrDefault().Value.Value == 1);
            Assert.True(responseDurationFamily != null);
            Assert.True(responseDurationFamily.Labelled.FirstOrDefault().Value.Value > 0);
            Assert.Equal(statusCode, _defaultContext.Response.StatusCode);
        }

        public static IEnumerable<object[]> ListData =
        new List<object[]>
        {
            new object[] { "total_request_status_OK", 200, "Home", "Get" },
            new object[] { "total_request_status_Created", 201 },
            new object[] { "total_request_status_No_Data", 204 },
            new object[] { "total_request_status_Bad_Request", 400 },
            new object[] { "total_request_status_Unauthorized", 401 },
            new object[] { "total_request_status_Forbidden", 403 },
            new object[] { "total_request_status_Not_Found", 404 },
            new object[] { "total_request_status_Method_Not_Allowed", 405 },
            new object[] { "total_request_status_Unsupported_Media_Type", 415 },
            new object[] { "total_request_status_Internal_Server_Error", 500 },

        };

        internal static string _responseDuration = "current_response_duration";
        internal static string _totalRequestCount = "total_request_count";
    }
}

