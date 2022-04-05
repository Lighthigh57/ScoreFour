using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject Pole;
    [SerializeField] private Transform board;
    [SerializeField] private Material[] materials;

    private InterstitialAdExample interstitialAdExample;

    /// <summary>
    /// �Տ�̏����i�[(���āA���s���A�悱)
    /// </summary>
    private readonly int[,,] field = new int[4, 4, 4];
    /// <summary>
    /// �_�ɉ������Ă��邩(���s���A�悱)
    /// </summary>
    private readonly int[,] onStick = new int[4, 4];
    /// <summary>
    /// ����1 ����-1
    /// </summary>
    private int turn = 1;
    private bool nowPlaying = true;
    private const int distance = 10;
    private const int duration = 3;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AdsInitializer>().InitializeAds();
        interstitialAdExample = GetComponent<InterstitialAdExample>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector3 vector = new(1.5f - i, 1.5f, -1.5f + j);
                Instantiate(Pole, vector, Quaternion.identity).transform.SetParent(board);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && nowPlaying)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("SelectPoint"))
                {
                    Vector3 vector = hit.transform.position;
                    int x = (int)(vector.x + 1.5f);
                    int z = (int)Mathf.Abs(vector.z - 1.5f);
                    if (onStick[z, x] < 4)
                    {
                        vector.y = 8;

                        GameObject game = Instantiate(sphere, vector, Quaternion.identity);
                        game.GetComponent<Renderer>().material = materials[Mathf.Abs(turn - 1) / 2];

                        field[onStick[z, x], z, x] = turn;
                        
                        if (Jadge(onStick[z, x]++, z, x))
                        {
                            Debug.Log("GameSet!");
                            StartCoroutine(GameSet());
                            nowPlaying = false;
                        }
                        turn *= -1;
                    }
                }

            }
        }
    }

    private IEnumerator GameSet()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1;
        yield return null;
    }

    public void GameRestart()
    {
        interstitialAdExample.LoadAd();
        interstitialAdExample.ShowAd();
        // ���݂�Scene���擾
        Scene loadScene = SceneManager.GetActiveScene();
        // ���݂̃V�[�����ēǂݍ��݂���
        SceneManager.LoadScene(loadScene.name);
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif
    }

    private bool Jadge(int y, int z, int x)
    {
        int[] sum = new int[3];

        for (int i = 0; i < 4; i++)
        {
            sum[0] += field[i, z, x];
            sum[1] += field[y, i, x];
            sum[2] += field[y, z, i];
        }
        foreach (int i in sum)
        {
            if (Mathf.Abs(i) == 4)
                return true;
        }

        int sumX = 0;

        if ((z * 4 + x) % 3 == 0)
        {
            for (int i = 0; i < 4; i++)
                sumX += field[y, i, 3 - i];
        }
        else if (((z * 4) + x) % 5 == 0)
        {
            for (int i = 0; i < 4; i++)
                sumX += field[y, i, i];
        }
        if (Mathf.Abs(sumX) == 4)
            return true;

        int[] sumDX = new int[4];
        for (int i = 0; i < 4; i++)
        {
            sumDX[0] = field[i, i, i];
            sumDX[1] = field[3 - i, i, i];
            sumDX[2] = field[i, i, 3 - i];
            sumDX[3] = field[3 - i, i, 3 - i];
        }
        foreach (int i in sumDX)
        {
            if (Mathf.Abs(i) == 4)
            {
                return true;
            }
        }
        return false;
    }
}
