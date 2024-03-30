using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        public int Id { get; set; }
        #region relationships
        public List<Book> Books { get; set; }
        #endregion
        public string Name { get; set; }
    }
}
