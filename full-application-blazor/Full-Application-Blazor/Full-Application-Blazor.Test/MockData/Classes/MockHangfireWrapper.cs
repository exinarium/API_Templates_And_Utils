using System;
using System.Linq.Expressions;
using Full_Application_Blazor.Utils.Helpers.Interfaces;

namespace Full_Application_Blazor.Test.MockData.Classes
{
	public class MockHangfireWrapper : IHangfireWrapper
	{
		public MockHangfireWrapper()
		{
		}

        public void BackgroundJobEnqueue<T>(Expression<Action<T>> function)
        {
            Console.WriteLine($"Hangfire Job Queued");
        }
    }
}

