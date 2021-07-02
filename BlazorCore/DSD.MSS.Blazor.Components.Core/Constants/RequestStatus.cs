namespace DSD.MSS.Blazor.Components.Core.Constants
{
    using System.ComponentModel;
    /// <summary>
    /// Defines the status of the work request
    /// </summary>
    public enum RequestStatus
    {
        [Description("New")]
        NEW,

        [Description("In Progress")]
        IN_PROGRESS,

        [Description("Pending")]
        PENDING,

        [Description("Complete")]
        COMPLETE,

        [Description("Cancelled")]
        CANCELLED,

        [Description("Unknown")]
        UNKNOWN
    }
}
