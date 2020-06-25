using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SchedulerApi.ApiClients
{
    public class SwitchesApiClient
    {
        private Uri _switchesApiUrl = new Uri("http://localhost:53990/api/switches/toggle/");

        public async Task FireSwitchToggleRequestAsync(int id)
        {
            using (var client = new HttpClient())
            {
                await client.PutAsync(new Uri(_switchesApiUrl, $"{id}"), null);
            }
        }
    }
}
