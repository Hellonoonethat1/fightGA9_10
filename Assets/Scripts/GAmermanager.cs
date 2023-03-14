using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GAmermanager : MonoBehaviour
{
    [Header("Lists")]
    public Color[] player_colors;
    public List<playercon> players_list = new List<playercon>();
    public Transform[] spawn_points;

    [Header("prefab refs")]
    public GameObject deathEfactprefab;
    public GameObject playerContPrefab;

    public static GAmermanager instance;

    [Header("Components")]
    private AudioSource audio;
    public AudioClip[] gamefx;
    public Transform containerGroup;
    public TextMeshProUGUI timetext;

    [Header("Level vars")]
    public float startTime;
    public float curTime;
    List<playercon> winningplayers;
    public bool canJoin;
    public bool test;

    




    private void Awake()
    {
        canJoin = true; 
        instance = this;
        audio = GetComponent<AudioSource>();
    
        startTime = PlayerPrefs.GetFloat("roundTime", 100);
        winningplayers = new List<playercon>();
    }
    // Start is called before the first frame update
    void Start()
    {
        curTime = startTime;
        timetext.text = ((int)curTime).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (curTime <= 0)
        {
            int highscore = 0;
            int index = 0;


            foreach (playercon player in players_list)
            {

                if (player.score > highscore)
                {
                    winningplayers.Clear();
                    highscore = player.score;
                    index = players_list.IndexOf(player);
                    winningplayers.Add(player);
                }
                else if (player.score == highscore)
                {
                    winningplayers.Add(player);
                }
                if (winningplayers.Count > 1)
                {
                    // this is a tie
                    //play a sound to incate overtime
                  foreach(playercon Player in players_list)
                    {
                        if (!winningplayers.Contains(player))
                        {
                            player.drop_out();
                        }
                    }
                }
                else
                {
                    PlayerPrefs.SetInt("colorIndex", index);
                    SceneManager.LoadScene("winscreen");
                }
            }
        }
    }
        void FixedUpdate()
        {
            curTime -= Time.deltaTime;
            timetext.text = ((int)curTime).ToString();
        }


    public void onPlayerJoined(PlayerInput player)
    {
        if (canJoin)
        {
            //set player color when joining
            audio.PlayOneShot(gamefx[0]);
            player.GetComponent<playercon>().setColor(player_colors[players_list.Count]);

            //create a ui container
            playercontainer cont = Instantiate(playerContPrefab, containerGroup).GetComponent<playercontainer>();
            player.GetComponent<playercon>().setUI(cont);
            cont.initialize(player_colors[players_list.Count]);


            players_list.Add(player.GetComponent<playercon>());
            player.transform.position = spawn_points[Random.Range(0, spawn_points.Length)].position;
        }
    }
























  }

