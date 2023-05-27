using Data;
using Infrastructure.Factory;
using Services.Randomizer;
using UnityEngine;

namespace Logic.Enemy
{
  public class LootSpawner : MonoBehaviour
  {
    public EnemyDeath EnemyDeath;
    
    private IGameFactory _factory;
    private IRandomService _randomizer;
    
    private int _lootValue;

    public void Construct(IGameFactory factory, IRandomService randomService)
    {
      _factory = factory;
      _randomizer = randomService;
    }
    
    private void Start()
    {
      EnemyDeath.Happened += SpawnLoot;
    }

    public void SetLootValue(int value)
    {
      _lootValue = value;
    }

    private void SpawnLoot()
    {
      EnemyDeath.Happened -= SpawnLoot;

      if (ShouldSpawn())
      {
        LootPiece lootPiece = _factory.CreateLoot();
        lootPiece.transform.position = transform.position + Vector3.up;
        lootPiece.GetComponent<UniqueId>().GenerateId();

        Loot loot = GenerateLoot();
      
        lootPiece.Initialize(loot); 
      }
    }

    private bool ShouldSpawn()
    {
      return _randomizer.Next(0, 2) == 0;
    }

    private Loot GenerateLoot()
    {
      Loot loot = new Loot()
      {
        Value = _lootValue
      };
      return loot;
    }
  }
}