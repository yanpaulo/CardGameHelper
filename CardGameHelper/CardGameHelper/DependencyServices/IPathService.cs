using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameHelper.DependencyServices
{
    public interface IPathService
    {
        string GetLocalPath(string filename);
    }
}
