using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Scp914;
using Exiled.Events.EventArgs.Server;
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
			List<RoleTypeId> chosenRoles = new List<RoleTypeId>();
			foreach ( Player ply in Player.List )
			{
				int r = rand.Next( 5 );
				if ( ply.IsScp && r == 1 )
				{
					RoleTypeId[] roles = { RoleTypeId.Scp096, RoleTypeId.Scp3114 };
					RoleTypeId randRole = roles[rand.Next( roles.Length )];
					if ( chosenRoles.Contains( randRole ) )
					{
						// Don't assign multiple players to the same SCP
						continue;
					}
					if ( randRole == RoleTypeId.Scp096 && ply.GetScpPreference( RoleTypeId.Scp096 ) < 0 )
					{
						// Respect player's 096 preference
						continue;
					}
					ply.Role.Set( randRole, SpawnReason.ForceClass );
					chosenRoles.Add( randRole );
				}
			}
		}

		private bool resetFriendlyFire;
		public void OnRoundStart()
		{
			if ( plugin.Config.ExtendedSpawnPool )
			{
				Timing.RunCoroutine( SetScp() );
			}
			if ( resetFriendlyFire )
			{
				Server.FriendlyFire = false;
				resetFriendlyFire = false;
			}
		}

		public void OnRoundEnd( RoundEndedEventArgs ev )
		{
			if ( plugin.Config.EndRoundFriendlyFire && !Server.FriendlyFire )
			{
				Server.FriendlyFire = true;
				resetFriendlyFire = true;
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
			if ( plugin.Config.Scp914TeleportChance > 0 && ev.KnobSetting == Scp914.Scp914KnobSetting.VeryFine )
			{
				double chance = rand.NextDouble() * 100;
				if ( chance <= plugin.Config.Scp914TeleportChance )
				{
					RoleTypeId[] roles = {
						RoleTypeId.ClassD, RoleTypeId.Scientist, RoleTypeId.ChaosConscript,
						RoleTypeId.NtfCaptain, RoleTypeId.FacilityGuard, RoleTypeId.Scp049,
						RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp173,
						RoleTypeId.Scp3114, RoleTypeId.Scp939
					};
					RoleTypeId randRole = roles.RandomItem();
					ev.OutputPosition = randRole.GetRandomSpawnLocation().Position;
				}
			}
		}
	}
}
