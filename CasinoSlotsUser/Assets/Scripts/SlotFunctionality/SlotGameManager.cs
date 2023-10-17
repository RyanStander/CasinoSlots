using System;
using System.Collections.Generic;
using SlotDisplay;
using UnityEngine;
using Random = UnityEngine.Random;

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

        [Header("Spin Settings")] [SerializeField, Range(-2f, -0.01f)]
        private float startingSpinSpeed = -0.1f;

        [SerializeField, Tooltip("The time in seconds before the first reel stops")]
        private float firstReelStopTime = 2f;

        [SerializeField, Tooltip("The time between each reel stopping in seconds")]
        private float reelStopDelay = 1f;

        [SerializeField] private AudioSource spinAudioSource;
        [SerializeField] private AudioClip clickNoise;

        [Header("Line Settings"),SerializeField] Material lineMaterial;
        private readonly Vector3 symbolOffset = new(0, 0, -1);
        private GameObject[,] symbols;

        private float topRowYPos;
        private float bottomRowYPos;
        private float reelSpinTimestamp;
        private int reelStopCount;
        private bool isSpinning;

        private MatchMaker matchMaker;

        private void OnValidate()
        {
            if (slotLayoutManager == null)
                slotLayoutManager = FindObjectsByType<SlotLayoutManager>(FindObjectsSortMode.None)[0];

            if (slots == null)
                slots = GameObject.Find("Slots");
        }

        private void Awake()
        {
            matchMaker = new MatchMaker(lineMaterial, symbolOffset);
        }

        private void Start()
        {
            symbols = new GameObject[slotLayoutManager.ReelCount, slotLayoutManager.RowCount];

            topRowYPos = slotLayoutManager.SlotBoard[0,0].transform.localPosition.y+ slotLayoutManager.RowSpacing;
            bottomRowYPos = slotLayoutManager.SlotBoard[slotLayoutManager.ReelCount-1,slotLayoutManager.RowCount].transform.localPosition.y ;

            spinAudioSource.loop = true;
        }

        private void Update()
        {
            WheelSpinning();
        }

        private void WheelSpinning()
        {
            if (!isSpinning)
                return;

            var toleranceValue = Math.Abs(slotLayoutManager.SlotBoard[reelStopCount+1, 0].transform.localPosition.y -
                                          (topRowYPos - slotLayoutManager.RowSpacing));

            if (Time.time >= reelSpinTimestamp&& toleranceValue < 0.01f)
            {
                reelStopCount++;
                spinAudioSource.PlayOneShot(clickNoise);

                if (reelStopCount == slotLayoutManager.ReelCount-1)
                {
                    isSpinning = false;
                    spinAudioSource.Stop();
                    matchMaker.CheckForMatches(symbols, slotLayoutManager);
                    return;
                }

                reelSpinTimestamp = Time.time + reelStopDelay;
            }

            //take the slots and move them down on the y axis
            for (var i = 0; i < slotLayoutManager.ReelCount; i++)
            {
                var specifiedToleranceValue = Math.Abs(slotLayoutManager.SlotBoard[i, 0].transform.localPosition.y -
                                                       (topRowYPos - slotLayoutManager.RowSpacing));
                if (reelStopCount >= i && specifiedToleranceValue < 0.01f)
                    continue;

                for(var j = 0; j <= slotLayoutManager.RowCount; j++)
                {
                    var slot = slotLayoutManager.SlotBoard[i, j];
                    if (slot.transform.localPosition.y <= bottomRowYPos)
                    {
                        slot.transform.localPosition = new Vector3(slot.transform.localPosition.x, topRowYPos, slot.transform.localPosition.z);
                    }
                    slot.transform.localPosition += new Vector3(0, startingSpinSpeed, 0);
                }
            }
        }

        public void StartSpin()
        {
            matchMaker.ClearLines();

            reelSpinTimestamp = Time.time + firstReelStopTime;
            reelStopCount = -1;
            isSpinning = true;
            spinAudioSource.Play();

            for (var i = 0; i < slotLayoutManager.ReelCount; i++)
            {
                for (var j = 0; j < slotLayoutManager.RowCount; j++)
                {
                    var gridPosition = slotLayoutManager.SlotBoard[i, j];

                    foreach (Transform child in slotLayoutManager.SlotBoard[i,j].transform)
                    {
                        Destroy(child.gameObject);
                    }

                    var pieceType = slotSymbols[Random.Range(0, slotSymbols.Length)];
                    var thisPiece = Instantiate(pieceType, gridPosition.transform.position + symbolOffset,
                        Quaternion.identity);
                    thisPiece.name = pieceType.name;
                    thisPiece.transform.parent = gridPosition.transform;
                    symbols[i, j] = thisPiece;
                }

                //for the extra row of symbols, we dont need to keep track of what they are
                var pieceType2 = slotSymbols[Random.Range(0, slotSymbols.Length)];
                var thisPiece2 = Instantiate(pieceType2, slotLayoutManager.SlotBoard[i, slotLayoutManager.RowCount].transform.position + symbolOffset,
                    Quaternion.identity);
                thisPiece2.name = pieceType2.name;
                thisPiece2.transform.parent = slotLayoutManager.SlotBoard[i, slotLayoutManager.RowCount].transform;
            }
        }
    }
}
