using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    [Serializable]
    internal class MissingException : Exception
    {
        string disExsisting;
        string typeObj;

        public MissingException() : base() { }
        public MissingException(string message) : base(message) { }
        public MissingException(string message, Exception inner) : base(message, inner) { }
        protected MissingException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public MissingException(string type, string disExg="")
        {
            disExsisting = disExg;
            typeObj = type;
        }

        public override string ToString()
        {
            return "MissingException : From Dal layer \n "
                + typeObj + " " + disExsisting + " was not found in the system\n";
        }
    }
}
