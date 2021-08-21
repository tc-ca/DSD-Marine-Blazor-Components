namespace DSD.MSS.Blazor.Components.Core.Components
{
using DSD.MSS.Blazor.Components.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

    public partial class RequestCommentComponent
    {
        [Parameter]
        public List<RequestCommentInfo> WorkComments
        {
            get => workComments;
            set
            {
                if (workComments == value) return;
                this.workComments = value;
                WorkCommentsChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public bool ReadOnly { get; set; }

        [Parameter]
        public EventCallback<string> SubmitComment { get; set; }

        [Parameter]
        public EventCallback<List<RequestCommentInfo>> WorkCommentsChanged { get; set; }
        
        [Parameter]
        public CommentTextModel TextModel { get; set; }

        public int MaxShownComments { get; set; } = 5;

        private SortOption SortOption { get; set; } = new SortOption();

        public EditContext EditContext { get; set; }

        private List<RequestCommentInfo> workComments { get; set; }

        private CommentDTO requestComment { get; set; } = new CommentDTO();

        private bool areAllCommentsShown { get; set; }

        public static List<SelectListItem> SortOptions { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (this.WorkComments == null)
            {
                this.WorkComments = new List<RequestCommentInfo>();
                this.setMaxToCurrentCount();
            }
            if (this.WorkComments.Any())
            {
                this.WorkComments = WorkComments.OrderByDescending(o => o.CreatedDateUTC).ToList();
                this.setMaxToCurrentCount();
            }
            else
            {
                this.areAllCommentsShown = true;
            }
          
            if(this.TextModel == null)
            {
                this.TextModel = new CommentTextModel()
                {
                    AddCommentBtn = "Add Comment",
                    CommentsLabel = "Comments",
                    NoCommentsLabel = "No Comments",
                    LoadMoreCommentBtn = "Load More Comments",
                    SortOrderNew = "Newest",
                    SortOrderOld = "Oldest"
                };
            }
            SortOptions = new List<SelectListItem>() {
            new SelectListItem { Id = "1", Text = TextModel.SortOrderNew },
            new SelectListItem { Id = "2", Text = TextModel.SortOrderOld }
           };

            this.EditContext = new EditContext(this.requestComment);
           
        }

        public async void SaveComment()
        {
            if (!string.IsNullOrWhiteSpace(this.requestComment.CommentText))
            {
                if (this.EditContext.Validate())
                {
                    await SubmitComment.InvokeAsync(this.requestComment.CommentText);

                }
                this.MaxShownComments += 1;
                this.requestComment.CommentText = null;
            }
        }

        public void ChangeSortOrder()
        {
            if (string.Equals(this.SortOption.Text, "2"))
            {
                this.WorkComments = WorkComments.OrderByDescending(o => o.CreatedDateUTC).ToList();
            }
            else
            {
                this.WorkComments = WorkComments.OrderBy(o => o.CreatedDateUTC).ToList();
            }
         
        }

        private void LoadMoreComments()
        {
            this.MaxShownComments += 10;
            this.setMaxToCurrentCount();
        }

        private void setMaxToCurrentCount()
        {
            if (this.MaxShownComments >= this.WorkComments.Count)
            {
                this.MaxShownComments = this.WorkComments.Count;
                this.areAllCommentsShown = true;
            }
        }

        private bool IsCommentListEmpty() => this.WorkComments == null || !this.WorkComments.Any();
    }

  
}
