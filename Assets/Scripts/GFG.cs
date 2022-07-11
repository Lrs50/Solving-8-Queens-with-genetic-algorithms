using System;
using System.Collections.Generic;

class GFG : IComparer<DNA>
{
    public int Compare(DNA p1, DNA p2)
    {
        if (p1 == null || p2 == null)
        {
            return 0;
        }

        return  p1.CompareTo(p2);;
          
    }
}