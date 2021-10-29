namespace Alice.Tweedle
{
    public class StaticStackFrame : IStackFrame {
        private string m_StackFrameEntry;
    
        public StaticStackFrame(string stackFrameEntry) {
            m_StackFrameEntry = stackFrameEntry;
        }

        public string ToStackFrame() {
            return m_StackFrameEntry;
        }
    }
}