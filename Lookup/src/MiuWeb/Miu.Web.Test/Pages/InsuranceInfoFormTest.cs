using Bunit;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Miu.Web.Models.View;
using Miu.Web.Pages;
using Miu.Web.Services;
using MIUClient.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Miu.Web.Test.Pages
{
    [System.Obsolete]
    public class InsuranceInfoFormTest : ComponentTestFixture
    {
        private readonly ITestOutputHelper _output;
        public InsuranceInfoFormTest(ITestOutputHelper output)
        {
            _output = output;
            var stringLocalizerMock = new Mock<IStringLocalizer<InsuranceInfoForm>>();
            var stringLocalizerCommonMock = new Mock<IStringLocalizer<Miu.Web.Shared.Common>>();
            var insInfoServiceMock = new Mock<IInsuranceInfoService>();
            var jsRuntimeMock = new Mock<IJSRuntime>();
            var fileUploadMock = new Mock<IFileUpload>();
            var configMock = new Mock<IConfiguration>();

            //configMock.Setup(theObject => theObject.GetValue<double>(It.Is<string>(i => i == "MaxFileUploadSize"))).Returns<double>(x => 3072000);

            Services.AddSingleton<IStringLocalizer<InsuranceInfoForm>>(stringLocalizerMock.Object);
            Services.AddSingleton<IStringLocalizer<Miu.Web.Shared.Common>>(stringLocalizerCommonMock.Object);
            Services.AddSingleton<IInsuranceInfoService>(insInfoServiceMock.Object);
            Services.AddSingleton<IJSRuntime>(jsRuntimeMock.Object);
            Services.AddSingleton<IFileUpload>(fileUploadMock.Object);
            Services.AddSingleton<IConfiguration>(configMock.Object);                    
            Services.AddSingleton<IValidator<InsuranceInfo>>(new Miu.Web.Validation.InsuranceInfoValidator());
            Services.AddSingleton<IAppStateManager>(new AppState.Manager(new AppState(new ClientApplicationContext())));
        }
        [Fact(DisplayName = "Parameters are correctly assigned on render")]
        public async Task Test005()
        {
            //Arrange
            bool formEdited = false;
            const string lang = "fr";
            const string securityType = "Security Type";
            const string formTitle = "Insurance Info Form Title";

            //Act
            var cut = base.RenderComponent<InsuranceInfoForm>(
               (nameof(InsuranceInfoForm.Title), formTitle),
               (nameof(InsuranceInfoForm.Model), new InsuranceInfo { SecurityType = securityType }),
               (nameof(InsuranceInfoForm.Language), lang),
               (nameof(InsuranceInfoForm.MaxFileUploadSize), 3072000.00),
               EventCallback<bool>(nameof(InsuranceInfoForm.FormEditedCallback), () => formEdited = true)
               );

            var instance = cut.Instance;

            //Asset
            Assert.Equal(formTitle, instance.Title);
            Assert.NotNull(instance.Model);
            Assert.Equal(securityType, instance.Model.SecurityType);
            Assert.Equal(lang, instance.Language);

            Assert.False(formEdited);
            //cut.Find("input#SecurityType").SetAttribute("value", "Force");
            cut.Find("input#SecurityType").Change("Force");            
            await instance.FormChangedAsync();
           
            Assert.True(formEdited);
            _output.WriteLine(cut.Markup);
        }
    }
}
