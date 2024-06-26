namespace CGames
{
    public interface ISavable<T> where T : Data
    {        
        /// <summary> Passes some values to given data. </summary>
        public void PassData(T data);

        /// <summary> Reads some values from given data. </summary>
        public void ReceiveData(T data);
    }
}