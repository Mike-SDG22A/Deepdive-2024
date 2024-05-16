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
    [SerializeField] TMP_InputField nameField;

    public string name;

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        for(int i  = 0; i < 10; i++)
        if (leaderBoards.Count == 0) { leaderBoards = LoadBoard(); }
    }

    // Update is called once per frame
    void Update()
    {
        currentBoard.value = timer.timer;
    }

    void AddToBoard(Board board)
    {
        bool isBetter = false;

        if (leaderBoards.Count == 0) isBetter = true;
        else
        {
            foreach (var bo in leaderBoards)
            {
                if (board.value < bo.value) { isBetter = true; break; }
            }

        }


        if (isBetter)
        {
            submitArea.SetActive(true);
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

        OrderBoard();
    }

    public void OrderBoard()
    {
        leaderBoards = leaderBoards.OrderBy(x => x.value).ToList();
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
                Debug.LogError($"No JSON file found for key 'Json{i}'.");
            }
        }
        return boards;
    }

}
