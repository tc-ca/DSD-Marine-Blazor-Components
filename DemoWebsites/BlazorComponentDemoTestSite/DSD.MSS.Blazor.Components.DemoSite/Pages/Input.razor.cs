namespace DSD.MSS.Blazor.Components.Core.Demo.Pages
{
    using DSD.MSS.Blazor.Components.Core.Demo.Model;
    using DSD.MSS.Blazor.Components.Core.Models;
    using Microsoft.AspNetCore.Components.Forms;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class Input 
    {
        protected FormInputModel FormInputObj { get; set; }

        protected FormInputModel[] FormInputListObj { get; set; }

        protected EditContext editContext;
        protected List<SelectListItem> selectlist { get; set; } = new List<SelectListItem>();

        protected override async Task OnInitializedAsync()
        {
            FormInputObj = new FormInputModel();
            FormInputListObj = await InputList.GetFormInputListAsync(DateTime.Now);
            editContext = new EditContext(FormInputObj);
            selectlist.Add(new SelectListItem()
            {
                Text = "A",
                Value = true
            });
            selectlist.Add(new SelectListItem()
            {
                Text = "B",
                Value = false
            });
        }

        private void HandleValidSubmit()
        {
            var isValid = editContext.Validate();
            Console.WriteLine("Valid Submit");
        }
        private void OnInputChange(EventArgs e)
        {

        }
    }
}
