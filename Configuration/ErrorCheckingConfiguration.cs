using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ErrorChecking.Configuration
{
    public class ErrorCheckingConfiguration
    {
        public string FileName { get; set; }
        public List<string> ErrorsToCheck {get;set;}
    }
}