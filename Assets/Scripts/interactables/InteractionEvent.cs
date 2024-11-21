using UnityEngine; 
using UnityEngine.Events; 

// This class is attached to GameObjects that require event-based interaction functionality.
public class InteractionEvent : MonoBehaviour
{
    // A public UnityEvent that can be configured in the Unity Inspector.
    // This event will be triggered when the associated interaction occurs.
    public UnityEvent OnInteract; // UnityEvent allows us to hook up different methods to be called when the event is invoked.

}

