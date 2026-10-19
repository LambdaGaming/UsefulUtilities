using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Scp914;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using MapGeneration.Holidays;
using MEC;
using PlayerRoles;

namespace UsefulUtilities;

public class EventHandlers
{
	private Plugin plugin;
	private Random rand = new();
	public EventHandlers( Plugin plugin ) => this.plugin = plugin;

	private IEnumerator<float> SetScp()
	{
		yield return Timing.WaitForSeconds( 3f );
		List<RoleTypeId> chosenRoles = [];
		RoleTypeId[] roles = [];

		// Only add SCPs to extra spawn pool if they're not already in the main one
		if ( !HolidayUtils.IsHolidayActive( HolidayType.Halloween ) )
			roles.Append( RoleTypeId.Scp3114 );
		if ( Player.List.Count < 8 )
			roles.Append( RoleTypeId.Scp096 );
		if ( roles.Length == 0 )
			yield break;

		foreach ( Player ply in Player.List )
		{
			int r = rand.Next( 5 );
			if ( ply.IsScp && r == 1 )
			{
				RoleTypeId randRole = roles[rand.Next( roles.Length )];

				// Don't assign multiple players to the same SCP
				if ( chosenRoles.Contains( randRole ) )
					continue;

				// Respect player's 096 preference
				if ( randRole == RoleTypeId.Scp096 && ply.GetScpPreference( RoleTypeId.Scp096 ) < 0 )
					continue;

				ply.Role.Set( randRole, SpawnReason.ForceClass );
				chosenRoles.Add( randRole );
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

	private bool resetFriendlyFire;
	public void OnWaitingForPlayers()
	{
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
