using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Demo.Web.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text.Json;
using System.Threading.Tasks;
using Res = Demo.Web.Resources.Shared;

namespace Demo.Web.Services
{
    public class InteropService : IInteropService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<InteropService> _logger;

        public InteropService(IJSRuntime jSRuntime, ILogger<InteropService> logger)
        {
            this._jsRuntime = jSRuntime;
            this._logger = logger;
        }

        public async Task WriteModelToLocalStorageAsync<T>(object model, string objectName)
        {

            var jsonStr = JsonSerializer.Serialize((T)model, new JsonSerializerOptions { IgnoreReadOnlyProperties = true });

            //iterate through jsonStr properties, and call encryption on each value
            await _jsRuntime.InvokeAsync<object>("stateManager.save", objectName, jsonStr);
        }

        public async Task<T> ReadModelFromLocalStorageAsync<T>(object model, string objectName)
        {
            string storedJsonString;
            T outModel = (T)model;
            try
            {
                storedJsonString = await _jsRuntime.InvokeAsync<string>("stateManager.load", objectName);
            }
            // we are pre-rendering. No worries; will be called again when the client
            // is ready
            catch (InvalidOperationException)
            {
                return (T)model;
            }
            if (!string.IsNullOrEmpty(storedJsonString))
            {
                try
                {
                    var storedModel = JsonSerializer.Deserialize<T>(storedJsonString);
                    outModel = storedModel;
                    //to decrypt, comment out line above, and iterate through item properties and decrypt, skip collection propertes.
                    //if (storedModel != null)
                    //{
                    //    foreach (PropertyInfo prop in outModel.GetType().GetProperties())
                    //    {
                    //        if (prop.PropertyType.FullName.StartsWith("System.Collections.Generic.List")) continue;
                    //        var storedValue = storedModel.GetType().GetProperty(prop.Name).GetValue(storedModel);
                    //        //Decrypt storedValue here 
                    //        prop.SetValue(outModel, storedValue);
                    //    }
                    //}
                }
                catch (Exception ex)
                {                    
                    _logger.LogError(ex.Message);
                }
            }

            return outModel;
        }

        public async Task<string> GetInputValueById(string inputId)
        {
            var value = string.Empty;
            try
            {
                value = await _jsRuntime.InvokeAsync<string>("stateManager.getInputValue", inputId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return value;
        }

        public async Task RunJavascriptFunctionByName(string functionName)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync(functionName);
                _logger.LogInformation($"Javascript function called: {functionName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task RunJavascriptFunctionByName(string functionName, object[] parameters)
        {
            try
            {
                await _jsRuntime.InvokeAsync<object[]>(functionName, parameters);
                _logger.LogInformation($"Javascript function called: {functionName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
