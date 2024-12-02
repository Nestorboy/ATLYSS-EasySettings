using System.Collections.Generic;
using UnityEngine;

namespace Nessie.ATLYSS.EasySettings;

/// <summary>
/// Used to keep redirect component references when instantiating a new clone of a GameObject.
/// </summary>
public class ComponentReferences : MonoBehaviour
{
    public List<Component> components = new();
}