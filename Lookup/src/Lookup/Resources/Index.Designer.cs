﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DSD.MSS.Blazor.Components.AddressComplete.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Index {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Index() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DSD.MSS.Blazor.Components.AddressComplete.Resources.Index", typeof(Index).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unit.
        /// </summary>
        internal static string Address2Prefix {
            get {
                return ResourceManager.GetString("Address2Prefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///    { 
        ///        &quot;iso2&quot;: &quot;AF&quot;, 
        ///        &quot;iso3&quot;: &quot;AFG&quot;, 
        ///        &quot;name&quot;: &quot;Afghanistan&quot;, 
        ///        &quot;name_fr&quot;: &quot;Afghanistan&quot;, 
        ///        &quot;flag&quot;: 1 },
        ///    { &quot;iso2&quot;: &quot;AX&quot;, &quot;iso3&quot;: &quot;ALA&quot;, &quot;name&quot;: &quot;Åland&quot;, &quot;name_fr&quot;: &quot;Åland(les Îles)&quot;, &quot;flag&quot;: 220 },
        ///    { &quot;iso2&quot;: &quot;AL&quot;, &quot;iso3&quot;: &quot;ALB&quot;, &quot;name&quot;: &quot;Albania&quot;, &quot;name_fr&quot;: &quot;Albanie&quot;, &quot;alternates&quot;: [ &quot;Shqipëria&quot; ], &quot;flag&quot;: 2 },
        ///    { &quot;iso2&quot;: &quot;DZ&quot;, &quot;iso3&quot;: &quot;DZA&quot;, &quot;name&quot;: &quot;Algeria&quot;, &quot;name_fr&quot;: &quot;Algérie&quot;, &quot;flag&quot;: 3 },
        ///    { &quot;iso2&quot;: &quot;AS&quot;, &quot;iso3&quot;: &quot;ASM&quot;, &quot;name&quot;: &quot;American [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string countries {
            get {
                return ResourceManager.GetString("countries", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must enter at least {0} characters..
        /// </summary>
        internal static string MinimumLengthMessage {
            get {
                return ResourceManager.GetString("MinimumLengthMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No results found.
        /// </summary>
        internal static string NotFoundMessage {
            get {
                return ResourceManager.GetString("NotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please pick a country.
        /// </summary>
        internal static string PickACountryMessage {
            get {
                return ResourceManager.GetString("PickACountryMessage", resourceCulture);
            }
        }
    }
}
