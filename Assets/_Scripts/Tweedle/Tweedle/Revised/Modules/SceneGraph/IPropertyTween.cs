namespace Alice.Unity {
    public interface IPropertyTween
    {
        void Step(double dt);
        bool IsDone();
        void Finish();
    }
}