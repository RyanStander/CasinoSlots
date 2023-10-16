using SlotDisplay;
using UnityEngine;

namespace SlotFunctionality
{
    /// <summary>
    /// Manages the game logic of the slot machine.
    /// </summary>
    public class SlotGameManager : MonoBehaviour
    {
        [SerializeField] private SlotLayoutManager slotLayoutManager;
        [SerializeField] private GameObject[] slotSymbols;
        [SerializeField] private GameObject slots;
        [SerializeField] private Vector3 symbolOffset = new(0, 0, -1);

        private GameObject[,] symbols;

        private void OnValidate()
        {
            if (slotLayoutManager == null)
                slotLayoutManager = FindObjectsByType<SlotLayoutManager>(FindObjectsSortMode.None)[0];

            if (slots == null)
                slots = GameObject.Find("Slots");
        }

        private void Start()
        {
            symbols = new GameObject[slotLayoutManager.ReelCount, slotLayoutManager.RowCount];
            SpinTheWheel();
        }

        public void SpinTheWheel()
        {
            for (var i = 0; i < slotLayoutManager.ReelCount; i++)
            {
                for (var j = 0; j < slotLayoutManager.RowCount; j++)
                {
                    var gridPosition =  slotLayoutManager.SlotBoard[i, j];
                    
                    foreach (Transform child in gridPosition.transform)
                    {
                        Destroy(child.gameObject);
                    }

                    var pieceType = slotSymbols[Random.Range(0, slotSymbols.Length)];
                    var thisPiece = Instantiate(pieceType, gridPosition.transform.position + symbolOffset, Quaternion.identity);
                    thisPiece.name=pieceType.name;
                    thisPiece.transform.parent = gridPosition.transform;
                    symbols[i, j] = thisPiece;
                }
            }
        }
    }
}
