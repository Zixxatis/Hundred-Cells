namespace CGames
{   
    /// <summary> Implement this interface to be able to use Unity's "OnApplicationPause" in non-MonoBehaviour classes. </summary>
    public interface IPausable
    {
        public void OnPauseStarted();
        public void OnPauseFinished();
    }
}