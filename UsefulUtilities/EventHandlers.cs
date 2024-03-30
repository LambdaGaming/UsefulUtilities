using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
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
					ev.Player.Role.Set( RoleTypeId.Scp3114, SpawnReason.ForceClass );
				else if ( r < 20 && ev.Player.GetScpPreference( RoleTypeId.Scp096 ) >= 0 )
                    ev.Player.Role.Set( RoleTypeId.Scp096, SpawnReason.ForceClass );
            }
		}
	}
}
