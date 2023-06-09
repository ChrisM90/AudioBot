using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NAudio.CoreAudioApi;
using NAudio.Gui;
using NAudio.Wave;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Threading.Channels;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using Discord.Audio;
using System.IO;
using System.Text.RegularExpressions;
using NAudio.Wave.SampleProviders;
using System.Collections.Concurrent;
using System.Text;

namespace AudioBot
{
    public partial class Main : Form
    {

        private WasapiLoopbackCapture CaptureInstance;
        readonly MMDeviceCollection devices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        private MMDevice SelectedDevice;
        private bool Recording = false;

        string DISCORD_TOKEN = "YOUR_TOKEN_HERE";
        private IAudioClient audioClient;
        private bool JoinedChannel = false;
        private IConfiguration _config;
        private InteractionService _commands;
        private DiscordSocketClient _client;
        private List<IAudioChannel> Channels;
        private IAudioChannel SelectedChannel;
        private AudioOutStream AO;

        public Main()
        {
            InitializeComponent();
        }

        private Task Log(LogMessage msg)
        {
            Debug.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            audioDropdown.Items.Clear();

            foreach (MMDevice device in devices)
            {
                try
                {
                    audioDropdown.Items.Add(device.FriendlyName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            var start = Start();
            start.Wait();
        }

        private async Task Start()
        {
            var _builder = new ConfigurationBuilder();
   
            _config = _builder.Build();

            var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordSocketClient>();
            var commands = services.GetRequiredService<InteractionService>();
            _client = client;
            _commands = commands;

            client.Log += Log;
            commands.Log += Log;
            client.Ready += Ready;

            await client.LoginAsync(TokenType.Bot, DISCORD_TOKEN);
            await client.StartAsync();

            await services.GetRequiredService<CommandHandler>().InitializeAsync();
        }

        private async Task Ready()
        {
            var guilds = _client.Guilds;
            Channels = new List<IAudioChannel>();

            channelDropdown.Invoke((MethodInvoker)delegate
            {
                channelDropdown.Items.Clear();
            });

            foreach (SocketGuild guild in guilds)
            {
                foreach (SocketGuildChannel channel in guild.Channels)
                {
                    if (channel.GetChannelType() == ChannelType.Voice)
                    {
                        Channels.Add((IAudioChannel)channel);
                        channelDropdown.Invoke((MethodInvoker)delegate
                        {
                            channelDropdown.Items.Add(channel);
                        });
                    }
                }
            }
        }

        private void startStopAudio_Click(object sender, EventArgs e)
        {
            if (!Recording)
            {
                this.CaptureInstance = new WasapiLoopbackCapture(SelectedDevice);
                this.CaptureInstance.WaveFormat = new WaveFormat(48000, 16, 2);
                this.CaptureInstance.ShareMode = AudioClientShareMode.Shared;
                AO = audioClient.CreatePCMStream(AudioApplication.Music);
                BufferedWaveProvider bwp = new BufferedWaveProvider(this.CaptureInstance.WaveFormat);
                bwp.BufferDuration = new TimeSpan(0, 0, 3);
                bwp.DiscardOnBufferOverflow = true;
                TimeSpan OneSecond = new TimeSpan(0, 0, 1);
                byte[] copybuffer;

                this.CaptureInstance.DataAvailable += async (s, a) =>
                {
                    try
                    {
                        bwp.AddSamples(a.Buffer, 0, a.BytesRecorded);
                        
                        if (bwp.BufferedDuration > OneSecond)
                        {
                            copybuffer = new byte[(bwp.BufferedBytes / 2)];
                            bwp.Read(copybuffer,0 , copybuffer.Length);
                            await AO.WriteAsync(copybuffer, 0, copybuffer.Length);
                        }
                    }
                    finally
                    { }
                };

                this.CaptureInstance.RecordingStopped += (s, a) =>
                {
                    AO.Dispose();
                    CaptureInstance.Dispose();
                };

                this.CaptureInstance.StartRecording();
                Recording = true;
                audioDropdown.Enabled = false;
                joinChannel.Enabled = false;
                startStopAudio.Text = "Stop Audio";
            }
            else
            {
                this.CaptureInstance.StopRecording();
                Recording = false;
                audioDropdown.Enabled = true;
                joinChannel.Enabled = true;
                startStopAudio.Text = "Start Audio";
            }
        }

        private void joinChannel_Click(object sender, EventArgs e)
        {
            if (!JoinedChannel)
            {
                var s = SelectedChannel.ConnectAsync(false, false, false);
                s.Wait();
                audioClient = (IAudioClient)s.Result;
                channelDropdown.Enabled = false;
                joinChannel.Text = "Leave";
                JoinedChannel = true;
                startStopAudio.Enabled = true;
            }
            else
            {
                var s = SelectedChannel.DisconnectAsync();
                s.Wait();
                channelDropdown.Enabled = true;
                joinChannel.Text = "Join";
                JoinedChannel = false;
                startStopAudio.Enabled = false;
            }
        }

        private void channelDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedChannel = Channels[channelDropdown.SelectedIndex];
        }

        private void audioDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDevice = devices[audioDropdown.SelectedIndex];
        }
    }
}
