using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NovelCovidAPI.Module.HttpRequest;
using NovelCovidAPI.Module.Dataset;

namespace BackgroundWorker
{
    public class Worker : BackgroundService
    {
        #region Properties

        private readonly int RequestDelay = 1000 * 5;
        private readonly ILogger<Worker> Logger;
        private SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        private JsonSerializerSettings JsonSerializerSettings;
        private HttpClient HttpClient;

        private HttpRequest<List<CountryV1>> CountryV1;
        private HttpRequest<List<CountryV2>> CountryV2;
        private HttpRequest<General> General;
        private HttpRequest<List<Historical>> Historical;
        private HttpRequest<List<State>> States;

        #endregion

        #region Worker

        public Worker(ILogger<Worker> logger)
        {
            // init local
            Logger = logger;

            // init variable
            JsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            HttpClient = new HttpClient();

            // init request
            CountryV1 = new HttpRequest<List<CountryV1>>(HttpClient, @"https://corona.lmao.ninja/v2/jhucsse", JsonSerializerSettings);
            CountryV2 = new HttpRequest<List<CountryV2>>(HttpClient, @"https://corona.lmao.ninja/v2/countries", JsonSerializerSettings);
            General = new HttpRequest<General>(HttpClient, @"https://corona.lmao.ninja/v2/all", JsonSerializerSettings);
            Historical = new HttpRequest<List<Historical>>(HttpClient, @"https://corona.lmao.ninja/v2/historical", JsonSerializerSettings);
            States = new HttpRequest<List<State>>(HttpClient, @"https://corona.lmao.ninja/v2/states", JsonSerializerSettings);
        }

        public override Task StartAsync(CancellationToken cancellationToken) => base.StartAsync(cancellationToken);
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Semaphore.WaitAsync();

                var res1 = await RequestGeneral();
                var res2 = await RequestCountryv1();
                var res3 = await RequestCountryv2();
                var res4 = await RequestHistorical();
                var res5 = await RequestStates();

                Semaphore.Release();
                await Task.Delay(RequestDelay, stoppingToken);
                Logger.Log(LogLevel.Information, $"{DateTime.Now} - All Request OK!");
            }
        }

        #endregion

        #region Request

        private async Task<General> RequestGeneral() => await General.GetAsync();
        private async Task<List<CountryV1>> RequestCountryv1() => await CountryV1.GetAsync();
        private async Task<List<CountryV2>> RequestCountryv2() => await CountryV2.GetAsync();
        private async Task<List<Historical>> RequestHistorical() => await Historical.GetAsync();
        private async Task<List<State>> RequestStates() => await States.GetAsync();

        #endregion
    }
}
