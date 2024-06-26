using UnityEngine;

namespace CGames
{
    public abstract class SettingSlot : MonoBehaviour
    {
        public abstract void PrepareSettingSlot();
        public abstract void MatchValuesToCurrent();
    }
}