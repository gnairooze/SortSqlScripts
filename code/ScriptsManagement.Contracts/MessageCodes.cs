using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptsManagement.Contracts
{
    public class MessageCodes
    {
        public const string NoFilesToReorder = "9001";//"No files to reorder"
        public const string FileNotFound = "9002";//"File {0} not found in the list of files"
        public const string FileMoved = "9003";//"File {0} moved to position {1}";
        public const string FileAlreadyAtPosition = "9004";//"File {0} is already at position {1}. It is dependant on itself."
    }
}
