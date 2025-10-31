using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class MapGimmick : MonoBehaviour
{

    public static MapGimmick Instance { get; private set; }

    //メテオオブジェクト
    public GameObject minimeteor;
    //ゲームIngameのステート管理
    GameManager gameManager;
    //Ingameの経過時間
    private float gameTime = 0;
    //Meteorを降らせるタイミング
    private float MeteorTime = 30;
    //ボールを降らせるタイミング
    private float BallTime = 10;

    
    public GameObject[] ballPrefabs;
    //ボールを落とす数
    public int SpawnBall = 3;
    //ボールを降らせる高さ
    public float PosY = 10.0f;
    //オブジェクトを生成するXの範囲
    public float AreaX = 1.0f;
    //ボールのフラグ
    public bool ballTrigger = true;
    //メテオフラグ
    public bool meteorTrigger = true;
    //インスペクターからテーブルのコライダーを設定
    public Collider tableCollider;
    //インスペクターからメテオの出現位置を設定
    public Transform[] meteorSpawnPoint;
    //rayの進行方向
    Vector3 direction;
    //ヒットした場所
    RaycastHit hit;
    //rayの開始地点
    Vector3 startPoint;
    //予兆
    public GameObject Danger;
    private GameObject dangerPoint;

    void Awake()
    {
        //gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();

        Instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager.Instanceがnull");
        }
        //シーン内の "MeteorSpawn" タグが付いたオブジェクトを取得
        GameObject[] spawnObjects = GameObject.FindGameObjectsWithTag("MeteorSpawn");
        meteorSpawnPoint = new Transform[spawnObjects.Length];
        for (int i = 0; i < spawnObjects.Length; i++)
        {
            meteorSpawnPoint[i] = spawnObjects[i].transform;
        }



    }

    // Update is called once per frame
    void Update()
    {
        gimmick();
    }

    public void gimmick()
    {
        if (gameManager != null&& gameManager.state ==GameManager.State.Ingame)
        {
            //ゲームの経過時間を計測
            gameTime+=Time.deltaTime;

            if(gameTime > BallTime)
            {
                if(ballTrigger)
                {
                    BallFallgimmick();
                    ballTrigger = false;
                }
            }
            if(gameTime > MeteorTime)
            {
                Meteorgimmick();
                meteorTrigger = false;
            }

        }
    }

    //メテオ
    async public void Meteorgimmick()
    {
        int randomIndex = UnityEngine.Random.Range(0, meteorSpawnPoint.Length);
        Transform spawnpoint = meteorSpawnPoint[randomIndex];

        if (spawnpoint == null)
        {
            Debug.LogError("spawnpointがnullです（破棄された可能性）");
            return;
        }

        // 必要な情報をキャッシュ
        Vector3 spawnPosition = spawnpoint.position;
        Quaternion spawnRotation = spawnpoint.rotation;
        direction = -spawnpoint.up;
        startPoint = spawnPosition;

        Rayhit();
        gameTime = 0;
        ballTrigger = true;

        await UniTask.Delay(TimeSpan.FromSeconds(3));

        Instantiate(minimeteor, spawnPosition, spawnRotation);
    }

    //ボール
    public void BallFallgimmick()
    {
        Debug.Log("ゲーム開始から10秒経過、ボールを降らせます");
        //テーブルの境界情報を取得
        Bounds tableBounds = tableCollider.bounds;

        foreach (var ball in ballPrefabs)
        {
            for (int i = 0; i < SpawnBall; i++)//SpawnBallはボールを出す数
            {
                //ランダムなXの位置
                float randomX = UnityEngine.Random.Range(tableBounds.min.x, tableBounds.max.x);
                //ランダムなZの位置
                float randomZ = UnityEngine.Random.Range(tableBounds.min.z, tableBounds.max.z);
                //生成する位置の決定
                Vector3 spawnposition = new Vector3(randomX, tableBounds.max.y + PosY, randomZ);
                //ボールの生成
                Instantiate(ball, spawnposition, Quaternion.identity);
                //メテオトリガー復活
                meteorTrigger = true;
            }
        }
    }

    private void Rayhit()
    {
        Ray ray = new Ray(startPoint, direction);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            Debug.Log("ray衝突" + hit.collider.name);

            if (hit.collider.CompareTag("map"))
            {
                Debug.Log("rayがmapに衝突" + hit.point);
                Vector3 hitPoint = hit.point + new Vector3(0,0.1f,0);
                dangerPoint=Instantiate(Danger,hitPoint, Quaternion.identity);                
            }
        }
    }
    public void DestroyDanger()
    {
        Destroy(dangerPoint);
    }
}
