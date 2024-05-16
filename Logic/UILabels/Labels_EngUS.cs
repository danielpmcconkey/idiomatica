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
            _labels.Add("unknownError", "There was an unknown error.");
            _labels.Add("notLoggedIn", "You must be logged in to view this content.");

            #region nav
            _labels.Add("navHome", "Home");
            _labels.Add("navLogOut", "Log Out");
            _labels.Add("navYourBookshelf", "Your Bookshelf");
            _labels.Add("navRegister", "Register");
            _labels.Add("navLogIn", "Log In");
            _labels.Add("navNewBook", "Upload Book");
            #endregion

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
            _labels.Add("statusLabel8", "unknown 0");
            _labels.Add("statusLabel1", "new 1");
            _labels.Add("statusLabel2", "new 2");
            _labels.Add("statusLabel3", "learning 3");
            _labels.Add("statusLabel4", "learning 4");
            _labels.Add("statusLabel5", "learned 5");
            _labels.Add("statusLabel6", "ignored 6");
            _labels.Add("statusLabel7", "well known 7");
            _labels.Add("closeButton", "Close");
            _labels.Add("saveButton", "Save Changes");
            _labels.Add("TranslationFrom", "Translation from");
            _labels.Add("TranslationTo", "Translation to");
            #endregion
            #region book create
            _labels.Add("bcTitle", "Title");
            _labels.Add("bcLanguage", "What language is it written in?");
            _labels.Add("bcText", "Text");
            _labels.Add("sourceURI", "Source URL (if applicable)");
            _labels.Add("bcSubmit", "Save");
            _labels.Add("NewBook", "New Book");
            _labels.Add("bcSaving", "Saving...");
            _labels.Add("bcSelectLanguage", "Please select language");
            _labels.Add("bcTextAreaTooLarge", "Maximum length ({0} characters) exceeded in Text field. Please split this into multiple books.");
            _labels.Add("bcFileUploadError", "There was an error updloading that file.");
            #endregion
            //_labels.Add("", "");
            //_labels.Add("", "");
        }
        public override string GetLabel(string name)
        {
            if (_labels.ContainsKey(name)) return _labels[name];
            return "unknown";
        }
        public override string GetLabelF(string name, object?[] args)
        {
            if (_labels.ContainsKey(name)) return String.Format(_labels[name], args);
            return "unknown";
        }
    }
}
