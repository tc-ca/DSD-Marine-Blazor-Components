using Bunit;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Miu.Web.Models.View;
using Miu.Web.Pages;
using Res = Miu.Web.Resources.Pages;
using Miu.Web.Services;
using Moq;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.AspNetCore.Components;

namespace Miu.Web.Test.Pages
{
    public class MiuFormStateInitializerTests : ComponentTestFixture
    {
        private readonly ITestOutputHelper _output;
        public MiuFormStateInitializerTests(ITestOutputHelper output)
        {
            _output = output;
            //var stringLocalizerMock = new Mock<IStringLocalizer<MiuFormStateInitializer>>();
            //var vesselInfoServiceMock = new Mock<IVesselInfoService>();
            //var certificateTypeService = new Mock<ICertificateTypeService>();

            Services.AddSingleton<ClientApplicationContext>();
            Services.AddSingleton<AppState>();
            Services.AddSingleton<IAppStateManager, AppState.Manager>();
            //Services.AddSingleton<IStringLocalizer<MiuFormStateInitializer>>(stringLocalizerMock.Object);
            //Services.AddSingleton<IVesselInfoService>(vesselInfoServiceMock.Object);
            //Services.AddSingleton<ICertificateTypeService>(certificateTypeService.Object);
            //Services.AddSingleton<IValidator<VesselInfo>>(new Miu.Web.Validation.VesselInfoValidator());
            //Services.AddSingleton<IAppStateManager>(new AppState.Manager(new AppState(new ClientApplicationContext())));
        }

        [Fact(DisplayName = "Apps state correctly initialized on render")]
        public void Test001()
        {
            // Arrange
            const string vesselName = "Vessel Name";

            var vessel = new VesselInfo { Name = vesselName };

            // Act
            var cut = base.RenderComponent<MiuFormStateInitializerFake>(
                (nameof(MiuFormStateInitializerFake.EditContext), new EditContext(vessel))
                );
            var instance = cut.Instance;

            // Assert
            Assert.NotNull(instance.EditContext);
            Assert.Equal(vesselName, ((VesselInfo)instance.EditContext.Model).Name);

            var appState = Services.GetService<AppState>();
            Assert.NotNull(appState.ClientApplicationState.VesselInfoContexts);
            Assert.Single(appState.ClientApplicationState.VesselInfoContexts);
            Assert.Equal(vesselName, ((VesselInfo)appState.ClientApplicationState.VesselInfoContexts.First().Model).Name);

        }
    }

    /// <summary>
    /// Fake component to wrap MiuFormStateInitializer's OnInitialized method and set cascading parameter CurrentEditContext which currently cannot be set with bUnit
    /// </summary>
    public class MiuFormStateInitializerFake : MiuFormStateInitializer
    {
        [Parameter]
        public EditContext EditContext { get; set; }

        protected override void OnInitialized()
        {
            base.CurrentEditContext = EditContext;
            base.OnInitialized();
        }
    }
}
