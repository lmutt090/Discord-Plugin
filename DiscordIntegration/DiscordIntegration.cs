// Required namespaces
using System;
using System.Threading.Tasks;
using ArchiSteamFarm.Plugins;
using ArchiSteamFarm.Steam;
using DiscordRPC;

namespace DiscordIntegration {
    public class DiscordIntegrationPlugin : IBotSteamClient
    {
        private readonly DiscordRpcClient _discordClient;

        public DiscordIntegrationPlugin()
        {
            // Initialize the Discord RPC client with the application ID
            _discordClient = new DiscordRpcClient("1328446672132505680");
            _discordClient.Initialize();
        }

        public string Name => "Discord-Integration";

        public Version Version => new Version(1, 0, 0, 0);

        public void OnLoaded() {
            ASF.ArchiLogger.LogGenericInfo("Discord-Integration plugin loaded successfully.");
        }

        public async Task OnConnected(Bot bot, SteamClient steamClient) {
            // Update Discord status when the bot connects to Steam
            UpdateDiscordStatus(bot, "Connected to Steam");
        }

        public async Task OnDisconnected(Bot bot, SteamClient steamClient) {
            // Clear Discord status when the bot disconnects from Steam
            UpdateDiscordStatus(bot, "Disconnected from Steam");
        }

        public void UpdateDiscordStatus(Bot bot, string status) {
            if (!_discordClient.IsInitialized) {
                ASF.ArchiLogger.LogGenericError("Discord client not initialized.");
                return;
            }

            // Set Discord presence
            _discordClient.SetPresence(new RichPresence {
                Details = status,
                State = $"Bot: {bot.BotName}",
                Timestamps = Timestamps.Now,
                Assets = new Assets {
                    LargeImageKey = "steam",
                    LargeImageText = "ArchiSteamFarm"
                }
            });

            ASF.ArchiLogger.LogGenericInfo($"Discord status updated: {status}");
        }

        public void Dispose() {
            _discordClient.Dispose();
        }
    }
}
