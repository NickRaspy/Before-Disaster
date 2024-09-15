using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InteractableManager>().AsSingle().NonLazy();
        Container.Bind<GameManager>().AsSingle().NonLazy();
    }
}