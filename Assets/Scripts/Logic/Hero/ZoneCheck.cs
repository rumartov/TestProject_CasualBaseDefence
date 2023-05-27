using System;
using Logic.Animation;
using UnityEngine;

namespace Logic.Hero
{
    public class ZoneCheck : MonoBehaviour
    { 
        public Zone CurrentZone { get; private set; }
        public event Action<Zone> OnZoneChanged;
        
        private void Start() => CurrentZone = Zone.Unknown;

        private void Update()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            Physics.Raycast(ray, out RaycastHit hit, 1);

            if (hit.collider == null)
                return;

            int layerId = hit.collider.gameObject.layer;
            string layerName = LayerMask.LayerToName(layerId);
            
            Zone newZone = ZoneFor(layerName);
            if (newZone == CurrentZone)
                return;
            
            CurrentZone = ZoneFor(layerName);;
            OnZoneChanged?.Invoke(CurrentZone);
        }

        private Zone ZoneFor(string layerName)
        {
            Zone activeZone;
    
            if (layerName == nameof(Zone.Base))
                activeZone = Zone.Base;
            else
                activeZone = Zone.Unknown;
          
            return activeZone;
        }
    }

    public enum Zone
    {
        Base,
        Unknown
    }
}