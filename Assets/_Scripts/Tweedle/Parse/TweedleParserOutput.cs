using System.IO;
using System.Text;
using UnityEngine;
using System;

namespace Alice.Tweedle.Parse
{
    internal class TweedleParserOutput : TextWriter
    {
        public bool IsError { get; }


        public TweedleParserOutput(bool isError)
        {
            IsError = isError;
        }

        public override void Write(char value)
        {
            throw new NotImplementedException();
        }

        public override void Write(char[] value, int buffer, int count)
        {
            StringWriter writer = new StringWriter();
            writer.Write(value, buffer, count);

            if (IsError)
            {
                Debug.LogError(writer.ToString());
            } else
            {
                Debug.Log(writer.ToString());
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}