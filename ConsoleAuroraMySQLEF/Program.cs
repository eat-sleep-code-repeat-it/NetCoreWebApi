using ConsoleAuroraMySQLEF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleAuroraMySQLEF
{
    // https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core-scaffold-example.html
    // https://www.chaseaucoin.com/posts/aurora-serverless-lambda-with-entity-framework-core/
    // https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core-example.html

    class Program
    {
        //you can find this in your database details
        static readonly string _auroraAddress = "localhost";//"{endpoint}";
        static readonly string _auroraPassword = "7344744653";//"{randomlyGeneratedPassword}";

        //address of the ubuntu instance we created avilable in the EC2 instance panel
        static readonly string _sshTunnelAddress = "ec2-00-00-00-0.compute-1.amazonaws.com";

        //default is ubuntu
        static readonly string _sshUserName = "ubuntu";
        static readonly string _keyFileName = "mykey.pem";
        static bool isDebug = false;
        public static JokeContext GetContext()
        {
            return new JokeContext(_auroraAddress, _auroraPassword, _sshTunnelAddress, _sshUserName, _keyFileName, isDebug);
        }

        static void Main(string[] args)
        {
            var path = @"C:/workspace/NetCoreWebApi/ConsoleAuroraMySQLEF/wocka.json";
            var jsonJokes = File.ReadAllText(path);
            var jokes = JsonConvert.DeserializeObject<IEnumerable<Joke>>(jsonJokes);

            JokeContext context = GetContext();
            context.Database.EnsureCreated();

            var jokeCount = context.Jokes.Count();
            if (jokeCount == 0)
            {
                foreach (var batch in jokes.Batch(5000))
                {
                    context.Set<Joke>().AddRange(batch);
                    context.ChangeTracker.DetectChanges();
                    context.SaveChanges();
                    context?.Dispose();
                    context = GetContext();

                    Console.WriteLine("Writing to serverless database");
                }
            }


            //
            var oneLiners = context.Jokes.Where(joke => joke.Category == "One Liners").ToList();
            var randomOneLiner = oneLiners.OrderBy(x => Guid.NewGuid()).First();
            Console.WriteLine($"There are {oneLiners.Count} one liners!");
            Console.WriteLine(randomOneLiner.Body);

            Console.WriteLine("Hello World!");
        }
    }
}
