using DSD.MSS.Blazor.Components.Core.Demo.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Demo.Data
{
    public class FormInputList
    {
        private static readonly string[] Values = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<FormInputModel[]> GetFormInputListAsync(DateTime startDate)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new FormInputModel
            {
                ID = index,
                DateValue = startDate.AddDays(index),
                IntValue = rng.Next(-20, 55),
                StringValue = Values[rng.Next(Values.Length)]
            }).ToArray());
        }
    }
}
