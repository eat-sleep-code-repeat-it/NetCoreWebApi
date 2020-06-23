using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternConsole.Singleton
{
    public interface ISingletonContainer
    {
        int GetPopulation(string name);
    }
}
