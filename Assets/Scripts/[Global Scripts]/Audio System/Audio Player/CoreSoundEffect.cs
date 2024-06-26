using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class CoreSoundEffect
    {
        [field: SerializeField] public CoreSoundEffectType CoreSoundEffectType { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
    }
}