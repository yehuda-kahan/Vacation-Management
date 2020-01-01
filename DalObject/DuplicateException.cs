using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    [Serializable]
    internal class DuplicateException : Exception
    {
        string exsisting;
        string type;

        public DuplicateException() : base() { }
        public DuplicateException(string message) : base(message) { }
        public DuplicateException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public DuplicateException(string typeObj, string exg)
        {
            exsisting = exg;
            type = typeObj;
        }

        public override string ToString()
        {
            return "DuplicateException : From Dal layer \n "
                + type + " " + exsisting + " alrady exsist in the system\n";
        }
    }
}
