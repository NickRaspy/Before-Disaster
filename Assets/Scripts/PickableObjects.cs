using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Pickable Objects", menuName = "Installers/Pickable Objects")]
public class PickableObjects : ScriptableObjectInstaller<PickableObjects>
{
    public List<PickableObject> objects;
    public override void InstallBindings()
    {
        Container.BindInstances(objects);
    }
}
[Serializable]
public class PickableObject
{
    public Type type;
    public GameObject gameObject;
    public enum Type
    {
        Standard, Big
    }
}