using System.Collections.Generic;
using UnityEngine;

public class InteractableManager
{
    public List<InteractableGroup> interactableGroups = new();
    public List<Interactable> mustToCompleteInteractables = new();
}
public class InteractableGroup
{
    public string groupName;
    public List<Interactable> interactables = new();

    public InteractableGroup(string groupName)
    {
        this.groupName = groupName;
    }
}
