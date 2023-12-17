using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole
{
    public class HoleSpawnVisualizer : MonoBehaviour
    {
        [SerializeField] private int _itemCount;
        [SerializeField] private float _areaRadius;
        [SerializeField] private float _itemRadius;
        [SerializeField] private int _iterationCount;

        private List<Vector3> _spawnPoints;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, _areaRadius);

            if (_spawnPoints != null)
            {
                foreach (var point in _spawnPoints)
                {
                    UnityEditor.Handles.DrawWireDisc(point, transform.up, _itemRadius);
                }
            }
        }

        [Header("Debug")]
        [SerializeField] private bool _generateSpawnPoints = false;
        private void OnValidate()
        {
            if (_generateSpawnPoints) _spawnPoints = LevelManager.GenerateSpawnPoints(_itemCount, _areaRadius, _itemRadius, _iterationCount);

            _generateSpawnPoints = false;
        }
#endif
    }
}