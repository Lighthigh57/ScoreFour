using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject Pole;
    [SerializeField] private Transform board;
    [SerializeField] private Material[] materials;

    /// <summary>
    /// ”Õã‚Ìî•ñ‚ğŠi”[(‚½‚ÄA‰œs‚«A‚æ‚±)
    /// </summary>
    private readonly int[,,] field = new int[4, 4, 4];
    /// <summary>
    /// –_‚É‰½ŒÂ“ü‚Á‚Ä‚¢‚é‚©(‰œs‚«A‚æ‚±)
    /// </summary>
    private readonly int[,] onStick = new int[4, 4];

    private InterstitialAdExample interstitialAd;
    private int stone = 1;
    const int distance = 10;
    const int duration = 3;

    // Start is called before the first frame update
    void Start()
    {
        interstitialAd = GetComponent<InterstitialAdExample>();
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

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, duration, false);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 vector = hit.transform.position;
                int x = (int)(vector.x + 1.5f);
                int z = (int)Mathf.Abs(vector.z - 1.5f);
                if (onStick[z, x] < 4)
                {
                    vector.y = 8;

                    GameObject game = Instantiate(sphere, vector, Quaternion.identity);
                    game.GetComponent<Renderer>().material = materials[stone - 1];

                    field[onStick[z, x], z, x] = stone;
                    if (stone == 1)
                        stone++;
                    else
                        stone--;
                    Jadge(onStick[z, x]++, z, x, stone);
                }

                //interstitialAd.LoadAd();
                //interstitialAd.ShowAd();

            }
        }
    }
    private void Jadge(int y, int z, int x, int stone)
    {
        bool[] state = new bool[3];
        for (int i = 0; i < 4; i++)
        {
            if (!(field[i, z, x] == stone + 1))
                state[0] = true;
            if (!(field[y, i, x] == stone + 1))
                state[1] = true;
            if (!(field[y, z, i] == stone + 1))
                state[2] = true;
            if(state[0] == state[1] == state[2] == true)
                return;
        }
        SceneManager.LoadScene("result");
    }
}
