using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Detection/FilterInfo")]

public class DetectionFilter : ScriptableObject
{
    public ContactFilter2D DetectionInfo => _detectionInfo;
    [SerializeField] protected ContactFilter2D _detectionInfo = default;
}
