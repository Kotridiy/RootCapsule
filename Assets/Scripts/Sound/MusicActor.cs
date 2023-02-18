using System.Collections;
using UnityEngine;

namespace RootCapsule.Music
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicActor : MonoBehaviour
    {
        public float DelayBeforePlay = 1f;

        AudioSource source;
        Coroutine coroutine;

        public void StopPlaying()
        {
            StopCoroutine(coroutine);
            source.Stop();
        }

        void Start()
        {
            source = GetComponent<AudioSource>();
            source.Stop();
            coroutine = StartCoroutine(StartMusic());
        }

        IEnumerator StartMusic()
        {
            yield return new WaitForSeconds(DelayBeforePlay);
            source.Play();
        }

    }
}
