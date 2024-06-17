using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using DISCORD_BOT_Config;

namespace Discord_bot_4
{
    class Bot
    {
        DiscordSocketClient client;
        private readonly Config config = new Config();

        static void Main(string[] args)
            => new Bot().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            DiscordSocketConfig discordConfig = new DiscordSocketConfig();
            discordConfig.GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent;

            client = new DiscordSocketClient(discordConfig);

            client.MessageReceived += CommandsHandler;
            client.Log += Log;

            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();

            Console.ReadLine();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task CommandsHandler(SocketMessage msg)
        {
            if (!msg.Author.IsBot)
            {
                switch (msg.Content)
                {
                    case string command when command.Contains("Хохол"):
                        Random rnd = new Random();
                        string[] jokes = config.jokes;
                        string randomJoke = jokes[rnd.Next(jokes.Length)];
                        await msg.Channel.SendMessageAsync(randomJoke);
                        break;
                }

            }
        }
    }
}