using System.ComponentModel;
using Exiled.API.Interfaces;

namespace UsefulUtilities
{
	public sealed class Config : IConfig
	{
		[Description( "Indicates whether the plugin is enabled or not" )]
		public bool IsEnabled { get; set; } = true;

		[Description( "Whether or not debug messages should be shown in the console." )]
		public bool Debug { get; set; } = false;

		[Description( "Whether or not 096 and 3114 should be added to the regular spawn pool." )]
		public bool ExtendedSpawnPool { get; set; } = true;
	}
}
