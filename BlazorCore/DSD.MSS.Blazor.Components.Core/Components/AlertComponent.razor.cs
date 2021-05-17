namespace DSD.MSS.Blazor.Components.Core.Components
{
    using DSD.MSS.Blazor.Components.Core.Constants;
    using Microsoft.AspNetCore.Components;

    public partial class AlertComponent
    {
        [Parameter] 
        public string Title { get; set; }

        [Parameter] 
        public string Message { get; set; }

        [Parameter]
        public bool ShowIcon { get; set; } = true;

        [Parameter] 
        public RenderFragment ChildContent { get; set; }

        [Parameter] 
        public bool EnableShow { get; set; }

        [Parameter]
        public bool EnableCancel { get; set; }

        [Parameter] 
        public AlertTypes AlertType { get; set; }

        [Parameter] 
        public bool Dismisable { get; set; } = false;

        protected string AlertTypeCssString
        {
            get
            {
                switch(this.AlertType)
                {
                    case AlertTypes.Danger:
                        return "alert-danger";
                    case AlertTypes.Information:
                        return "alert-info";
                    case AlertTypes.Success:
                        return "alert-success";
                    case AlertTypes.Warning:
                        return "alert-warning";
                    case AlertTypes.Primary:
                    default:
                        return "alert-primary";
                }
            }
        }

        protected  string AlertFontAwesomeIconString
        {
            get
            {
                switch (this.AlertType)
                {
                    case AlertTypes.Danger:
                        return "fa-ban";
                    case AlertTypes.Success:
                        return "fa-check-circle";
                    case AlertTypes.Warning:
                        return "fa-exclamation-triangle";
                    case AlertTypes.Information:
                        return "fa-exclamation-circle";
                    case AlertTypes.Primary:
                    default:
                        return "fa-flag";
                }
            }
        }
        protected void HandleCancelClick()
        {
            EnableShow = false;
        }
    }
}
