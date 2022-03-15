using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Models
{
    /// <summary>
    /// Model used when creating and populating a list of comments
    /// </summary>
    public class RequestCommentInfo
    {
        /// <summary>
        /// Id for the comment
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Actual text from the component
        /// </summary>
        public string Comment { get; set; }
        
        /// <summary>
        /// Time when the comment was created
        /// </summary>
        public DateTimeOffset? CreatedDateUTC { get; set; }
        
        /// <summary>
        /// Datetime formatted to be displayed above the comment, can be customized
        /// </summary>
        public string CreatedDateFormatted { get; set; }
        
        /// <summary>
        /// name or ID of the user who submitted the comment
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
