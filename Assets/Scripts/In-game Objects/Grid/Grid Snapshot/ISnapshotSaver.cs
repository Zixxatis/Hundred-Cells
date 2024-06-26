namespace CGames
{
    public interface ISnapshotSaver
    {
        public void Initialize(Cell[,] cellsMatrix);
        public void CaptureData();
    }
}