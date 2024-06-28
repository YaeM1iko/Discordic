using Discordbot_Config;
using Discord.Net;
using Discord.WebSocket;
using Discord.Interactions;
using Newtonsoft.Json;
using Discord;
using Discordbot_Services;
using Discordbot_Commands;
using System.Threading.Tasks;
using System;

namespace Discordbot
{
    class Bot
    {
        private DiscordSocketClient client;
        private readonly Config config = new Config();
        private readonly ulong guildId = 125;
        private InteractionService interactionService;
        private CommandService commandService;

        static void Main(string[] args)
            => new Bot().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            DiscordSocketConfig discordConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds |
                     GatewayIntents.GuildMessages |
                     GatewayIntents.MessageContent
            };

            client = new DiscordSocketClient(discordConfig);
            interactionService = new InteractionService(client);
            commandService = new CommandService(config);

            client.Log += Log;
            client.Ready += Client_Ready;
            client.SlashCommandExecuted += SlashCommandHandler;
            client.MessageReceived += CommandsHandler;

            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();

            Console.ReadLine();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task Client_Ready()
        {
            var guild = client.GetGuild(guildId);

            var hoholCommand = new HoholCommand();
            try
            {
                await guild.CreateApplicationCommandAsync(hoholCommand.BuildCommand().Build());
            }
            catch (HttpException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }

            await interactionService.AddModulesAsync(System.Reflection.Assembly.GetExecutingAssembly(), null);
        }

        private async Task CommandsHandler(SocketMessage msg)
        {
            await commandService.HandleMessageCommand(msg);
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            await commandService.HandleSlashCommand(command);
        }
    }
}
