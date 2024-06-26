using System.Collections.Generic;
using UnityEngine;

namespace CGames
{       
    [CreateAssetMenu(fileName = "Core Theme Colors Config", menuName = "Configs/Core Theme Colors Config", order = 0)]
    public class CoreThemeColorsConfig : ScriptableObject
    {
        [SerializeField] private Color lightThemeCoreColor = new(1f, 1f, 1f, 1f);
        [SerializeField] private Color darkThemeCoreColor = new(0.15f, 0.15f, 0.15f, 1f);

        public Dictionary<bool, Color> GetThemeColorsDictionary()
        {
            return new()
            {
                { true, lightThemeCoreColor },
                { false, darkThemeCoreColor }
            };
        }
    }
}