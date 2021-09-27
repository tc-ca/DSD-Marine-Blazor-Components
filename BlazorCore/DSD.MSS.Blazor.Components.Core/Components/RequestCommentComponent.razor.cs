﻿namespace DSD.MSS.Blazor.Components.Core.Components
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

        /// <summary>
        /// Callback used when submitting a comment
        /// </summary>
        [Parameter]
        public EventCallback<string> SubmitComment { get; set; }

        [Parameter]
        public EventCallback<List<RequestCommentInfo>> WorkCommentsChanged { get; set; }
        
        /// <summary>
        /// Model used to change the text in the component. Used for localization.
        /// </summary>
        [Parameter]
        public CommentTextModel TextModel { get; set; }
        
        /// <summary>
        /// Callback used for the sort dropdown
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnSortChange { get; set; }
        
        /// <summary>
        /// List of sorting options for the comments list.
        /// </summary>
        [Parameter]
        public static List<SelectListItem> SortOptions { get; set; }

        public int MaxShownComments { get; set; } = 5;

        public bool ReadOnly { get; set; }

        private SortOption SortOption { get; set; } = new SortOption();

        public EditContext EditContext { get; set; }

        private List<RequestCommentInfo> workComments { get; set; }

        private Comment requestComment { get; set; } = new Comment();

        private bool areAllCommentsShown { get; set; }

       

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
          //insert default text
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

            //insert default sorting options
            if (SortOptions.Count <= 0)
            {
                SortOptions = new List<SelectListItem>() {
            new SelectListItem { Id = "1", Text = TextModel.SortOrderNew },
            new SelectListItem { Id = "2", Text = TextModel.SortOrderOld }
           };
            }
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
                this.requestComment.CommentText = string.Empty;
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
        protected virtual async Task OnChangeAsync(ChangeEventArgs e)
        {
            await this.OnSortChange.InvokeAsync(e);
        }
        private bool IsCommentListEmpty() => this.WorkComments == null || !this.WorkComments.Any();
    }
}