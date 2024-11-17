using System;
using Full_Application_Blazor.Utils.Configuration;

namespace Full_Application_Blazor.Utils.Helpers.Classes
{
	public interface ISeed
	{
		void SeedAll(DatabaseConfig databaseConfig, SeedConfig seedConfig);
    }
}

