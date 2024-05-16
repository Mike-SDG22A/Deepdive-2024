using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

[Serializable]
public class Board
{
    public string name;
    public float value;

    public Board() { }

    public Board(string name, float value)
    {
        this.name = name;
        this.value = value;
    }
}

public class Leaderboard : MonoBehaviour
{
    public List<Board> leaderBoards = new List<Board>();
    Board currentBoard = new Board();
    Timer timer;
    [SerializeField] GameObject submitArea;
    [SerializeField] GameObject menuButtons;
    [SerializeField] TMP_Text nameField;
    [SerializeField] RankPanel[] rankPanels;

    public string name;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        for(int i  = 0; i < 10; i++)
        if (leaderBoards.Count == 0) { leaderBoards = LoadBoard(); }

        GenerateLeaderBoardUI();

    }

    // Update is called once per frame
    void Update()
    {
        currentBoard.value = timer.timer;
    }

    public void AddToBoard(Board board)
    {
        bool isBetter = false;

        if (leaderBoards.Count < 10) isBetter = true;
        else
        {
            foreach (var bo in leaderBoards)
            {
                if (board.value < bo.value) { isBetter = true; break; }
            }

        }


        if (isBetter)
        {
            menuButtons.SetActive(false);
            submitArea.SetActive(true);
        }


    }

    void GenerateLeaderBoardUI()
    {
        for (int i = 0; i < leaderBoards.Count; i++)
        {
            rankPanels[i].nameText.text = leaderBoards[i].name;

            int min = (int)Mathf.Floor(leaderBoards[i].value / 60);

            float tempTimer = leaderBoards[i].value - (60 * min);

            string extraNumSec = "";
            string extraNumMin = "";
            if (tempTimer < 10) extraNumSec = "0";
            if (min < 10) extraNumMin = "0";

            string timeString = $"{extraNumMin}{min}.{extraNumSec}{tempTimer.ToString("F2")}".Replace('.', ':');

            rankPanels[i].timeText.text = timeString;
        }
    }

    public void Submit()
    {
        currentBoard.name = nameField.text;

        if (leaderBoards.Count < 10)
        {
            leaderBoards.Add(currentBoard);
        }
        else
        {
            leaderBoards.RemoveAt(leaderBoards.Count - 1);
            leaderBoards.Add(currentBoard);
        }

        submitArea.SetActive(false);
        menuButtons.SetActive(true);

        OrderBoard();
    }

    public void OrderBoard()
    {
        leaderBoards = leaderBoards.OrderBy(x => x.value).ToList();
        SaveJson();
        GenerateLeaderBoardUI();
    }



    public void SaveJson()
    {
        // Get the save directory

        for (int i = 0; i < leaderBoards.Count; i++)
        {
            string json = JsonUtility.ToJson(leaderBoards[i], true);
            string filePath = Application.persistentDataPath + $"/LeaderBoard{i}.json";
            print(filePath);
            File.WriteAllText(filePath, json);
            PlayerPrefs.SetString("JsonBoard" + i, filePath);
        }
    }

    List<Board> LoadBoard()
    {
        List<Board> boards = new List<Board>();
        for (int i = 0; i < 10; i++)
        {
            string filePath = PlayerPrefs.GetString("JsonBoard" + i);
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    Board board = JsonUtility.FromJson<Board>(json);
                    boards.Add(board);
                }
                catch (ArgumentException ex)
                {
                    Debug.LogError($"Failed to parse JSON file '{filePath}': {ex.Message}");
                }
            }
            else
            {
                Debug.LogError($"No JSON file found for key 'JsonBoard{i}'.");
            }
        }
        return boards;
    }

}
