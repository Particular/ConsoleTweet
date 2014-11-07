using System;
using CommandLine;
using CommandLine.Text;
using TweetSharp;

namespace ConsoleTweet
{
    abstract class AuthenticationOptions
    {
        [Option("key", HelpText = "Consumer (API) key.", Required = true)]
        public string ConsumerKey { get; set; }

        [Option("secret", HelpText = "Consumer secret.", Required = true)]
        public string ConsumerSecret { get; set; }

        [Option("token", HelpText = "Access token.", Required = true)]
        public string AccessToken { get; set; }

        [Option("token_secret", HelpText = "Access token secret.", Required = true)]
        public string AccessTokenSecret { get; set; }
    }

    class UpdateSubOptions : AuthenticationOptions
    {
        [Option('m', "message", HelpText = "Status update", Required = true)]
        public string Message { get; set; }
    }

    class Options
    {
        [VerbOption("update", HelpText = "Post status update.")]
        public UpdateSubOptions UpdateVerb { get; set; }

        [HelpVerbOption]
        public string DoHelpForVerb(string verbName)
        {
            return HelpText.AutoBuild(this, verbName);
        }
    }


    class Program
    {
        static int Main(string[] args)
        {
            var options = new Options();

            var result = 1;

            if (!Parser.Default.ParseArgumentsStrict(args, options, (verb, subOptions) =>
            {
                if (verb == "update" && subOptions != null)
                {
                    result = PostUpdate((UpdateSubOptions)subOptions);
                }
            }, () => { }))
            {
                return 1;
            }
            return result;
        }

        private static int PostUpdate(UpdateSubOptions options)
        {
            var service = new TwitterService(options.ConsumerKey, options.ConsumerSecret);
            service.AuthenticateWith(options.AccessToken, options.AccessTokenSecret);

            service.SendTweet(new SendTweetOptions
            {
                Status = options.Message,
            });
            return 0;
        }
    }
}
