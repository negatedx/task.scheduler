using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class SwitchesController : Controller
    {
        private string _connectionString = "Server=.\\mssqlserver02;Database=SchedulerSample;User Id=sa;Password=password;";

        [HttpGet]
        public List<Switch> Get()
        {
            var switches = new List<Switch>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Switch", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var s = new Switch
                        {
                            SwitchId = reader.GetInt32(0),
                            Colour = reader.GetString(1),
                            IsOn = reader.GetBoolean(2)
                        };
                        Console.WriteLine($"{s.SwitchId} | {s.Colour} | {s.IsOn}");
                        switches.Add(s);
                    }
                }
            }

            return switches;
        }

        [HttpGet("{id}")]
        public Switch Get(int id)
        {
            return GetSwitch(id);
        }

        private Switch GetSwitch(int id)
        {
            var s = new Switch();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"SELECT * FROM Switch where SwitchId = {id}", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        s = new Switch
                        {
                            SwitchId = reader.GetInt32(0),
                            Colour = reader.GetString(1),
                            IsOn = reader.GetBoolean(2)
                        };
                        Console.WriteLine($"{s.SwitchId} | {s.Colour} | {s.IsOn}");
                    }
                }
            }

            return s;
        }

        [HttpPut("toggle/{id}")]
        public void Toggle(int id)
        {
            var currentVal = GetSwitch(id).IsOn;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"update Switch set IsOn = {(currentVal ? 0 : 1)} where SwitchId = {id}", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class Switch
    {
        public int SwitchId { get; set; }
        public string Colour { get; set; }
        public bool IsOn { get; set; }
    }
}
