using System;
using System.Collections.Generic;
using System.Text;

namespace DSD.MSS.Blazor.Components.Core.Models
{
    /// <summary>
    /// Model class used to localize/customize text in the comment component
    /// </summary>
    public class CommentTextModel
    {
        public string AddCommentBtn { get; set; }
        public string CommentsLabel { get; set; }
        public string LoadMoreCommentBtn { get; set; }
        public string NoCommentsLabel { get; set; }
        public string SortOrderNew { get; set; }
        public string SortOrderOld { get; set; }
    }
}
