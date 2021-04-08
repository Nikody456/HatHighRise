#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using System.Collections.Generic;
using UnityEngine;

public class UIHealthDisplay : MonoBehaviour
{
    [SerializeField] GameObject _healthIconPrefab = default;

    Queue<GameObject> _healthIcons = new Queue<GameObject>();


    private void Awake()
    {
        if (_healthIconPrefab == null)
        {
            _healthIconPrefab = Resources.Load<GameObject>("UI/HealthIcon");
        }
    }

    public void SetHealth(int amnt)
    {
        int difference = _healthIcons.Count - amnt;

        if (difference == 0)
            return;

        for (int i = 0; i < Mathf.Abs(difference); i++)
        {
            if (difference > 0)
                DecreaseHealth();
            else
                IncreaseHealth();
        }


    }

    public void IncreaseHealth()
    {
        if (_healthIconPrefab)
        {
            var newIcon = Instantiate(_healthIconPrefab, this.transform);
            _healthIcons.Enqueue(newIcon);
        }
    }

    public void DecreaseHealth()
    {
        if (_healthIcons.Count > 0)
        {
            Destroy(_healthIcons.Dequeue());
        }
    }

}
