namespace CGames
{
    /// <summary> Implement this interface to be able to use Unity's "Awake" in non-MonoBehaviour classes. </summary>
    public interface IEarlyInitializable
    {
        public void InitializeEarly();
    }
}