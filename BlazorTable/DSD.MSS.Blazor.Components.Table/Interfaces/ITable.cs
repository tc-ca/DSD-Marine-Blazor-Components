
using System.Resources;

namespace DSD.MSS.Blazor.Components.Table
{
    /// <summary>
    /// DSD.MSS.Blazor.Components.Table Interface
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Page Size
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Allow Columns to be reordered
        /// </summary>
        bool ColumnReorder { get; set; }

        /// <summary>
        /// Current Page Number
        /// </summary>
        int PageNumber { get; set; }

        /// <summary>
        /// Search placeholder text
        /// </summary>
        string SearchPlaceHolderText { get; set; }

        /// <summary>
        /// Total Count of Items
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Page list Start Number
        /// </summary>
        public int PageListStartNumber { get; set; }

        /// <summary>
        /// Page list End Number
        /// </summary>
        public int PageListEndNumber { get; set; }

        /// <summary>
        /// Total Pages
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Is Table in Edit mode
        /// </summary>
        bool IsEditMode { get; }

        /// <summary>
        /// Go to First Page
        /// </summary>
        void FirstPage();

        /// <summary>
        /// Go to Next Page
        /// </summary>
        void NextPage();

        /// <summary>
        /// Go to Previous Page
        /// </summary>
        void PreviousPage();

        /// <summary>
        /// Go to Last Page
        /// </summary>
        void LastPage();

        /// <summary>
        /// Go to updated Page
        /// </summary>
        void UpdatePage(int pageNumber);

        /// <summary>
        /// Redraws the Table using EditTemplate instead of Template
        /// </summary>
        void ToggleEditMode();

        /// <summary>
        /// Table Element CSS
        /// </summary>
        string TableClass { get; set; }

        /// <summary>
        /// Table Body CSS
        /// </summary>
        string TableBodyClass { get; set; }

        /// <summary>
        /// Table Head CSS
        /// </summary>
        string TableHeadClass { get; set; }

        /// <summary>
        /// Redraws Table without Getting Data
        /// </summary>
        void Refresh();

        /// <summary>
        /// Gets Data and redraws the Table
        /// </summary>
        void Update();

        /// <summary>
        /// Set the EmptyDataTemplate for the table
        /// </summary>
        /// <param name="template"></param>
        void SetEmptyDataTemplate(EmptyDataTemplate template);

        /// <summary>
        /// Set the LoadingDataTemplate for the table
        /// </summary>
        /// <param name="template"></param>
        void SetLoadingDataTemplate(LoadingDataTemplate template);

        /// <summary>
        /// Select Type: None, Single or Multiple
        /// </summary>
        public SelectionType SelectionType { get; set; }

        /// <summary>
        /// Search all columns for the specified string, supports spaces as a delimiter
        /// </summary>
        string GlobalSearch { get; set; }

        /// <summary>
        /// Shows Search Bar above the table
        /// </summary>
        bool ShowSearchBar { get; set; }

        /// <summary>
        /// Resource manager
        /// </summary>
        ResourceManager ResourceManager { get; set; }
    }
}