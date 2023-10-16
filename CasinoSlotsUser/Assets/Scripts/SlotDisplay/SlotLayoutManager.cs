using UnityEngine;

namespace SlotDisplay
{
    /// <summary>
    /// Creates a grid of slots based on the values set in the inspector.
    /// </summary>
    public class SlotLayoutManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] slotRows;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField, Range(1, 10)] private int reelCount = 3;
        [SerializeField, Range(1, 10)] private int rowCount = 3;
        [SerializeField, Range(0.1f, 25f)] private float reelSpacing = 1f;
        [SerializeField, Range(0.1f, 25f)] private float rowSpacing = 1f;
        public int ReelCount => reelCount;
        public int RowCount => rowCount;
        public float RowSpacing => rowSpacing;
        public GameObject[] SlotRows => slotRows;

        public GameObject[,] SlotBoard { get; private set; }

        //function to update when values are changed
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            if (slotPrefab == null)
                return;

            if (slotRows.Length != rowCount+1)
            {
                Debug.LogError("Slot rows length needs to be equal to the row count + 1");
                return;
            }

            //destroy all children
            foreach (var slotRow in slotRows)
            {
                foreach (Transform child in slotRow.transform)
                {
                    UnityEditor.EditorApplication.delayCall += () => { if(child!=null) DestroyImmediate(child.gameObject); };
                }
            }


            SlotBoard = new GameObject[reelCount, rowCount+1];

            //create new children and name them to their index
            for (var i = 0; i < reelCount; i++)
            {
                for (var j = 0; j <= rowCount; j++)
                {
                    var slot = Instantiate(slotPrefab, slotRows[j].transform);
                    slot.transform.localPosition = new Vector3(i * reelSpacing, 0, 0);
                    slot.name = $"{i},{j}";
                    SlotBoard[i, j] = slot;
                }
            }
        }
    }
}
