using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/VoidEvent")]
public class VoidEvent:ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}