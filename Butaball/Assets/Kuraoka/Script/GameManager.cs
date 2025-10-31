using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //シングルトン化
    public static GameManager Instance { get; private set; }


    public enum State
    {
        Start,
        GameStartMessage,
        Ingame,
        End
    }

    [Header("UI")]
    [SerializeField] private Image countdownImage;

    [Header("Sprites")]
    [SerializeField] private Sprite sprite3;
    [SerializeField] private Sprite sprite2;
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite spriteGo;

    [Header("Sizes (幅, 高さ)")]
    [SerializeField] private Vector2 size3 = new Vector2(100, 100);
    [SerializeField] private Vector2 size2 = new Vector2(100, 100);
    [SerializeField] private Vector2 size1 = new Vector2(100, 100);
    [SerializeField] private Vector2 sizeGo = new Vector2(558, 88);

    [Header("Timing")]
    [SerializeField] private float countdownTime = 3f;
    [SerializeField] private float gameStartDisplayDuration = 1.5f;

    private float countdown;
    private float gameStartTimer;
    private bool gameStarted = false;

    [SerializeField] private MainGameEnd mainGameEnd;

    public State state;

    void Start()
    {
        Application.targetFrameRate = 60;

        if (countdownImage == null)
        {
            Debug.LogError("countdownImage が Inspector にアサインされていません！");
            return;
        }

        countdown = countdownTime;
        state = State.Start;
        countdownImage.gameObject.SetActive(true);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //重複防止
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        //シーンをまたいでも保持
        //DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        EndGame();
        Debug.Log("ok");
        switch (state)
        {
            case State.Start:
                HandleCountdown();
                break;

            case State.GameStartMessage:
                HandleGameStartMessage();
                break;

            case State.Ingame:
                // ゲーム中の処理
                break;

            case State.End:
                break;
        }
    }

    void HandleCountdown()
    {
        countdown -= Time.deltaTime;

        // カウントごとにスプライトとサイズを切り替え
        if (countdown > 2f)
        {
            SetCountdownSprite(sprite3, size3);
        }
        else if (countdown > 1f)
        {
            SetCountdownSprite(sprite2, size2);
        }
        else if (countdown > 0f)
        {
            SetCountdownSprite(sprite1, size1);
        }
        else
        {
            SetCountdownSprite(spriteGo, sizeGo);
            gameStartTimer = gameStartDisplayDuration;
            state = State.GameStartMessage;
            Debug.Log("Go!!");
        }
    }

    void HandleGameStartMessage()
    {
        gameStartTimer -= Time.deltaTime;

        if (gameStartTimer <= 0 && !gameStarted)
        {
            gameStarted = true;
            countdownImage.gameObject.SetActive(false);
            state = State.Ingame;
            Debug.Log(" Game Start!");
            mainGameEnd.ResetGameTime();
        }
    }

    // スプライト切り替え + サイズ調整
    void SetCountdownSprite(Sprite sprite, Vector2 size)
    {
        countdownImage.sprite = sprite;
        countdownImage.rectTransform.sizeDelta = size;
    }

    private void EndGame()
    {
        //Escが押された時
        if (Input.GetKey(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }

    }



    public State GetGameState() => state;
}