using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SchedulerApi.ApiClients;
using System;

namespace TaskScheduler.Controllers
{
    [Route("api/[controller]")]
    public class SchedulerController : Controller
    {
        private SwitchesApiClient _switchesApiClient;

        public SchedulerController()
        {
            _switchesApiClient = new SwitchesApiClient();
        }

        /// <summary>
        /// Schedule the toggling of the given switch for immediate execution.
        /// </summary>
        /// <param name="id">The Id of the switch to toggle</param>
        [HttpPost]
        [Route("fire-and-forget")]
        public void FireAndForget(int id)
        {
            BackgroundJob.Enqueue(() => _switchesApiClient.FireSwitchToggleRequestAsync(id));
        }

        /// <summary>
        /// Schedule the toggling of the given switch for execution after the given delay.
        /// </summary>
        /// <param name="id">The Id of the switch to toggle</param>
        /// <param name="milliseconds">The number of milliseconds to after to wait before toggling the switch</param>
        [HttpPost]
        [Route("delayed")]
        public void Delayed(int id, int milliseconds)
        {
            BackgroundJob.Schedule(() => _switchesApiClient.FireSwitchToggleRequestAsync(id), TimeSpan.FromMilliseconds(milliseconds));
        }

        /// <summary>
        /// Schedule the toggling of the given switch for execution at a specific date and time.
        /// If the date and time provided are in the past the toggle will proceed immediately.
        /// </summary>
        /// <param name="id">The Id of the switch to toggle</param>
        /// <param name="dateTime">The specific date and time at which to toggle the switch (yyyy-MM-ddTHH:mm:ss)</param>
        [HttpPost]
        [Route("specific-time")]
        public void SpecificTime(int id, DateTime dateTime)
        {
            var delay = dateTime - DateTime.Now;
            BackgroundJob.Schedule(() => _switchesApiClient.FireSwitchToggleRequestAsync(id), delay);
        }

        /// <summary>
        /// Schedule the toggling of the given switch to reccur with the given interval.
        /// </summary>
        /// <param name="id">The Id of the switch to toggle</param>
        /// <param name="minuteInterval">The interval at which to perfom the toggle</param>
        [HttpPost]
        [Route("recurring")]
        public void Recurring(int id, int minuteInterval)
        {
            RecurringJob.AddOrUpdate(() => _switchesApiClient.FireSwitchToggleRequestAsync(id), Cron.MinuteInterval(minuteInterval));
        }
    }
}
