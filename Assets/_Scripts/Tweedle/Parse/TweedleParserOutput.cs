using System.IO;
using System.Text;
using UnityEngine;
using System;

namespace Alice.Tweedle.Parse
{
    internal class TweedleParserOutput : TextWriter
    {
        public bool IsError { get; }

        public TAssembly Assembly { get;  }


        public TweedleParserOutput(bool isError, TAssembly assembly = null)
        {
            IsError = isError;
            Assembly = assembly;
        }

        public override void Write(char value)
        {
            throw new NotImplementedException();
        }

        public override void Write(char[] value, int buffer, int count)
        {
            StringWriter writer = new StringWriter();
            writer.Write(value, buffer, count);

            string assemblyName = Assembly != null ? Assembly.Name : "(unknown)";

            if (IsError)
            {
                Debug.LogErrorFormat("[Tweedle Parser] {0}: {1}", assemblyName, writer.ToString());
            } else
            {
                Debug.LogFormat("[Tweedle Parser] {0}: {1}", assemblyName, writer.ToString());
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}