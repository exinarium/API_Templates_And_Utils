using System;
using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
	public enum State
	{
		[Description("The item is not deleted")]
        NOT_DELETED = 0,

        [Description("The item is deleted")]
        DELETED = 1,
	}
}

