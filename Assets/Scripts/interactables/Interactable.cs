using UnityEngine; 

// The Interactable class is an abstract class, meaning it is intended to be inherited by other classes
// that define how interaction works for different types of objects (e.g., doors, NPCs, items).
public abstract class Interactable : MonoBehaviour
{
    // A flag to determine if this object uses UnityEvents for interaction
    public bool useEvents;

    // A prompt message that will be displayed when the player looks at the object
    [SerializeField]
    public string promptMessage;

    // A virtual method that returns the prompt message when the player looks at the object
    // It can be overridden in subclasses to provide specific behavior.
    public virtual string OnLook()
    {
        return promptMessage; // Return the promptMessage to be shown to the player
    }

    // A method that handles the interaction with the object.
    // This calls the UnityEvent OnInteract (if enabled) and then calls the Interact method.
    public void BaseInteract()
    {
        if (useEvents) // Check if the object should use UnityEvents
        {
            // If using events, invoke the OnInteract UnityEvent (which triggers any assigned listeners)
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        // Call the Interact method (defined by subclasses)
        Interact();
    }

    // A virtual method to be overridden by subclasses to define custom interaction behavior.
    // This is a template function, so subclasses need to implement it.
    protected virtual void Interact()
    {
        // Template for interaction logic (can be overridden in derived classes)
    }
}

