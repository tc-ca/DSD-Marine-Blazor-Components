using AddressComplete.Demo.Models.View;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressComplete.Demo.Services
{
    public interface IInteropService
    {
        Task<T> ReadModelFromLocalStorageAsync<T>(object model, string objectName);
        /// <summary>
        /// Writes model of type T to browser local storage by calling a JavaScript function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        Task WriteModelToLocalStorageAsync<T>(object model, string objectName);
        Task<string> GetInputValueById(string inputId);

        /// <summary>
        /// Calls a JavaScript function to clear localStorage for domain
        /// </summary>
        /// <returns></returns>
        //Task ClearLocalStorage();

        /// <summary>
        /// Calls JavaScript function to apply modal window styling to the body tag (bootstrap broken)
        /// </summary>
        /// <returns></returns>
        //Task SetBodyModalMode();

        /// <summary>
        /// Calls JavaScript function to remove modal window styling to the body tag (bootrap broken)
        /// </summary>
        /// <returns></returns>
        //Task ClearBodyModalMode();

        /// <summary>
        /// Calls a javascript function
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        Task RunJavascriptFunctionByName(string functionName);

        Task RunJavascriptFunctionByName(string functionName, object[] parameters);
    }
}