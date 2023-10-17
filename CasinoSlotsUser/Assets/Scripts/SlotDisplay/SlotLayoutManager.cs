using System;
using UnityEngine;

namespace SlotDisplay
{
    /// <summary>
    /// Creates a grid of slots based on the values set in the inspector.
    /// </summary>
    public class SlotLayoutManager : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField, Range(1, 10)] private int reelCount = 3;
        [SerializeField, Range(1, 10)] private int rowCount = 3;
        [SerializeField, Range(0.1f, 25f)] private float reelSpacing = 1f;
        [SerializeField, Range(0.1f, 25f)] private float rowSpacing = 1f;
        public int ReelCount => reelCount;
        public int RowCount => rowCount;
        public float RowSpacing => rowSpacing;

        public GameObject[,] SlotBoard { get; private set; }

#if UNITY_EDITOR
        //function to update when values are changed
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            if (slotPrefab == null)
                return;

            //destroy all children
            foreach (Transform child in transform)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    if (child != null) DestroyImmediate(child.gameObject);
                };
            }


            SlotBoard = new GameObject[reelCount, rowCount + 1];

            //create new children and name them to their index
            for (var i = 0; i < reelCount; i++)
            {
                for (var j = 0; j <= rowCount; j++)
                {
                    var slot = Instantiate(slotPrefab, transform);
                    slot.transform.localPosition = new Vector3(i * reelSpacing, -j*rowSpacing, 0);
                    slot.name = $"{i},{j}";
                    SlotBoard[i, j] = slot;
                }
            }
        }
        #endif

        private void Awake()
        {
            //destroy all children
            foreach (Transform child in transform)
            {
                if (child != null) Destroy(child.gameObject);
            }


            SlotBoard = new GameObject[reelCount, rowCount + 1];

            //create new children and name them to their index
            for (var i = 0; i < reelCount; i++)
            {
                for (var j = 0; j <= rowCount; j++)
                {
                    var slot = Instantiate(slotPrefab, transform);
                    slot.transform.localPosition = new Vector3(i * reelSpacing, -j*rowSpacing, 0);
                    slot.name = $"{i},{j}";
                    SlotBoard[i, j] = slot;
                }
            }
        }
    }
}
