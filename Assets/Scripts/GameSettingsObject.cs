using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Installers/GameSettings")]
public class GameSettingsObject : ScriptableObjectInstaller<GameSettingsObject>
{
    public GameSettings gameSettings;
    public override void InstallBindings()
    {
        Container.BindInstances(gameSettings);
    }
}
[Serializable]
public class GameSettings
{
    public GameTime gameTime;
    public int amountOfInteractablesToComplete;
}