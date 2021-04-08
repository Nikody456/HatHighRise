#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;

public class CoinCreator : MonoSingleton<CoinCreator>
{

    [SerializeField] GameObject _coinPrefab = default;

    private System.Random _rng ;

    protected override void Awake()
    {
        base.Awake();
        _rng = new System.Random();
        if (_coinPrefab==null)
        {
            _coinPrefab = Resources.Load<GameObject>("Coin");
        }
    }

    public void CreateSomeCoins(int amount, Vector3 nearWorldPos)
    {
       // Debug.Log($"Creating {amount} coins ");
        for (int i = 0; i < amount; i++)
        {
            var newCoin = Instantiate(_coinPrefab, this.transform);
            newCoin.transform.position = GetRandomLocNear(nearWorldPos);
        }
    }

    private Vector3 GetRandomLocNear(Vector3 pos)
    {
        float randX = pos.x + _rng.Next(-5, 5);
        float randY = pos.y + _rng.Next(0, 4);

        return new Vector3(randX, randY, pos.z);
    }
}
