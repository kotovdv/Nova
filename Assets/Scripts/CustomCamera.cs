using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class CustomCamera : MonoBehaviour
{
    public Camera cam;
    private bool _canScroll = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var delta = Input.mouseScrollDelta;
        if (_canScroll && delta != Vector2.zero)
        {
            _canScroll = false;
            DoZoom(delta);
        }
    }


    private async void DoZoom(Vector2 direction)
    {
        for (float i = 0; i <= 1; i += 0.05f)
        {
            cam.orthographicSize += -direction.y * 0.05F;
            await new WaitForSeconds(0.025f);
        }

        _canScroll = true;
    }
}