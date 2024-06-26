namespace CGames
{
    /// <summary> A class for storing a preview language in Editor Mode. </summary>
    public static class PreviewTextAssistant
    {
        public static Language PreviewLanguage { get; set; }

        public static void SetPreviewLanguage(Language language)
        {
            PreviewLanguage = language;
        }
    }
}