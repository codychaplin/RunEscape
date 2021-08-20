using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 2f;
    public Transform interactionTransform;

    Transform player;
    bool isFocus = false;
    bool hasInteracted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFocus && !hasInteracted) // if focused but has not interacted yet
        {
            // get distance to focused object
            float distance = Vector3.Distance(player.position, interactionTransform.position);

            if (distance <= radius) // if within range
            {
                Interact(); // base interact function
                hasInteracted = true;
            }
        }
    }

    // debuging purposes
    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    public virtual void Interact()
    {
        
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform; // gets player position
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null; // removes player transform from player
        hasInteracted = false;
    }
}
