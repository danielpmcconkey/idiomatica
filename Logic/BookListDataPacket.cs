using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Services.API;
using Model.Enums;

namespace Logic
{
    public class BookListDataPacket
    {
        public BookListDataPacket()
        {

        }
        public BookListDataPacket(IdiomaticaContext context, bool isBrowse)
        {
            OrderByOptions[1] = "Difficulty";
            OrderByOptions[2] = "Language";
            OrderByOptions[3] = "Completed";
            OrderByOptions[4] = "Title";
            OrderByOptions[5] = "Total Pages";
            OrderByOptions[6] = "Total Word Count";
            OrderByOptions[7] = "Distinct Word Count";

            LanguageOptions = LanguageApi.LanguageOptionsRead(context, 
                (x => x.IsImplementedForLearning == true));
            
            ShouldShowOnlyInShelf = !isBrowse;
        }
        public int BookListRowsToDisplay { get; set; } = 10;
        public Dictionary<int, string> OrderByOptions { get; set; } = [];
        public Dictionary<Guid, Language> LanguageOptions { get; set; } = [];
        public List<BookListRow>? BookListRows { get; set; } = null;
        public long? BookListTotalRowsAtCurrentFilter { get; set; } = null;
        public int SkipRecords { get; set; } = 0;
        public string? TagsFilter { get; set; } = null;
        public AvailableLanguageCode? LcFilterCode { get; set; } = null;
        public Language? LcFilter
        {
            get
            {
                if(LcFilterCode is null) return null;
                var kvp = LanguageOptions
                    .Where(x => x.Value.Code == LcFilterCode)
                    .FirstOrDefault();
                return kvp.Value;
            }
        }
        public AvailableBookListSortProperties SortProperty
        {
            get
            {
                if (OrderBy != null) return (AvailableBookListSortProperties)(int)OrderBy;
                return AvailableBookListSortProperties.TITLE;
            }
        }
        public string? TitleFilter { get; set; } = null;
        public int? OrderBy { get; set; } = 4;    // title
        public bool ShouldSortAscending { get; set; } = true;
        public bool ShouldShowOnlyInShelf { get; set; } = true;
    }
}
