using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexAnalyzer
{
    public enum State
    {
        Start,
        Input,
        Identifier,
        Constant,
        OpenPar,
        ClosePar,
        Comp,
        RevComp,
        EqComp,
        Del,
        Ao,
        As,
        Error,
        Final
    }
}
