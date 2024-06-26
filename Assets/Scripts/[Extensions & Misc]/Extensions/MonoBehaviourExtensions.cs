using System;
using System.Collections;
using UnityEngine;

namespace CGames
{
    public static class MonoBehaviourExtensions
    {
        /// <summary> Starts local coroutine, waits given time and invokes action. </summary>
        public static void InvokeAfterDelay(this MonoBehaviour monoBehaviour, float delayInSeconds, Action action)
        {
            monoBehaviour.StartCoroutine(WaitAndInvoke());

            IEnumerator WaitAndInvoke()
            {
                yield return new WaitForSeconds(delayInSeconds);
                action?.Invoke();
            }
        }
    }
}