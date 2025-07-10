using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Scp914;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace UsefulUtilities
{
	public class EventHandlers
	{
		private Plugin plugin;
		private Random rand = new Random();
		public EventHandlers( Plugin plugin ) => this.plugin = plugin;

		private IEnumerator<float> SetScp()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					int r = rand.Next( 5 );
					if ( r == 1 )
					{
						RoleTypeId[] roles = { RoleTypeId.Scp096, RoleTypeId.Scp3114 };
						RoleTypeId randRole = roles[rand.Next( roles.Length - 1 )];
						if ( randRole == RoleTypeId.Scp096 && ply.GetScpPreference( RoleTypeId.Scp096 ) < 0 )
						{
							// Respect player's 096 preference
							continue;
						}
						ply.Role.Set( randRole, SpawnReason.ForceClass );
					}
				}
			}
		}

		public void OnRoundStart()
		{
			if ( plugin.Config.ExtendedSpawnPool )
			{
				Timing.RunCoroutine( SetScp() );
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

		public void OnUpgradingPlayer( UpgradingPlayerEventArgs ev )
		{
			if ( plugin.Config.Allow914PlayerDowngrades )
			{
				if ( ev.KnobSetting == Scp914.Scp914KnobSetting.Coarse && ev.Player.Role.Type == RoleTypeId.Scientist )
				{
					ev.Player.Role.Set( RoleTypeId.ClassD, SpawnReason.Respawn, RoleSpawnFlags.None );
					ev.Player.Position = ev.OutputPosition;
				}
				else if ( ev.KnobSetting == Scp914.Scp914KnobSetting.Rough && ev.Player.Role.Type == RoleTypeId.ClassD )
				{
					ev.Player.Kill( DamageType.Crushed );
				}
			}
		}
	}
}
