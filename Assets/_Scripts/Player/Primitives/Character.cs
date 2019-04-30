using System.Diagnostics;
using Alice.Tweedle.Interop;

namespace Alice.Player.Primitives
{
    [PInteropType]
    public class Character
    {
        public readonly char Value;

        #region Interop Interfaces

        [PInteropConstructor]
        public Character(string theChar)
        {
            if(theChar.Length == 1)
            {
                char[] chars = theChar.ToCharArray();
                Value = chars[0];
            }
            else
                Value = '\0';
        }

        public Character(char theChar)
        {
            Value = theChar;
        }

        [PInteropMethod]
        public bool equals(Character other) {
            return Value == other.Value;
        }

        [PInteropMethod]
        public string getString() {
            return string.Format("{0}", Value);
        }

        #endregion

        public override string ToString() {
            return string.Format("{0}", Value);
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }
    
    }
}