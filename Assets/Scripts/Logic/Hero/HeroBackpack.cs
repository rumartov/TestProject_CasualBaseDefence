using System.Collections;
using System.Collections.Generic;
using Infrastructure.Factory;
using Logic.Enemy;
using UnityEngine;

namespace Logic.Hero
{
    public class HeroBackpack : MonoBehaviour
    {
        public int Items => _backpackItems.Count;

        [SerializeField] private ZoneCheck zoneCheck;
    
        [SerializeField] private Transform startBackpackPosition;

        [SerializeField] private Transform content;

        private Stack<Dictionary<GameObject, int>> _backpackItems = new();
        private float _dropLootDecay = 0.1f;
        private IGameFactory _factory;

        public void Construct(IGameFactory factory)
        {
            _factory = factory;
        }
        private void Start() => zoneCheck.OnZoneChanged += RemoveAllItemsFromBackpack;
    
        public void AddToBackpack(GameObject item)
        {
            item.transform.SetParent(content);

            Dictionary<GameObject, int> pair = new Dictionary<GameObject, int>
            {
                {item, _backpackItems.Count}
            };
        
            _backpackItems.Push(pair);
        }

        private Dictionary<GameObject, int> RemoveFromBackpack()
        {
            return _backpackItems.Count > 0 ? _backpackItems.Pop() : null;
        }
    
        public void RemoveAllItemsFromBackpack(Zone zone)
        {
            if(zone != Zone.Base)
                return;
        
            StartCoroutine(RemoveItemsCoroutine(Items));
        }

        private IEnumerator RemoveItemsCoroutine(int itemsAmount)
        {
            for (int i = 0; i < itemsAmount; i++)
            {
                Dictionary<GameObject, int> item = RemoveFromBackpack();

                if (item == null)
                    yield break;
            
                Dictionary<GameObject, int>.Enumerator enumerator = item.GetEnumerator();
                enumerator.MoveNext();
                GameObject itemObject = enumerator.Current.Key;

                itemObject.transform.SetParent(null);
            
                StartCoroutine(MoveItemFromBackpack(itemObject, _factory.Hero.transform));
            
                yield return new WaitForSeconds(_dropLootDecay);
            }
        }
    
        private IEnumerator MoveItemFromBackpack(GameObject item, Transform moveTo)
        {
            Vector3 startPosition = item.transform.position;
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                item.transform.position = Vector3
                    .Lerp(startPosition,  moveTo.position, i);
            
                yield return null;
            }
        
            item.transform.position = moveTo.position;

            item.GetComponent<LootPiece>().RegisterPickup();
        
            Destroy(item, 0.1f);
        }

        public Vector3 NextBackpackPosition(int index, Vector3 itemSize)
        {
            float sizeY = itemSize.y;
            Vector3 nextBackpackPosition = startBackpackPosition.position + new Vector3(
                0,
                sizeY * index,
                0);
        
            return nextBackpackPosition;
        }
    }
}