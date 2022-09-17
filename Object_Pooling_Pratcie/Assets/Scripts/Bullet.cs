using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool; //object pool 네임스페이스

public class Bullet : MonoBehaviour
{
    private Vector3 _Direction;

    [SerializeField]
    private float _Speed = 3f;

    private IObjectPool<Bullet> _ManagedPool;   //bullet을 관리할 변수 선언

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_Direction * Time.deltaTime * _Speed);
    }

    public void SetManagedPool(IObjectPool<Bullet> pool)    //pool 매개변수로 받은 bullet을 managedpool에 저장
    {
        _ManagedPool = pool;
    }

    public void Shoot(Vector3 dir)
    {
        _Direction = dir;
        // object pool 쓰지않을때에 총알 삭제 로직 ,5초가 지난 후 총알을 파괴
        //Destroy(gameObject, 5f);
        //object pool을 사용하여 5초뒤 bullet을 다시 pool에 반환
        Invoke("DestroyBullet", 5f);
    }

    public void DestroyBullet()     //호출시 bullet을 다시 pool에 반환
    {
        _ManagedPool.Release(this);
    }
}
