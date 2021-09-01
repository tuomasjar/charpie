using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using csharpie;

namespace csharpi.Modules
{
    // for commands to be available, and have the Context passed to them, we must inherit ModuleBase
    public class ExampleCommands : ModuleBase
    {
        
        [Command("hello")]
        public async Task HelloCommand()
        {
            // initialize empty string builder for reply
            var sb = new StringBuilder();

            // get user info from the Context
            var user = Context.User;
            
            // build out the reply
            sb.AppendLine($"You are -> [{user.Username}]");
            sb.AppendLine("I must now say, World!");
            sb.Append("Restarts: ");
            sb.AppendLine(Starters.starts.ToString());
            // send simple string reply
            await ReplyAsync(sb.ToString());
        }

        [Command("roll")]
        public async Task RollDice([Remainder]string args = null)
        {
            var sb = new StringBuilder();
            var embed = new EmbedBuilder();
            int result=0;
            embed.WithColor(new Color(0,0,255));
            embed.WithTitle("This is how I roll");
            if(args==null){
                sb.AppendLine("Nothing to roll");
            }else if(!args.Contains("d")){
                sb.AppendLine("Nothing to roll");
            }else{
                Random rand = new Random();
                String input = args.ToString();
                String[] parts = input.Split("d");
                int lkm = 0;
                int eyes= 0;
                try{
                    int.TryParse(parts[0],out lkm);
                    int.TryParse(parts[1],out eyes);
                }catch(FormatException e){
                    sb.AppendLine("Nothing to roll");
                }
                if(lkm != 0 && eyes != 0){
                    for(int i = 0;i<lkm;i++){
                        int random = rand.Next(eyes);
                        random++;
                        if(i!=0)sb.Append("+");
                        sb.Append(random.ToString());
                        result+=random;
                    }
                }else{
                    sb.AppendLine("Nothing to roll");
                }
                if(result != 0){
                    sb.Append("="+result.ToString());
                }

            }
            embed.Description = sb.ToString();
            await ReplyAsync(null, false, embed.Build());
        }

        [Command("8ball")]
        [Alias("ask")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task AskEightBall([Remainder]string args = null)
        {
            // I like using StringBuilder to build out the reply
            var sb = new StringBuilder();
            // let's use an embed for this one!
            var embed = new EmbedBuilder();

            // now to create a list of possible replies
            var replies = new List<string>();

            // add our possible replies
            replies.Add("yes");
            replies.Add("no");
            replies.Add("maybe");
            replies.Add("hmm what?");

            // time to add some options to the embed (like color and title)
            embed.WithColor(new Color(0, 255,0));
            embed.Title = "Welcome to the 8-ball!";
            
            // we can get lots of information from the Context that is passed into the commands
            // here I'm setting up the preface with the user's name and a comma
            sb.AppendLine($"{Context.User.Username},");
            sb.AppendLine();
            // let's make sure the supplied question isn't null 
            if (args == null)
            {
                // if no question is asked (args are null), reply with the below text
                sb.AppendLine("Sorry, can't answer a question you didn't ask!");
            }
            else 
            {
                // if we have a question, let's give an answer!
                // get a random number to index our list with (arrays start at zero so we subtract 1 from the count)
                var answer = replies[new Random().Next(replies.Count - 1)];
                
                // build out our reply with the handy StringBuilder
                sb.AppendLine($"You asked: [**{args}**]...");
                sb.AppendLine();
                sb.AppendLine($"...your answer is [**{answer}**]");

                // bonus - let's switch out the reply and change the color based on it
                switch (answer) 
                {
                    case "yes":
                    {
                        embed.WithColor(new Color(0, 255, 0));
                        break;
                    }
                    case "no":
                    {
                        embed.WithColor(new Color(255, 0, 0));
                        break;
                    }
                    case "maybe":
                    {
                        embed.WithColor(new Color(255,255,0));
                        break;
                    }
                    case "hmm what?":
                    {
                        embed.WithColor(new Color(255,0,255));
                        break;
                    }
                }
            }

            // now we can assign the description of the embed to the contents of the StringBuilder we created
            embed.Description = sb.ToString();

            // this will reply with the embed
            await ReplyAsync(null, false, embed.Build());
        }
    }
}