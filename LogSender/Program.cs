using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using LogSender.Models;

namespace LogSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var stringComparison = StringComparison.InvariantCultureIgnoreCase;

            try
            {
                var logs = Directory.EnumerateFiles(config["logsPath"], config["searchPattern"]);
                var senders = config.GetSection("emailSenders").Get<EmailSenders>()
                    .Senders.Where(x => !string.IsNullOrWhiteSpace(x.From))
                    .ToArray();

                var emailClient = new EmailClient();
                foreach (var logPath in logs)
                {
                    try
                    {
                        using var stream = new StreamReader(logPath);
                        var to = stream.ReadLine()?.Replace("To:", "", stringComparison).Trim();
                        if (string.IsNullOrWhiteSpace(to))
                        {
                            continue;
                        }

                        var from = stream.ReadLine()?.Replace("From:", "", stringComparison).Trim();
                        var sender = senders.FirstOrDefault(x => x.UseForAnySender 
                                                                 || StringComparer.InvariantCultureIgnoreCase.Compare(x.From, from) == 0);
                        if (sender is null)
                        {
                            Console.WriteLine($"Not found sender {from}");
                            continue;
                        }

                        var subject = stream.ReadLine()?.Replace("Subject:", "", stringComparison).Trim();
                        var builder = new StringBuilder();
                        while (!stream.EndOfStream)
                        {
                            builder.AppendLine(stream.ReadLine());
                        }
                        var body = builder.ToString();

                        emailClient.Send(sender, to, subject, body);
                        Console.WriteLine($"Message {logPath} to {to} is send.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine();
            Console.WriteLine("All done.");
            Console.ReadKey();
        }
    }
}
