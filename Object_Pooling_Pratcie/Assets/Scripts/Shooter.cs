using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool; //object pool 네임스페이스

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject _BulletPrefab;

    private Camera _MainCam;

    private IObjectPool<Bullet> _Pool;  //bullect을 관리해줄 변수

    private void Awake()
    {
        _Pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize:20);
        //아래에서 만든 Createbullet,onget,onrelease,ondestroy그리고 최대 곗수를 매개 변수로 받음
    }

    // Start is called before the first frame update
    void Start()
    {
        _MainCam = Camera.main;
    } 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hitResult;
            if(Physics.Raycast(_MainCam.ScreenPointToRay(Input.mousePosition), out hitResult))
            {
                var direction = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
                // obejct pooling 없이 총알을 새로 생성해서 사용하는 경우 
                //var bullet = Instantiate(_BulletPrefab, transform.position + direction.normalized, Quaternion.identity).GetComponent<Bullet>();
                //object pooling을 사용하여 총알 발사
                var bullet = _Pool.Get();
                bullet.transform.position = transform.position + direction.normalized;
                bullet.Shoot(direction.normalized);
            }
        }
    }

    private Bullet CreateBullet()   //bullet 생성
    {
        Bullet bullet = Instantiate(_BulletPrefab).GetComponent<Bullet>();
        bullet.SetManagedPool(_Pool);   //자신이 등록되어야할 pool을 알려줌
        return bullet;
    }

    private void OnGetBullet(Bullet bullet) //pool에서 bullet을 가져오기
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet) //pool에 bullet 반환하기
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet) //pool에서 오브젝트 파괴  
    {
        Destroy(bullet.gameObject);
    }
}
