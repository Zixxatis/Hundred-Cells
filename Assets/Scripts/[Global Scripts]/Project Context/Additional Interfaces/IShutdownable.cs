namespace CGames
{   
    /// <summary> Implement this interface to be able to use Unity's "OnApplicationQuit" in non-MonoBehaviour classes. </summary>
    public interface IQuitable
    {
        public void PrepareToQuit();
    }
}