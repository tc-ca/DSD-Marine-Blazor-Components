using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Miu.Shared.Utilities;
using Moq;
using Xunit.Abstractions;
using Microsoft.Extensions.Configuration;
using Miu.Web.Models.View;
using System.Linq;
using Miu.Web.Utilities;

namespace Miu.Web.Test.Unit
{
    public class WebHelperTests
    {
        private readonly ITestOutputHelper _output;
        private readonly IConfiguration _config;
        public WebHelperTests(ITestOutputHelper output)
        {
            this._config = InitConfiguration();
            _output = output;
            TrackerSteps = SessionHelper.ParseListFromConfig<TrackerStep>(_config, "ProgressTrackerSteps");
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

        [Fact]
        public void ParseListFromConfig_ShouldReturnList()
        {
            //arrange

            //act
            var result = SessionHelper.ParseListFromConfig<TrackerStep>(_config, "ProgressTrackerSteps");

            //assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IList<TrackerStep>>(result);
            Assert.True(result.Count > 0);

            (result as List<TrackerStep>).ForEach(t => _output.WriteLine($"Path: {t.Path}, Name: {t.Name}, Current: {t.Status}"));
            _output.WriteLine("===============================================================================================");

        }

        public IList<TrackerStep> TrackerSteps { get; set; }

        [Theory]
        [InlineData("en/contactinfo", 1, "/contactinfo")]
        [InlineData("fr/servicedetails", 1, "/servicedetails")]
        [InlineData("/contactinfo", 1, "/contactinfo")]
        [InlineData("/#", 1, "/#")]
        public void UpdateStatuses_ShouldUpdateItemStatuses(string currentPath, int expectedCount, string expectedCurrentPath)
        {
            //arrange
            //string currentPath = "/contactinfo";

            //act
            TrackerSteps = WebHelper.UpdateStatuses(TrackerSteps, currentPath);

            //assert
            Assert.Equal(expectedCount, TrackerSteps.Where(t => t.Status == TrackerStatus.Active).Count());
            Assert.Equal(expectedCurrentPath, TrackerSteps.FirstOrDefault(t => t.Status == TrackerStatus.Active).Path);

            //output
            (TrackerSteps as List<TrackerStep>).ForEach(t => _output.WriteLine($"Path: {t.Path}, Name: {t.Name}, Current: {t.Status}"));
            _output.WriteLine("===============================================================================================");
        }

    }
}
