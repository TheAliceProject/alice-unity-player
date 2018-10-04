namespace Alice.Utils
{
    public class Tuple<T1, T2>
    {
        public T1 First { get; private set; }
        public T2 Second { get; private set; }
        public Tuple(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Tuple<T1, T2>)) return false;
            return Equals((Tuple<T1, T2>)obj);
        }

        public bool Equals(Tuple<T1, T2> other)
        {
            return First.Equals(other.First) && Second.Equals(other.Second);
        }

        public override int GetHashCode()
        {
            return (((First.GetHashCode() << 5) + First.GetHashCode()) ^ Second.GetHashCode());
        }
    }

    public static class Tuple
    {
        public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
        {
            var tuple = new Tuple<T1, T2>(first, second);
            return tuple;
        }
    }
}