using System;
using System.ComponentModel;

namespace Full_Application_Blazor.Utils.Helpers.Enums
{
    public enum Operator
    {
		[Description(">")]
		GT = 1,

		[Description(">=")]
		GTE = 2,

		[Description("<")]
		LT = 3,

		[Description("<=")]
		LTE = 4,

		[Description("=")]
		EQ = 5,

		[Description("<>")]
		NEQ = 6,

		[Description("IN")]
		IN = 7,

		[Description("NOT IN")]
		NOT_IN = 8,

		[Description("IS NULL")]
		NULL = 9,

		[Description("IS NOT NULL")]
		NOT_NULL = 10,

		[Description("CONTAINS")]
		CONTAINS = 11,

		[Description("NO OPERATOR")]
		NO_OP = 12,
	}
}

