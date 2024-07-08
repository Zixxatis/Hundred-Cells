using System;
using UnityEngine;

namespace CGames
{
    /// <summary> Use this attribute with the "SerializeFieldAttribute" to limit the changes that can be made in the Inspector window. </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class InspectorReadOnlyAttribute : PropertyAttribute
    {
        
    }
}