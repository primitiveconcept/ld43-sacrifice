namespace LetsStartAKittyCult
{
    using System.Collections;
    using UnityEngine;


    public class Destroyable : MonoBehaviour
    {
        [SerializeField]
        private GameObject destroyEffectPrefab;

        [SerializeField]
        private AudioClip destroyClip;

        [SerializeField]
        private bool destroyOnStart;

        [SerializeField]
        private float delay;


        public void Destroy()
        {
            if (this.destroyEffectPrefab != null)
            {
                GameObject destroyEffect = PoolManager.Spawn(this.destroyEffectPrefab, this.transform.position);
                destroyEffect.transform.SetParent(this.transform.parent);
            }

            if (this.delay > 0)
                StartCoroutine(DelayedDestroyCoroutine());
            else
                RemoveObject();
        }


        public void DestroyImmediately()
        {
            RemoveObject();
        }


        public void OnSpawn()
        {
            if (this.destroyOnStart)
                Destroy();
        }


        public void Start()
        {
            if (this.destroyOnStart)
                Destroy();
        }


        private IEnumerator DelayedDestroyCoroutine()
        {
            if (GameTime.IsPaused)
                yield return null;

            yield return new WaitForSeconds(this.delay);
            RemoveObject();
        }


        private void RemoveObject()
        {
            if (this.destroyClip != null)
                AudioPlayer.Play(this.destroyClip);

            PoolManager.Despawn(this.gameObject);
        }
    }
}