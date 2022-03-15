using System;
using System.Collections.Generic;
using System.Text;

namespace DSD.MSS.Blazor.Components.Core.Models
{
    /// <summary>
    /// Object used to transfer comment info between components, used in the comment component's event callback method
    /// </summary>
    class Comment
    {
        public string CommentText { get; set; }

        public DateTime? CommentCreatedDate { get; set; }
    }
}
