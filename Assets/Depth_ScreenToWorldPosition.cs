using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.Lightship.AR.Utilities;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Depth_ScreenToWorldPosition : MonoBehaviour
{
    public AROcclusionManager _occMan;
    public Camera _camera;
    public GameObject _prefabToSpawn;
    private bool isTouch = false;
    public GameObject canvas;

    XRCpuImage? depthimage;
    void Update()
    {
        if (isTouch)
        {
            return;
        }

        if (_occMan == null || !_occMan.subsystem.running)
        {
            return;
        }

        Matrix4x4 displayMat = Matrix4x4.identity;

        if (_occMan.TryAcquireEnvironmentDepthCpuImage(out var image))
        {
            depthimage?.Dispose();
            depthimage = image;
        }
        else
        {
            return;
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            var screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
#else
        if(Input.touches.Length>0)
        {
            var screenPosition = Input.GetTouch(0).position;
#endif
            if (depthimage.HasValue)
            {
                // 被写界深度 
                var uv = new Vector2(screenPosition.x / Screen.width, screenPosition.y / Screen.height);
                uv = new Vector2(0, 0);
                var eyeDepth = depthimage.Value.Sample<float>(uv, displayMat);

                // ワールド座標を取得 
                var worldPosition =
                    _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, eyeDepth));

                //深度マップ上に物をスポーンする 
                if (!isTouch)
                {
                    isTouch = true;
                    Instantiate(_prefabToSpawn, worldPosition, Quaternion.identity);
                    canvas.GetComponent<GameManager>().GameStart();
                }

            }
        }
    }
}
