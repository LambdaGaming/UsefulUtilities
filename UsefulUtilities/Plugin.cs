using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using events = Exiled.Events.Handlers;

namespace UsefulUtilities
{
	public class Plugin : Plugin<Config>
	{
		private EventHandlers EventHandlers;
		public override Version Version { get; } = new Version( 1, 0, 1 );
		public override Version RequiredExiledVersion { get; } = new Version( 8, 8, 0 );
		public override string Author { get; } = "OPGman";
		public override PluginPriority Priority { get; } = PluginPriority.Medium;

		public override void OnEnabled()
		{
			base.OnEnabled();
			EventHandlers = new EventHandlers( this );
			events.Player.Spawned += EventHandlers.OnPlayerSpawned;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			events.Player.Spawned -= EventHandlers.OnPlayerSpawned;
			EventHandlers = null;
		}
	}
}
