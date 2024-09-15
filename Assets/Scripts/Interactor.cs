using UnityEngine;
using UnityEngine.Events;

public class Interactor : MonoBehaviour
{
    public UnityEvent<bool> OnInteractableAppeared { get; set; }
    public Interactable Interactable { get; set; }
    private void Start()
    {
        OnInteractableAppeared = new();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Interactable>() != null)
        {
            Interactable = other.GetComponent<Interactable>();
            OnInteractableAppeared.Invoke(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            Interactable = null;
            OnInteractableAppeared.Invoke(false);
        }
    }
}
