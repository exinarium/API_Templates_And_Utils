using Prometheus.Client;

namespace Full_Application_Blazor.API.Middleware
{
    public class MetricsLogger
    {
        private readonly RequestDelegate _next;
        private readonly IMetricFactory _metricFactory;
        private IMetricFamily<IGauge> _responseDuration;
        private IMetricFamily<ICounter> _totalRequestCount;

        private IMetricFamily<ICounter> _totalRequestStatusOK;
        private IMetricFamily<ICounter> _totalRequestStatusCreated;
        private IMetricFamily<ICounter> _totalRequestStatusNoData;
        private IMetricFamily<ICounter> _totalRequestStatusBadRequest;
        private IMetricFamily<ICounter> _totalRequestStatusUnauthorized;
        private IMetricFamily<ICounter> _totalRequestStatusForbidden;
        private IMetricFamily<ICounter> _totalRequestStatusNotFound;
        private IMetricFamily<ICounter> _totalRequestStatusMethodNotAllowed;
        private IMetricFamily<ICounter> _totalRequestStatusUnsupportedMediaType;
        private IMetricFamily<ICounter> _totalRequestStatusInternalServerError;

        public MetricsLogger(RequestDelegate next, IMetricFactory metricFactory)
        {
            this._next = next;
            this._metricFactory = metricFactory;

            SetupMetrics();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var controller = httpContext.Request.RouteValues["controller"]?.ToString() ?? string.Empty;
            var action = httpContext.Request.RouteValues["action"]?.ToString() ?? string.Empty;

            var path = httpContext.Request.Path;

            IncreaseRequestCount(controller, action, path);

            var requestDateTime = DateTime.UtcNow;

            await _next.Invoke(httpContext);

            var responseDateTime = DateTime.UtcNow;
            var responseDuration = responseDateTime - requestDateTime;
            var responseStatusCode = httpContext.Response.StatusCode;

            WriteCurrentResponseDuration(responseDuration.TotalMilliseconds, controller, action, path);
            WriteResponseStatus(responseStatusCode, controller, action, path);
        }

        private void SetupMetrics()
        {
            _responseDuration = _metricFactory.CreateGauge("current_response_duration", "The current response duration", false, "controller", "action", "path");
            _totalRequestCount = _metricFactory.CreateCounter("total_request_count", "The current request count", false, "controller", "action", "path");
            _totalRequestStatusOK = _metricFactory.CreateCounter("total_request_status_OK", "The total request status of type 200", false, "controller", "action", "path");
            _totalRequestStatusCreated = _metricFactory.CreateCounter("total_request_status_Created", "The total request status of type 201", false, "controller", "action", "path");
            _totalRequestStatusNoData = _metricFactory.CreateCounter("total_request_status_No_Data", "The total request status of type 204", false, "controller", "action", "path");
            _totalRequestStatusBadRequest = _metricFactory.CreateCounter("total_request_status_Bad_Request", "The total request status of type 400", false, "controller", "action", "path");
            _totalRequestStatusUnauthorized = _metricFactory.CreateCounter("total_request_status_Unauthorized", "The total request status of type 401", false, "controller", "action", "path");
            _totalRequestStatusForbidden = _metricFactory.CreateCounter("total_request_status_Forbidden", "The total request status of type 403", false, "controller", "action", "path");
            _totalRequestStatusNotFound = _metricFactory.CreateCounter("total_request_status_Not_Found", "The total request status of type 404", false, "controller", "action", "path");
            _totalRequestStatusMethodNotAllowed = _metricFactory.CreateCounter("total_request_status_Method_Not_Allowed", "The total request status of type 405", false, "controller", "action", "path");
            _totalRequestStatusUnsupportedMediaType = _metricFactory.CreateCounter("total_request_status_Unsupported_Media_Type", "The total request status of type 415", false, "controller", "action", "path");
            _totalRequestStatusInternalServerError = _metricFactory.CreateCounter("total_request_status_Internal_Server_Error", "The total request status of type 500", false, "controller", "action", "path");
        }

        private void WriteCurrentResponseDuration(double duration, string controller = "", string action = "", string path = "")
        {
            _responseDuration.WithLabels(controller, action, path).Set(duration);
        }

        private void IncreaseRequestCount(string controller = "", string action = "", string path = "")
        {
            _totalRequestCount.WithLabels(controller, action, path).Inc(1);
        }

        private void WriteResponseStatus(int statusCode, string controller = "", string action = "", string path = "")
        {
            switch(statusCode)
            {
                case 200:
                    {
                        _totalRequestStatusOK.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
                case 201:
                    {
                        _totalRequestStatusCreated.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
                case 204:
                    {
                        _totalRequestStatusNoData.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
                case 400:
                    {
                        _totalRequestStatusBadRequest.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
                case 401:
                    {
                        _totalRequestStatusUnauthorized.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
                case 403:
                    {
                        _totalRequestStatusForbidden.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
                case 404:
                    {
                        _totalRequestStatusNotFound.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
                case 405:
                    {
                        _totalRequestStatusMethodNotAllowed.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
                case 415:
                    {
                        _totalRequestStatusUnsupportedMediaType.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
                case 500:
                    {
                        _totalRequestStatusInternalServerError.WithLabels(controller, action, path).Inc(1);
                        break;
                    }
            }
        }
    }
}

