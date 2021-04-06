#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Hat Score")]
public class HatScore : ScriptableObject
{
    List<HatData> _hatsBetweenlevels = new List<HatData>();

    public void SaveHat(HatData hat)
    {
        _hatsBetweenlevels.Add(hat);
    }
    public void RemoveHat(HatData hat)
    {
        if (_hatsBetweenlevels.Contains(hat))
        {
            _hatsBetweenlevels.Remove(hat);
        }
    }
    public void Clear()
    {
        _hatsBetweenlevels.Clear();
    }

    public HatData[] GetHats()
    {
        return _hatsBetweenlevels.ToArray();
    }

}

