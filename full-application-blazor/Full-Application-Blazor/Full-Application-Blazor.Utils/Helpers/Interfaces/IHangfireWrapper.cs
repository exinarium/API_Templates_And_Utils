using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Full_Application_Blazor.Utils.Helpers.Interfaces
{
	public interface IHangfireWrapper
	{
        void BackgroundJobEnqueue<T>(Expression<Action<T>> function);
    }
}

