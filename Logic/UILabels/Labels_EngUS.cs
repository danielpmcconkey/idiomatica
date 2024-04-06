using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UILabels
{
	public class Labels_EngUS : UILabels
	{
		
		protected override void FillLabelsDict()
		{
            // todo: come up with error codes and put the human readable into the language packs

            _labels.Add("loading", "Loading...");
			_labels.Add("noDataReturned", "No data returned.");
			_labels.Add("unknown", "unknown");
            _labels.Add("error", "Error");

			#region book list
			_labels.Add("bookListFormFilter", "Filter...");
			_labels.Add("bookListLanguageColumnHead", "Language");
			_labels.Add("bookListCompletedColumnHead", "");
			_labels.Add("bookListTitleColumnHead", "Title");
			_labels.Add("bookListProgressColumnHead", "Progress");
			_labels.Add("bookListTotalWordCountColumnHead", "Total Word Count");
			_labels.Add("bookListTotalKnownPercentColumnHead", "Total Known Percent");
			_labels.Add("bookListDistinctWordCountColumnHead", "Distinct Word Count");
			_labels.Add("bookListDistinctKnownPercentColumnHead", "Distinct Known Percent");
            _labels.Add("bookListNoBookNotice", "No books exist. Please add some.");
            _labels.Add("btnUpdateBookListStats", "Update booklist statistics");
            #endregion
            #region read
            _labels.Add("readErrorRetrievingData", "There was an error retrieving this book from the database.");
            _labels.Add("untranslatable", "untranslatable");
            _labels.Add("btnNextPage", "next page");
            _labels.Add("btnPreviousPage", "previous page");
            #endregion
            //_labels.Add("", "");
            //_labels.Add("", "");
        }
        public override string GetLabel(string name)
		{
			if(_labels.ContainsKey(name)) return _labels[name];
			return "unknown";
		}
	}
}
