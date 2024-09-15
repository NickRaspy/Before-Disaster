using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Spawnable Objects", menuName = "Scriptable Objects/Spawnable Objects")]
public class SpawnableObjects : ScriptableObjectInstaller<SpawnableObjects>
{
    public List<SpawnableObject> objects;
    public override void InstallBindings()
    {
        Container.BindInstances(objects);
    }
}
[Serializable]
public class SpawnableObject
{
    public string groupName;
    public List<RequiredWithMainObject> objects;

    [Serializable]
    public class RequiredWithMainObject
    {
        public GameObject required;
        public GameObject main;
        public Sprite markerSprite;
    }
}
