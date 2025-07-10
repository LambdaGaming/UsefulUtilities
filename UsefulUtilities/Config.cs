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

		[Description( "Whether or not announcing Chaos Insurgency waves is disabled." )]
		public bool DisableChaosAnnouncement { get; set; } = true;

		[Description( "Whether or not the Alpha Warhead deadman's switch is disabled." )]
		public bool DisableDeadmansSwitch { get; set; } = true;

		[Description( "Whether or not the downgrading of Scientists and Class Ds in SCP-914 is allowed." )]
		public bool Allow914PlayerDowngrades { get; set; } = true;

		[Description( "Chance of players being teleported to a random location when using SCP-914 on very fine. Set to 0 to disable." )]
		public double Scp914TeleportChance { get; set; } = 5;
	}
}
