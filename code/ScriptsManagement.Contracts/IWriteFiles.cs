using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptsManagement.Contracts
{
    public interface IWriteFiles
    {
        void WriteFile(string fileName, string content);
    }
}
