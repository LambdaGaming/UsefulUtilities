# Useful Utilities
 EXILED plugin for SCP:SL that contains various gameplay changes and configurations, mainly intended for servers with small playerbases.

# Features
Each feature is optional and can be toggled in the config.
- Add SCP-096 and SCP-3114 to the regular spawn pool, no matter how many players are on the server
  - Players who have their SCP-096 preference in the red by any amount will not be forced to switch
- Disable the announcement when a Chaos Insurgency wave spawns
- Disable the Alpha Warhead deadman's switch
- Allow Scientists and Class Ds to downgrade themselves in SCP-914
  - Scientists will downgrade to Class D when knob is set to course
  - Class Ds will be killed when knob is set to rough
- Chance for players to be teleported to a random location when using SCP-914 on the very fine setting
  - 5% by default
- Enable friendly fire after the round ends, if it isn't already enabled by default

# Building
 The project files are intended to be built using either the command line or VSCode with the C# Dev Tools extension, but Visual Studio should work too. You will need to have the SCP:SL dedicated server and .NET SDK 8 or above installed. You will also need to change the reference paths in the .csproj file to the location of your dedicated server.

# Contributing
 Contributions are welcome! Please read through the [guidelines](https://lambdagaming.github.io/guides/contributing) before submitting an issue or pull request.
