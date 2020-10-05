using Microsoft.AspNetCore.Components.Forms;
using Miu.Web.Models.View;
using Miu.Web.Services;
using Xunit;

namespace Miu.Web.Test.Services
{
    public class AppStateManagerTests
    {
        private readonly AppState _appState;
        private readonly IAppStateManager _appStateManager;
        private readonly ClientApplicationContext  _clientApplicationContext;

        public AppStateManagerTests()
        {
            _clientApplicationContext = new ClientApplicationContext();
            _appState = new AppState(_clientApplicationContext);
            _appStateManager = new AppState.Manager(_appState);
        }

        [Fact(DisplayName = "On SetClientApplicationContext AppState should have non-null edit contexts")]
        public void Test001()
        {
            // Arrange
            ContactInfo contactInfoModel = new ContactInfo { FirstName = "FirstName", Index = UserInfoIndex.Applicant };
            ContactInfo mailingInfoModel = new ContactInfo { FirstName = "FirstName", Index = UserInfoIndex.MailRecipient };
            InsuranceInfo insuranceInfoModel = new InsuranceInfo { NameOfInsurer = "InsurerName" };
            VesselInfo vesselInfoModel = new VesselInfo { Name = "VesselName" };

            // Act
            _appStateManager.SetClientApplicationContext(new EditContext(contactInfoModel));
            _appStateManager.SetClientApplicationContext(new EditContext(mailingInfoModel));
            _appStateManager.SetClientApplicationContext(new EditContext(insuranceInfoModel));
            _appStateManager.SetClientApplicationContext(new EditContext(vesselInfoModel));

            // Assert
            Assert.NotNull(_appState.ClientApplicationState.ApplicantContext);
            Assert.NotNull(_appState.ClientApplicationState.MailRecipientContext);
            Assert.Single<EditContext>(_appState.ClientApplicationState.VesselInfoContexts);
        }

        [Fact(DisplayName="On Clear AppState should have null edit contexts")]
        public void Test002()
        {
            // Arrange
            ContactInfo contactInfoModel = new ContactInfo { FirstName = "FirstName", Index = UserInfoIndex.Applicant };
            ContactInfo mailingInfoModel = new ContactInfo { FirstName = "FirstName", Index = UserInfoIndex.MailRecipient };
            InsuranceInfo insuranceInfoModel = new InsuranceInfo { NameOfInsurer = "InsurerName" };
            VesselInfo vesselInfoModel = new VesselInfo { Name = "VesselName" };
            _appStateManager.SetClientApplicationContext(new EditContext(contactInfoModel));
            _appStateManager.SetClientApplicationContext(new EditContext(mailingInfoModel));
            _appStateManager.SetClientApplicationContext(new EditContext(insuranceInfoModel));
            _appStateManager.SetClientApplicationContext(new EditContext(vesselInfoModel));

            // Act
            _appStateManager.Clear();

            // Assert
            Assert.Null(_appState.ClientApplicationState.ApplicantContext);
            Assert.Null(_appState.ClientApplicationState.MailRecipientContext);
            Assert.True(_appState.ClientApplicationState.VesselInfoContexts.Count < 1);
        }
    }
}
