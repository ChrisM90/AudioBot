# AudioBot
This is a really simple bot that records audio from an output device on the local machine and fowards it to a discord channel, The only way to interact with the bot is via the GUI no discord chat commands ae supported.

This will probaly also work with virtual output devices, the idea being you setup your media played to output to an unused output device and then tell the bot to record audio from that device.

It's really janky and badly written so dont blame me if it catches fire or runs off with your wife.

It has a bunch of dependancies but they can all be installed and managed with NuGet in visual studio, I used 2022 community.

Written in C# for .Net 6
Discord.net
NAudio
OpusDotNet
SodiumCore
Microsoft Extensions Configuration
Microsoft Extensions DependencyInjection
