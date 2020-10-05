using GoC.WebTemplate.Components.Core.Services;
using GoC.WebTemplate.Components.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Demo.Web.Resources.Shared;
using System;
using System.Collections.Generic;
using System.Resources;

namespace Demo.Web.Views
{
    public class LayoutViewModel
    {
        private readonly ModelAccessor _modelAccessor;
        private readonly ResourceManager _localizer;

        public LayoutViewModel(ModelAccessor modelAccessor)
        {
            this._modelAccessor = modelAccessor;
            this._localizer = new ResourceManager(typeof(Common));
        }

        public void InitializePage()
        {

            GoC.WebTemplate.Components.Model WebTemplateModel;
            WebTemplateModel = _modelAccessor.Model;

            WebTemplateModel.HeaderTitle = _localizer.GetString("AppLayoutHeaderTitle");
            WebTemplateModel.ApplicationTitle.Text = _localizer.GetString("AppLayoutTitle_Text");
            WebTemplateModel.ApplicationTitle.Href = "/";
            WebTemplateModel.ApplicationTitle.Acronym = _localizer.GetString("ApplicationTitle_Acronym");
            WebTemplateModel.ApplicationTitle.NewWindow = true;

            WebTemplateModel.HTMLHeaderElements = new List<string> {
            "<meta name=\"keywords\" content=\"Certification,Marine,Insurance,Unit,TC,Cost,Recovery\">",
            "<meta name=\"description\" content=\"MIU external application\">",
            "<meta name=\"author\" content=\"Team Narwhals\">"
            };

            WebTemplateModel.DateModified = new DateTime(2020, 04, 29);
            WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Href = "https://www.canada.ca", Title = _localizer.GetString("Breadcrumb_Home") });
            WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Href = "https://www.canada.ca/en/services/transport.html", Title = _localizer.GetString("Breadcrumb_TCServices") });
            WebTemplateModel.Breadcrumbs.Add(new Breadcrumb { Href = "https://www.tc.gc.ca/en/services/marine.html", Title = _localizer.GetString("Breadcrumb_MarineInsurance") });
        }

    }
}
