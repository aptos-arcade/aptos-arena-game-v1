using UnityEngine;
using UnityEngine.UI;

namespace GameManagement
{
    public class MusicManager : MonoBehaviour
    {

        private AudioSource _musicSource;

        [SerializeField] private Image muteIcon;
        
        private static MusicManager _instance;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _musicSource = GetComponent<AudioSource>();
        }

        public void ToggleMusic()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Stop();
                muteIcon.enabled = true;
            }
            else
            {
                _musicSource.Play();
                muteIcon.enabled = false;
            }
        }
    }
}