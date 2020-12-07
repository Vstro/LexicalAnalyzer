using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer
{
    enum LexemType
    {
        Keyword,
        Identifier,
        Uint,
        Delimiter,
        Assignment,
        Comment,
        Undefined
    }
}
