using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discordbot_Config;

namespace Discordbot_Services
{
    public class CommandService
    {
        private readonly Config _config;

        public CommandService(Config config)
        {
            _config = config;
        }

        public async Task HandleSlashCommand(SocketSlashCommand command)
        {
            if (command.Data.Name == "hohol")
            {
                Random rnd = new Random();
                string[] jokes = _config.jokes;
                string randomJoke = jokes[rnd.Next(jokes.Length)];
                await command.RespondAsync(randomJoke);
            }
        }

        public async Task HandleMessageCommand(SocketMessage msg)
        {
            if (!msg.Author.IsBot)
            {
                if (msg.Content.Contains("Хохол"))
                {
                    Random rnd = new Random();
                    string[] jokes = _config.jokes;
                    string randomJoke = jokes[rnd.Next(jokes.Length)];
                    await msg.Channel.SendMessageAsync(randomJoke);
                }
            }
        }
    }
}
