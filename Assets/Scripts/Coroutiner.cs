using UnityEngine;
using Zenject;

public class Coroutiner : MonoBehaviour
{
    [Inject] GameManager gameManager;

    public Coroutine currentCoroutine;

    public void Start()
    {
        gameManager.coroutiner = this;
    }
    public void StopCoroutine() => StopCoroutine(currentCoroutine);
}
