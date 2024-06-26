namespace CGames
{
    public interface ISaver<T> where T : Data
    {
        /// <summary> Loads data from save file to all scripts in the handling list. </summary>
        public void LoadData();

        /// <summary> Loads all values from given data. </summary>
        public void OverrideData(T data);

        /// <summary> Saves data from all scripts in the handling list to the save file. </summary>
        public void SaveData();
    }
}