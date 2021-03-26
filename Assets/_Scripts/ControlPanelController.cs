using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    public RectTransform rectTransform;

    public Vector2 offScreenPosition;
    public Vector2 onScreenPosition;

    [Range(0.1f, 10.0f)] 
    public float speed = 1.0f;
    public float timer = 0.0f;
    public bool isOnScreen = false;

    [Header("Player Settings")] 
    public PlayerBehaviour player;
    public CameraController playerCamera;

    public Pauseable pausable;

    [Header("Scene Data")] 
    public SceneDataSO sceneData;

    public GameObject gameStateElement;

    void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        pausable = FindObjectOfType<Pauseable>();
        player = FindObjectOfType<PlayerBehaviour>();
        playerCamera = FindObjectOfType<CameraController>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = offScreenPosition;
        timer = 0.0f;

        //var sceneDataJSONString = PlayerPrefs.GetString("playerData");
        //JsonUtility.FromJsonOverwrite(sceneDataJSONString, sceneData);
        //Deserializing and loading data from player preferences
        //sceneData = JsonUtility.FromJson<SceneDataSO>(sceneDataJSONString);


        LoadFromPlayerPrefs();


    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    ToggleControlPanel();
        //}

        if (isOnScreen)
        {
            MoveControlPanelDown();
        }
        else
        {
            MoveControlPanelUp();
        }

        gameStateElement.SetActive(pausable.isGamePaused);

        
    }

    void ToggleControlPanel()
    {
        isOnScreen = !isOnScreen;
        timer = 0.0f;

        if (isOnScreen)
        {
            //Cursor.lockState = CursorLockMode.None;
            playerCamera.enabled = false;
        }
        else
        {

            //Cursor.lockState = CursorLockMode.Locked;
            playerCamera.enabled = true;
        }
    }

    private void MoveControlPanelDown()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(offScreenPosition, onScreenPosition, timer);
        if (timer < 1.0f)
        {
            timer += Time.deltaTime * speed;
        }
    }

    private void MoveControlPanelUp()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(onScreenPosition, offScreenPosition, timer);
        if (timer < 1.0f)
        {
            timer += Time.deltaTime * speed;
        }

        if (pausable.isGamePaused)
        {
           pausable.TogglePause();
        }
    }

    public void OnControlButtonPressed()
    {
        ToggleControlPanel();
    }

    public void OnLoadButtonPressed()
    {
        LoadFromPlayerPrefs();
        player.controller.enabled = false;
        player.transform.position = sceneData.playerPosition;
        player.transform.rotation = sceneData.playerRotation;
        player.controller.enabled = true;

        player.health = sceneData.playerHealth;
        player.healthBar.SetHealth(sceneData.playerHealth);
    }

    public void OnSaveButtonPressed()
    {
        sceneData.playerPosition = player.transform.position;
        sceneData.playerHealth = player.health;
        sceneData.playerRotation = player.transform.rotation;

        //serialize and save our data to Player prefences to localdb/dictionary
        //PlayerPrefs.SetString("playerData", JsonUtility.ToJson(sceneData));
        SavetoPlayerPrefs();
    }

    public void SavetoPlayerPrefs()
    {
        PlayerPrefs.SetFloat("playerTransformX", sceneData.playerPosition.x);
        PlayerPrefs.SetFloat("playerTransformY", sceneData.playerPosition.y);
        PlayerPrefs.SetFloat("playerTransformZ", sceneData.playerPosition.z);

        PlayerPrefs.SetFloat("playerRotatationZ", sceneData.playerRotation.z);
        PlayerPrefs.SetFloat("playerRotatationY", sceneData.playerRotation.y);
        PlayerPrefs.SetFloat("playerRotatationX", sceneData.playerRotation.x);
        PlayerPrefs.SetFloat("playerRotatationW", sceneData.playerRotation.w);

        PlayerPrefs.SetInt("playerHealth", sceneData.playerHealth);
    }

    public void LoadFromPlayerPrefs()
    {
        sceneData.playerPosition.x = PlayerPrefs.GetFloat("playerTransformX");
        sceneData.playerPosition.y = PlayerPrefs.GetFloat("playerTransformY");
        sceneData.playerPosition.z = PlayerPrefs.GetFloat("playerTransformZ");

        sceneData.playerRotation.z = PlayerPrefs.GetFloat("playerRotatationZ");
        sceneData.playerRotation.y = PlayerPrefs.GetFloat("playerRotatationY");
        sceneData.playerRotation.x = PlayerPrefs.GetFloat("playerRotatationX");
        sceneData.playerRotation.w = PlayerPrefs.GetFloat("playerRotatationW");

        sceneData.playerHealth = PlayerPrefs.GetInt("playerHealth");

    }
}
