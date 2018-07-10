using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PagedQuery  
    {
        public int RequestedPage { get; set; }
        public int PagedSize { get; set; }
        public IList<SortFieldExpression> SortFieldExpressions { get; set; }
        public String SearchString { get; set; }
    }

    public class SortFieldExpression
    {
        public string FieldName { get; set; }
        public bool Descending { get; set; }
    }
}
