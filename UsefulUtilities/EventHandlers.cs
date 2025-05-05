using Exiled.API.Enums;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Warhead;
using PlayerRoles;
using System;

namespace UsefulUtilities
{
	public class EventHandlers
	{
		private Plugin plugin;
		private Random rand = new Random();
		public EventHandlers( Plugin plugin ) => this.plugin = plugin;

		public void OnPlayerSpawned( SpawnedEventArgs ev )
		{
			if ( plugin.Config.ExtendedSpawnPool && ev.Player.IsScp && ev.OldRole.Team != Team.SCPs && ev.Player.Role.Type != RoleTypeId.Scp0492 )
			{
				int r = rand.Next( 100 );
				if ( r < 10 )
				{
					RoleTypeId[] roles = { RoleTypeId.Scp096 };
					RoleTypeId randRole = roles[rand.Next( roles.Length - 1 )];
					if ( randRole == RoleTypeId.Scp096 && ev.Player.GetScpPreference( RoleTypeId.Scp096 ) <= 0 )
					{
						// Respect player's 096 preference
						return;
					}
					ev.Player.Role.Set( randRole, SpawnReason.ForceClass );
				}
			}
		}

		public void OnAnnounceChaos( AnnouncingChaosEntranceEventArgs ev )
		{
			ev.IsAllowed = !plugin.Config.DisableChaosAnnouncement;
		}

		public void OnDeadmanSwitch( DeadmanSwitchInitiatingEventArgs ev )
		{
			ev.IsAllowed = !plugin.Config.DisableDeadmansSwitch;
		}
	}
}
