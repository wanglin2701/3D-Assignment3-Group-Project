using UnityEditor;

[CustomEditor(typeof(Interactable), true)] // 'true' ensures this custom editor affects the target class and all its child classes (subclasses).

public class InteractableEditor : Editor
{
    // This method overrides the default OnInspectorGUI method and customizes the Inspector for the 'Interactable' class.
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target; // Cast the 'target' (which is the selected object in the Inspector) to an 'Interactable' type.

        // Check if the target is of type 'EventOnlyInteractable'.
        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            // Show a text field in the Inspector for modifying the 'promptMessage' property of the Interactable object.
            interactable.promptMessage = EditorGUILayout.TextField("Prompt Message", interactable.promptMessage);
            
            // Display an information box explaining that 'EventOnlyInteractable' can only use UnityEvents.
            EditorGUILayout.HelpBox("EventOnlyInteract can ONLY use UnityEvents.", MessageType.Info);

            // If the target doesn't already have an 'InteractionEvent' component attached, add one automatically.
            if (interactable.GetComponent<InteractionEvent>() == null)
            {
                interactable.useEvents = true; // Ensure that 'useEvents' is set to true.
                interactable.gameObject.AddComponent<InteractionEvent>(); // Add the 'InteractionEvent' component to the game object.
            }
        }
        else
        {
            // If the target is not of type 'EventOnlyInteractable', call the base class's OnInspectorGUI to render the default inspector for the 'Interactable' class.
            base.OnInspectorGUI();

            // Check if the 'useEvents' property is true.
            if (interactable.useEvents)
            {   
                // If the target uses events, ensure the 'InteractionEvent' component is attached to the object.
                if (interactable.GetComponent<InteractionEvent>() == null)
                    interactable.gameObject.AddComponent<InteractionEvent>(); // Add the component if it's missing.
            }
            else
            {
                // If the target does not use events, check if the 'InteractionEvent' component exists and remove it if present.
                if (interactable.GetComponent<InteractionEvent>() != null)
                    DestroyImmediate(interactable.GetComponent<InteractionEvent>()); // Remove the 'InteractionEvent' component immediately.
            }
        }
    }
}
