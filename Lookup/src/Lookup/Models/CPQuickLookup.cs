namespace DSD.MSS.Blazor.Components.AddressComplete
{
    public class CPQuickLookup
    {   
        //Canada Post return data strucutre based on quick lookup AddressCompleteInteractiveFindv210 interactive search
        public string Id { get; set; }
        public string Text { get; set; }
        public string Highlight { get; set; }
        public int Cursor { get; set; }
        public string Description { get; set; }
        public string Next { get; set; }
        public string Error { get; set; }
        public string Cause { get; set; }
        public string Resolution { get; set; }

        public string Summary 
        { 
            get { return $"{Text} {Description}"; } 
        }
        public string Formatted { 
            get {
                string formatted = string.Empty;
                if (string.IsNullOrEmpty(Highlight) ||
                    string.IsNullOrWhiteSpace(Highlight) ||
                    Highlight.Length < 3)
                {
                    formatted = $"<strong>{Text}</strong>&nbsp;{Description}";
                }
                else
                {
                    formatted = $"{Text} {Description}";
                    string[] highlights = Highlight.Split(',');

                    for(int i = highlights.Length - 1; i >= 0; i--)
                    {
                        string highlight = highlights[i];

                        int low = int.Parse(highlight.Split('-')[0]);
                        int high = int.Parse(highlight.Split('-')[1]);

                        // Start from the right end of string to insert html tags, 
                        //  this preserves indices for left side of string
                        if (high < formatted.Length - 1) // boundary condition
                        {
                            formatted = formatted.Insert(high + 1, "</strong>");
                        }
                        else
                        {
                            formatted = $"{formatted}</strong>";
                        }

                        if (low > 0)
                        {
                            formatted = formatted.Insert(low, "<strong>");
                        }
                        else // boundary condition
                        {
                            formatted = $"<strong>{formatted}";
                        }
                    }
                    formatted = $"<span>{formatted}</span>";
                }

                return formatted; 
            } 
        }

        //Id, String, The Id to be used as the LastId with the Find method., CAN | PR | X247361852 | E | 0 | 0
        //Text, String, The found item., 2701 Riverside Dr, Ottawa, ON
        //Highlight, String, A list of number ranges identifying the characters to highlight in the Text response(zero - based start position and end). , 0 - 2,6 - 4
        //Cursor, Integer,  A zero-based position in the Text response indicating the suggested position of the cursor if this item is selected.A - 1 response indicates no suggestion is available., 0
        //Description, String, Descriptive information about the found item, typically if it's a container., 102 Streets 
        //Next, String, The next step of the search process., Values(Find Retrieve), Retrieve

        // Formatted is a combination of the text and description fields, with select text highlighted
    }
}
