using UnityEngine;

public class CarExplosion : MonoBehaviour
{
    private float _lifetime;
    private float _timer = 0;

    private void Awake()
    {
        _lifetime = gameObject.GetComponent<ParticleSystem>().main.duration;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _lifetime)
        {
            Destroy(gameObject);
        }
    }
}
