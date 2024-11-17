using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Full_Application_Blazor.Utils.Helpers.Interfaces;
using Hangfire;

namespace Full_Application_Blazor.Utils.Helpers.Classes
{
    [ExcludeFromCodeCoverage]
    public class HangfireWrapper : IHangfireWrapper
    {
        public void BackgroundJobEnqueue<T>(Expression<Action<T>> function)
        {
            BackgroundJob.Enqueue<T>(function);
        }
    }
}

