using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    class DescendedStringComparer 
        : IComparer<String>
    {
        public int Compare(String x, String y)
        {
            // use the default comparer to do the original comparison for strings
            int ascendingResult = Comparer<String>.Default.Compare(x, y);

            // turn the result around
            return 0 - ascendingResult;
        }
    }
    public class MicroblogUserModel
    {
        public string Name { get; set; }

        public SortedList<String, Tweet> feed = new SortedList<string,Tweet>(new DescendedStringComparer());
    }
}