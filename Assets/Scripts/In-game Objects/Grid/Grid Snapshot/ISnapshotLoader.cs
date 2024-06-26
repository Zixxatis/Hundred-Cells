namespace CGames
{
    public interface ISnapshotLoader
    {
        public bool HasCapturedData();
        public SessionData GetDataFromSnapshot();
    }
}