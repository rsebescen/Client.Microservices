using MainUI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainUI.ValueObjects
{
    public class CompositePages
    {
        IEnumerable<CompositePage> _compositePages;

        public CompositePage[] Pages => _compositePages.ToArray();

        public CompositePages(IEnumerable<CompositePage> compositePages)
        {
            _compositePages = compositePages;
        }

        internal CompositePage ThatMatch(Func<CompositePage, bool> doesMatch)
        {
            return _compositePages.FirstOrDefault(doesMatch);
        }
    }
}
