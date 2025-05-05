using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using events = Exiled.Events.Handlers;

namespace UsefulUtilities
{
	public class Plugin : Plugin<Config>
	{
		private EventHandlers EventHandlers;
		public override Version Version { get; } = new Version( 1, 4, 0 );
		public override Version RequiredExiledVersion { get; } = new Version( 9, 5, 0 );
		public override string Author { get; } = "OPGman";
		public override PluginPriority Priority { get; } = PluginPriority.Medium;

		public override void OnEnabled()
		{
			base.OnEnabled();
			EventHandlers = new EventHandlers( this );
			events.Player.Spawned += EventHandlers.OnPlayerSpawned;
			events.Map.AnnouncingChaosEntrance += EventHandlers.OnAnnounceChaos;
			events.Warhead.DeadmanSwitchInitiating += EventHandlers.OnDeadmanSwitch;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			events.Player.Spawned -= EventHandlers.OnPlayerSpawned;
			events.Map.AnnouncingChaosEntrance -= EventHandlers.OnAnnounceChaos;
			events.Warhead.DeadmanSwitchInitiating -= EventHandlers.OnDeadmanSwitch;
			EventHandlers = null;
		}
	}
}
