using UnityEngine;
using Zenject;

public class PlaceObject : MonoBehaviour
{
    public string objectsGroup;
    [SerializeField] private Transform placePoint;

    public void ObjectPlace(GameObject placeableObject)
    {
        Instantiate(placeableObject, placePoint);
    }

    public void HideObject() => placePoint.gameObject.SetActive(false);
    public void ShowObject() => placePoint.gameObject.SetActive(true);
}
