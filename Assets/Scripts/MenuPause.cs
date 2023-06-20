using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject menuOptions;
    [SerializeField] private GameObject History;
    [SerializeField] private GameObject HUD;
    [SerializeField] private AudioMixer audioMixerMusic;
    private MusicManager _musicManager;
    public bool juegoPausado = false;
    public GameObject MenuDead;


    private void Start()
    {
        _musicManager = Camera.main.GetComponent<MusicManager>();
        Time.timeScale = 0f;
        _musicManager.SwitchMusic(History);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
                Resume();
            else
                Pausa();
        }
    }

    public void Pausa()
    {
        juegoPausado = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _musicManager.SwitchMusic(juegoPausado);
        menuPausa.SetActive(true);
        HUD.SetActive(false);
    }

    public void Resume()
    {
        juegoPausado = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _musicManager.SwitchMusic(juegoPausado);
        menuPausa.SetActive(false);
        menuOptions.SetActive(false);
        History.SetActive(false);
        HUD.SetActive(true);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void BackToMenu() => SceneManager.LoadScene(0);
    public void ChangeVolumeMusic(float volume) => audioMixerMusic.SetFloat("Volume", volume);

}
