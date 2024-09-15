using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public abstract class Interactable : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] InteractableManager interactableManager;

    [Header("Required")]
    public bool isCompletable;
    public bool isImportant;
    public bool playerCanMove;
    [Space]
    public string groupName;
    public string interactableText;
    public GameObject requiredObject;
    public bool objectMustDestroy;
    [Space]
    public GameObject marker;
    public SpriteRenderer spriteOnMarker;
    [Space]
    public UnityEvent onInteractionEnd = new();

    [Header("Optional")]
    [Tooltip("Those objects will appear only during interaction")]
    public List<GameObject> temporaryObjectToAppear = new();

    public Player InteractingPlayer { get; set; }

    public Sprite MarkerSprite
    {
        set
        {
            spriteOnMarker.sprite = value;
        }
    }

    private void Start()
    {
        if (interactableManager.interactableGroups.Any(x => x.groupName == groupName))
            interactableManager.interactableGroups.Find(x => x.groupName == groupName).interactables.Add(this);
        else
        {
            InteractableGroup interactableGroup = new(groupName);
            interactableGroup.interactables.Add(this);
            interactableManager.interactableGroups.Add(interactableGroup);
        }
    }
    private void OnTriggerEnter(Collider other) => InteractableSet(true, other);
    private void OnTriggerExit(Collider other) => InteractableSet(false, other);

    public void InteractableSet(bool isEntered, Collider other)
    {
        if(!enabled) return;
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().Interactable = isEntered ? this : null;
            gameManager.UIController?.ChangeInteractableText(isEntered ? interactableText : "");
        }
    }

    public void Interact()
    {
        if (requiredObject != null && InteractingPlayer.HoldableObject != requiredObject)
        {
            gameManager.UIController?.WarnTextAppear();
            InteractingPlayer = null;
            return;
        }

        if (objectMustDestroy) InteractingPlayer.RemoveObject();

        if (temporaryObjectToAppear.Count > 0) 
        {
            temporaryObjectToAppear.ForEach(o => 
            {
                o.SetActive(true);
                onInteractionEnd.AddListener(() => o.SetActive(false));
            });
        }

        InteractingPlayer.CanMove = playerCanMove;
        onInteractionEnd.AddListener(() => InteractingPlayer.CanMove = true);

        Action();
    }
    public abstract void Action();
}
public interface IInteractable
{
    public void Action();
}