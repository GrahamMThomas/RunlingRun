namespace RunlingRun.Effects
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.VFX;

    public class Blink : MonoBehaviour
    {
        public VisualEffect vfx;
        public int SpawnRate = 100;

        private void Awake()
        {
            vfx.GetComponent<VisualEffect>();
        }
        private void Start()
        {
            vfx.SetInt("SpawnRate", SpawnRate);
            StartCoroutine(DieSlowly());
        }

        private IEnumerator DieSlowly()
        {
            int decayRate = vfx.GetInt("SpawnRate") / 10;
            while (vfx.GetInt("SpawnRate") > 0)
            {
                vfx.SetInt("SpawnRate", vfx.GetInt("SpawnRate") - decayRate);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
}