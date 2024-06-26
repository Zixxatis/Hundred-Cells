using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CGames
{
    /// <summary> This script is used for injection to non-mono classes to use coroutines & some of the Unity methods. </summary>
    public class MonoProxy : MonoBehaviour 
    {
        private List<IEarlyInitializable> earlyInitializableScriptsList;
        private List<IPausable> pausableScriptsList;
        private List<IQuitable> shutdownableScriptsList;

        [Inject]
        private void Construct(List<IEarlyInitializable> earlyInitializableScriptsList, List<IPausable> pausableScriptsList, List<IQuitable> shutdownableScriptsList)
        {
            this.earlyInitializableScriptsList = earlyInitializableScriptsList;
            this.pausableScriptsList = pausableScriptsList;
            this.shutdownableScriptsList = shutdownableScriptsList;
        }

        private void Awake()
        {
            earlyInitializableScriptsList.ForEach(x => x.InitializeEarly());
        }
    
        private void OnApplicationPause(bool pauseStatus)
        {
            if(pauseStatus)
                pausableScriptsList.ForEach(x => x.OnPauseStarted());
            else
                pausableScriptsList.ForEach(x => x.OnPauseFinished());        
        }
    
        private void OnApplicationQuit()
        {
            shutdownableScriptsList.ForEach(x => x.PrepareToQuit());
        }
    }
}