using UnityEngine;

namespace SlotDisplay
{
    public class SlotLayoutManager : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField, Range(1, 10)] private int reelCount = 3;
        [SerializeField, Range(1, 10)] private int rowCount = 3;
        [SerializeField, Range(0.1f, 25f)] private float reelSpacing = 1f;
        [SerializeField, Range(0.1f, 25f)] private float rowSpacing = 1f;
        public int ReelCount => reelCount;
        public int RowCount => rowCount;

        //function to update when values are changed
        private void OnValidate()
        {
            if (slotPrefab == null)
                return;

            //destroy all children
            foreach (Transform child in transform)
            {
                UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(child.gameObject); };
            }

            //create new children and name them to their index
            for (var i = 0; i < reelCount; i++)
            {
                for (var j = 0; j < rowCount; j++)
                {
                    var slot = Instantiate(slotPrefab, transform);
                    slot.transform.localPosition = new Vector3(i * reelSpacing, -j * rowSpacing, 0);
                    slot.name = $"{i},{j}";
                }
            }
        }
    }
}
