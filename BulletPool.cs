using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    private ObjectPool<Bullet> pool;
    [SerializeField] private Bullet bulletPrefab;

    void Start()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, true, 20, 100);
    }
    public Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.gameObject.SetActive(false); 
        bullet.SetPool(this);
        return bullet;

    }

    public void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    
    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);   
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public Bullet GetBullet(Transform spawnPoint)
    {
        Bullet bullet = pool.Get();
        bullet.transform.position = spawnPoint.position;
        bullet.transform.rotation = spawnPoint.rotation;
        bullet.gameObject.SetActive(true);
        return bullet;
    }

    public void ReleaseBullet(Bullet bullet)
    {
        pool.Release(bullet);
    }
}
