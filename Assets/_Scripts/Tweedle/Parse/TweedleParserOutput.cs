using System.IO;
using System.Text;
using UnityEngine;
using System;

namespace Alice.Tweedle.Parse
{
    internal class TweedleParserOutput : TextWriter
    {
        public bool IsError { get; }

        public TAssembly Assembly { get; }

        public string FileName { get; }

        private readonly StringBuilder builder = new StringBuilder();

        public TweedleParserOutput(bool isError, TAssembly assembly = null, string fileName = null)
        {
            IsError = isError;
            Assembly = assembly;
            FileName = fileName;
        }

        public override void Write(char value)
        {
            throw new NotImplementedException();
        }

        public override void Write(char[] value, int buffer, int count)
        {
            builder.Clear();
            builder.Append(value, buffer, count);
            
            string assemblyName = Assembly != null ? Assembly.Name : "(unknown)";
            string fileName = FileName ?? "(unknown)";

            if (IsError)
            {
                Debug.LogErrorFormat("[Tweedle Parser] {0} {1}: {2}", assemblyName, fileName, builder.ToString());
            } else
            {
                Debug.LogFormat("[Tweedle Parser] {0} {1}: {2}", assemblyName, fileName, builder.ToString());
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}