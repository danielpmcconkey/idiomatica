﻿using System;
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
            _labels.Add("closeButton", "Close");
            _labels.Add("saveButton", "Save Changes");
            _labels.Add("getLoggedInUserError", "There was an error determining the logged in user.");

            #region nav
            _labels.Add("navHome", "Home");
            _labels.Add("navLogOut", "Log Out");
            _labels.Add("navBrowse", "Browse All Books");
            _labels.Add("navYourBookshelf", "Your Bookshelf");
            _labels.Add("navRegister", "Register");
            _labels.Add("navLogIn", "Log In");
            _labels.Add("navNewBook", "Upload Book");
            _labels.Add("navFlashcards", "Flash card review");
            #endregion

            #region home
            _labels.Add("homeInitializationError", "There was an error initializing data for the home page."); 
            _labels.Add("homeWordCountsHeader", "How many words have you read?");
            _labels.Add("homeBookshelfButtonText", "Read more...");
            _labels.Add("pickUpTitle", "Pick back up where you left off");
            _labels.Add("pickUpError", "There was an error determining your last reading location");
            _labels.Add("pickUpRead", "Read");
            _labels.Add("pickUpNoCrumbs", "No prior reading history found");
            #endregion

            #region book list
            _labels.Add("bookListInitializationError", "There was an error fetching your book list.");
            _labels.Add("bookListRemoveBookError", "There was an error removing a book from the list");
            _labels.Add("bookListSortTableError", "There was an error sorting your book list");
            _labels.Add("bookListIsVisibleError", "There was an error determining your book list filter.");
            _labels.Add("bookListRefreshStatsError", "There was an error refreshing book user stats.");
            _labels.Add("bookListAddTagError", "There was an error adding a book tag.");
            _labels.Add("bookListRefreshError", "There was an error refreshing the book list");
            _labels.Add("bookListFormFilterLanguage", "Filter on language...");
            _labels.Add("bookListFormFilterTitle", "Filter on title...");
            _labels.Add("bookListFormFilterTags", "Filter on tags...");
            _labels.Add("bookListFormFilterNone", "No filter...");
            _labels.Add("bookListFormSortColumn", "Sort by");
            _labels.Add("bookListFormSortDirection", "Sort direction...");
            _labels.Add("bookListFormSortAscending", "Sort ascending");
            _labels.Add("bookListFormSortDescending", "Sort descending");
            _labels.Add("bookListLanguageColumnHead", "Language");
			_labels.Add("bookListCompletedColumnHead", "Completed");
			_labels.Add("bookListTitleColumnHead", "Title");
			_labels.Add("bookListProgressColumnHead", "Progress");
			_labels.Add("bookListTotalWordCountColumnHead", "Total Word Count");
			_labels.Add("bookListDistinctWordCountColumnHead", "Distinct Word Count");
			_labels.Add("bookListDistinctKnownPercentColumnHead", "Percent Known");
            _labels.Add("bookListNoBookNotice", "No books exist. Please add some.");
            _labels.Add("btnUpdateBookListStats", "Update booklist statistics");
            _labels.Add("bookListRefreshBookStats", "Refresh stats");
            _labels.Add("bookListRead", "Read");
            _labels.Add("bookListUpdate", "Update");
            _labels.Add("bookListRemove", "Remove");
            _labels.Add("bookListRemoveToolTip", "Remove from your bookshelf. You can put it back later and keep your old stats.");
            _labels.Add("bookListAdd", "Add");
            _labels.Add("bookListButtonGroupLabel", "Book actions");
            _labels.Add("bookListAddToolTip", "Add to your bookshelf.");
            _labels.Add("bookListAddTag", "Add tag");
            _labels.Add("bookListBrowseForBooks", "Browse for books to add to your shelf");
            _labels.Add("bookListDifficultyLabel", "Difficulty score");
            #endregion
            #region read
            _labels.Add("readInitializationError", "There was an error retrieving this book from the database.");
            _labels.Add("readErrorPageChange", "There was an error retrieving page data from the database.");
            _labels.Add("showWordModalError", "There was an error opening the word edit form.");
            _labels.Add("showPpModalError", "There was an error opening the paragraph translation dialog.");
            _labels.Add("clearPageUserError", "There was an error updating all unknown words on the page.");
            _labels.Add("untranslatable", "untranslatable");
            _labels.Add("btnNextPage", "next page");
            _labels.Add("btnClearPage", "mark page complete");
            _labels.Add("btnPreviousPage", "previous page");
            _labels.Add("statusLabel8", "unknown 0");
            _labels.Add("statusLabel1", "new 1");
            _labels.Add("statusLabel2", "new 2");
            _labels.Add("statusLabel3", "learning 3");
            _labels.Add("statusLabel4", "learning 4");
            _labels.Add("statusLabel5", "learned 5");
            _labels.Add("statusLabel6", "ignored 6");
            _labels.Add("statusLabel7", "well known 7");
            _labels.Add("ppModalTitle", "Translation from {0} to {1}");
            _labels.Add("pageTurnerCurrentPage", "Page {0} of {1}");
            _labels.Add("readJumpDefault", "Jump to page...");
            _labels.Add("readJumpSubmit", "Jump");
            _labels.Add("createFlashCard", "Make a flash card of this word");
            _labels.Add("formatTranslationInput", "Format help SpanishDictionary.com verbs");
            _labels.Add("wordModalOverrideTranslations", "What you think this word means...");
            _labels.Add("wordModalDefaultTranslations", "What we think this word means...");
            _labels.Add("provideTranslation", "Provide your own translation");
            #endregion
            #region book create
            _labels.Add("bookCreateInitializationError", "There was an error setting up the book creation form");
            _labels.Add("bookCreateSaveError", "There was an error saving the new book");
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
            _labels.Add("bcSuccess", "Book successfully saved");
            _labels.Add("bcStartReading", "Start reading it");
            #endregion
            #region flash card review
            _labels.Add("flashCardReviewInitializationError", "There was an error initializing flash card review.");
            _labels.Add("fcLanguage", "Which language do you want to study today?");
            _labels.Add("fcSelectLanguage", "Please select language");
            //_labels.Add("fcNumNewCardsLabel", "How many new cards do you want to add?");
            //_labels.Add("fcNumOldCardsLabel", "How many review cards do you want to fetch?");
            _labels.Add("fcPullACard", "Pull a card");
            _labels.Add("fcSeeAnswer", "Show translation");
            _labels.Add("fcCustomTranslationHead", "Your personal translation");
            _labels.Add("fcWrong", "Wrong");
            _labels.Add("fcHard", "Hard");
            _labels.Add("fcGood", "Good");
            _labels.Add("fcEasy", "Easy");
            _labels.Add("fcStop", "Remove this card");
            _labels.Add("fcLoading", "Loading...");
            //_labels.Add("fcDeckCreateError", "There was an error creating the card deck");
            //_labels.Add("fcDeckComplete", "You have finished reviewing all cards in this deck. Create another?");
            _labels.Add("fcShowConjugationTable", "Show conjugation table");
            #endregion
            #region conjugation table
            _labels.Add("btnShowTranslations", "Show translations");
            _labels.Add("btnHideTranslations", "Hide translations");
            #endregion
            //_labels.Add("", "");
            //_labels.Add("", "");
        }
        public override string GetLabel(string name)
        {
            try
            {
                if (_labels.ContainsKey(name)) return _labels[name];
                return "unknown";
            }
            catch (Exception)
            {

                throw;
            }
        }
        public override string GetLabelF(string name, object?[] args)
        {
            if (_labels.ContainsKey(name)) return String.Format(_labels[name], args);
            return "unknown";
        }
    }
}
