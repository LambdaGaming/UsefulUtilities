using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using events = Exiled.Events.Handlers;

namespace UsefulUtilities
{
	public class Plugin : Plugin<Config>
	{
		private EventHandlers EventHandlers;
		public override Version Version { get; } = new Version( 1, 6, 1 );
		public override Version RequiredExiledVersion { get; } = new Version( 9, 9, 0 );
		public override string Author { get; } = "OPGman";
		public override PluginPriority Priority { get; } = PluginPriority.Medium;

		public override void OnEnabled()
		{
			base.OnEnabled();
			EventHandlers = new EventHandlers( this );
			events.Server.RoundStarted += EventHandlers.OnRoundStart;
			events.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
			events.Server.RoundEnded += EventHandlers.OnRoundEnd;
			events.Map.AnnouncingChaosEntrance += EventHandlers.OnAnnounceChaos;
			events.Warhead.DeadmanSwitchInitiating += EventHandlers.OnDeadmanSwitch;
			events.Scp914.UpgradingPlayer += EventHandlers.OnUpgradingPlayer;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			events.Server.RoundStarted -= EventHandlers.OnRoundStart;
			events.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
			events.Server.RoundEnded -= EventHandlers.OnRoundEnd;
			events.Map.AnnouncingChaosEntrance -= EventHandlers.OnAnnounceChaos;
			events.Warhead.DeadmanSwitchInitiating -= EventHandlers.OnDeadmanSwitch;
			events.Scp914.UpgradingPlayer -= EventHandlers.OnUpgradingPlayer;
			EventHandlers = null;
		}
	}
}
