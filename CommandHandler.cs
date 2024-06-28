using Discord;

namespace Discordbot_Commands
{

    public abstract class BaseCommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract SlashCommandBuilder BuildCommand();
    }

    public class HoholCommand : BaseCommand
    {
        public override string Name => "hohol";
        public override string Description => "Много шуток про негров!";

        public override SlashCommandBuilder BuildCommand()
        {
            return new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription(Description);
        }
    }
}
