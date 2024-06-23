using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using System;
using System.Threading.Tasks;
using DISCORD_BOT_Config;
using Discord.Net;
using Newtonsoft.Json;

namespace Discord_bot_4
{
    class Bot
    {
        private DiscordSocketClient client;
        private readonly Config config = new Config();
       // private readonly ulong guildId = 
        private InteractionService interactionService;

        static void Main(string[] args)
            => new Bot().MainAsync().GetAwaiter().GetResult();
        private async Task MainAsync()
        {
            DiscordSocketConfig discordConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            client = new DiscordSocketClient(discordConfig);
            interactionService = new InteractionService(client);

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

            var guildCommand = new SlashCommandBuilder();
            guildCommand.WithName("hohol")
                        .WithDescription("Много шуток про негров!");
            try
            {
                await guild.CreateApplicationCommandAsync(guildCommand.Build());
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
            if (!msg.Author.IsBot)
            {
                if (msg.Content.Contains("Хохол"))
                {
                    Random rnd = new Random();
                    string[] jokes = config.jokes;
                    string randomJoke = jokes[rnd.Next(jokes.Length)];
                    await msg.Channel.SendMessageAsync(randomJoke);
                }
            }
        }
        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name == "hohol")
            {
                Random rnd = new Random();
                string[] jokes = config.jokes;
                string randomJoke = jokes[rnd.Next(jokes.Length)];
                await command.RespondAsync(randomJoke);
            }
        }
    }
}