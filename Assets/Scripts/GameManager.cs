using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameManager
{
    [Inject] InteractableManager interactableManager;
    [Inject] GameSettings gameSettings;
    [Inject] List<SpawnableObject> spawnableObjects;
    [Inject] List<PickableObject> pickableObjects;

    public List<Player> players = new();

    public UIController UIController;

    public GameTimer gameTimer;

    public Coroutiner coroutiner;

    public bool isGameGoing = false;

    public void GameStart()
    {
        isGameGoing = true;
        UIController.BlockEnable();
        players.ForEach(p => p.CanMove = true);
        gameTimer.Init(gameSettings.gameTime);
        gameTimer.OnTimeEnded += GameStop;
        coroutiner.currentCoroutine = coroutiner.StartCoroutine(gameTimer.StartTimer());
    }
    public void GameStop()
    {
        interactableManager.interactableGroups.ForEach(group => 
        {
            group.interactables.ForEach(interactable => 
            { 
                if (interactable.GetComponent<UIInteractable>() != null) interactable.GetComponent<UIInteractable>().DisableElement(); 
            });
        });
        isGameGoing = false;
        UIController.ShowFinishScreen();
        float percentage = (float)(gameSettings.amountOfInteractablesToComplete - interactableManager.mustToCompleteInteractables.Count) / gameSettings.amountOfInteractablesToComplete;
        Debug.Log(percentage);
        UIController.SelectFinishText(percentage);
        players.ForEach(p => p.CanMove = false);
        coroutiner.StopCoroutine();
    }
    public void LevelPrepare()
    {
        if (IsTooMuchAmountToComplete())
        {
            Debug.LogError("Very big amount of interactables to complete. Please lower the amount!");
            return;
        }

        List<Interactable> completableInteractables = new();
        List<Interactable> selectedInteractables = new();
        foreach (InteractableGroup interactableGroup in interactableManager.interactableGroups) 
        {
            interactableGroup.interactables.ForEach(interactable => 
            {
                if (interactable.GetComponent<PlaceObject>() != null) 
                {
                    int r = Random.Range(0, spawnableObjects.Find(x => x.groupName == interactable.GetComponent<PlaceObject>().objectsGroup).objects.Count);
                    GameObject placeableObject = spawnableObjects.Find(x => x.groupName == interactable.GetComponent<PlaceObject>().objectsGroup).objects[r].main;

                    interactable.GetComponent<PlaceObject>().ObjectPlace(placeableObject);

                    interactable.requiredObject = spawnableObjects.Find(x => x.groupName == interactable.GetComponent<PlaceObject>().objectsGroup).objects[r].required;

                    interactable.MarkerSprite = spawnableObjects.Find(x => x.groupName == interactable.GetComponent<PlaceObject>().objectsGroup).objects[r].markerSprite;
                }
                
                if (interactable.isCompletable) completableInteractables.Add(interactable);
                if (!interactable.isImportant) interactable.enabled = false;
            });
        }

        for (int i = 0; i < gameSettings.amountOfInteractablesToComplete; i++)
        {
            int r = Random.Range(0, completableInteractables.Count);
            selectedInteractables.Add(completableInteractables[r]);
            completableInteractables.RemoveAt(r);
        }

        foreach (Interactable interactable in selectedInteractables) 
        {
            interactable.enabled = true;

            if(interactable.marker != null) interactable.marker.SetActive(true);

            if(interactable.GetComponent<PlaceObject>() != null)
            {
                interactable.GetComponent<PlaceObject>().HideObject();
                interactable.onInteractionEnd.AddListener(() => 
                {
                    interactable.GetComponent<PlaceObject>().ShowObject();
                });
            }

            interactable.onInteractionEnd.AddListener(() => 
            {
                interactable.InteractingPlayer.CanMove = true;
                interactable.enabled = false;
                if (interactable.marker != null) interactable.marker.SetActive(false);
                UIController.ChangeInteractableText("");
                interactableManager.mustToCompleteInteractables.Remove(interactable);
                UIController.ChangeInteractionLeftText
                (
                    $"{gameSettings.amountOfInteractablesToComplete - interactableManager.mustToCompleteInteractables.Count}/{gameSettings.amountOfInteractablesToComplete}"    
                );
            });
        }

        interactableManager.mustToCompleteInteractables.AddRange(selectedInteractables);

        UIController.ChangeInteractionLeftText
        (
            $"0/{gameSettings.amountOfInteractablesToComplete}"
        );
    }
    private bool IsTooMuchAmountToComplete()
    {
        int count = 0;
        foreach (InteractableGroup interactableGroup in interactableManager.interactableGroups) count += interactableGroup.interactables.Count(x => x.isCompletable);
        if (gameSettings.amountOfInteractablesToComplete < count)
        {
            return false;
        }
        else return true;
    }
}
