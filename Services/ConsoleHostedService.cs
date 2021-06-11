using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ErrorChecking.Configuration;

namespace ErrorChecking.Services
{
  public class ConsoleHostedService : IHostedService
  {
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly IOptions<ErrorCheckingConfiguration> _errcConfig;
    private readonly IConfiguration _config;

    public ConsoleHostedService(
        IHostApplicationLifetime appLifetime,
        IOptions<ErrorCheckingConfiguration> errcConfig,
        IConfiguration config
        )
    {
        _appLifetime = appLifetime;
        _errcConfig = errcConfig;
        _config = config;
    }

public Task StartAsync(CancellationToken cancellationToken)
    {

        _appLifetime.ApplicationStarted.Register(() =>
        {
            Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("Checking errors");

                    Dictionary<string, int> errors = new Dictionary<string, int>();
                    string fileLines  = File.ReadAllText(_errcConfig.Value.FileName);
                    Console.WriteLine(fileLines.Length.ToString());
                    string[] words = {"neco"};

                    for(int i = 0; i < fileLines.Length; i++)
                    {
                        if(fileLines[i] != '（')
                            continue;

                        string err = FindSubstring(fileLines, i);

                        // if(_errcConfig.Value.ErrorsToCheck.Contains(err))
                        // {
                            if(!errors.ContainsKey(err))
                                errors.Add(err, 1);
                            else
                                errors[err]++;
                        // }
                    }

                    foreach(KeyValuePair<string, int> kvp in errors)
                    {
                        Console.WriteLine(kvp.Key + " : " + kvp.Value );
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            });
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public string FindSubstring(string fileLines, int index)
    {
        StringBuilder foundError = new StringBuilder();
        int tmpIndex = index;

        // Console.WriteLine(index.ToString());

        do {
            foundError.Append(fileLines[tmpIndex]);
        } while(fileLines[tmpIndex++] != '）' && tmpIndex - index < 7 && tmpIndex != fileLines.Length - 1);

        // Console.WriteLine(foundError.ToString());
        return foundError.ToString();
    }
  }
}