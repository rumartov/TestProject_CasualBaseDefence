using System.Collections;
using Data;
using Infrastructure.Factory;
using Logic.Hero;
using UnityEngine;

namespace Logic.Enemy
{
  public class LootPiece : MonoBehaviour
  {
    [SerializeField] private GameObject pickupFxPrefab;
    
    private WorldData _worldData;
    private HeroBackpack _heroBackpack;
    
    private Loot _loot;

    private const string Player = "Player";

    private string _id;

    private bool _pickedUp;
    private Vector3 _startPosition;

    public void Construct(WorldData worldData, HeroBackpack heroBackpack)
    {
      _heroBackpack = heroBackpack;
      _worldData = worldData;
    }
    
    public void Initialize(Loot loot) => 
      _loot = loot;

    private void Start()
    {
      _id = GetComponent<UniqueId>().Id;

      _startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (!_pickedUp && other.CompareTag(Player))
      {
        _pickedUp = true;
        Pickup();
      }
    }
    
    public void RegisterPickup()
    {
      UpdateWorldData();
    }
    
    private void Pickup()
    {
      PlayPickupFx();
      PickUpToBackpack();
    }

    private void PickUpToBackpack()
    {
      _heroBackpack.AddToBackpack(gameObject);
      Vector3 itemSize = GetComponentInChildren<Renderer>().bounds.size;
      
      StartCoroutine(MoveItemToBackpack(_heroBackpack.Items, itemSize));
    }
    private IEnumerator MoveItemToBackpack(int index, Vector3 itemSize)
    {
      for (float i = 0; i < 1; i += Time.deltaTime)
      {
        transform.position = Vector3
          .Lerp(_startPosition,  _heroBackpack.NextBackpackPosition(index, itemSize), i);
        yield return null;
      }
      transform.position = _heroBackpack.NextBackpackPosition(index, itemSize);
    }
    
    private void UpdateWorldData()
    {
      UpdateCollectedLootAmount();
    }

    private void UpdateCollectedLootAmount() =>
      _worldData.LootData.Collect(_loot);

    private void PlayPickupFx() =>
      Instantiate(pickupFxPrefab, transform.position, Quaternion.identity);
  }
}