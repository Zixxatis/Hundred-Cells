using UnityEditor;

namespace CGames.CustomEditors
{
    public class CustomObjectsCreator : Editor
    {
        [MenuItem("GameObject/UI/Localizable TMP", false, 1)]
        public static void CreateTextBlock() => GameObjectCreator.CreateObject("Text (LTMP)");

        [MenuItem("GameObject/UI/Localizable TMP (Header)", false, 1)]
        public static void CreateHeaderBlock() => GameObjectCreator.CreateObject("Header Text (LTMP)");


        [MenuItem("GameObject/UI/Button (LTMP)", false, 2)]
        public static void CreateCustomButton() => GameObjectCreator.CreateObject("Button");


        [MenuItem("GameObject/UI/Scroll View (Horizontal)", false, 3)]
        public static void CreateHorizontalSW() => GameObjectCreator.CreateObject("Scroll View (H)");
        
        [MenuItem("GameObject/UI/Scroll View (Vertical)", false, 3)]
        public static void CreateVerticalSW() => GameObjectCreator.CreateObject("Scroll View (V)");
    }
}