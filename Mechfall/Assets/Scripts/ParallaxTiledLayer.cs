using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxTiledLayer : MonoBehaviour
{
    [Tooltip("0 = fixed to camera, 1 = moves with foreground")]
    public float multiplierX = 0.2f;
    public float multiplierY = 0f;

    [Tooltip("Optional slow drift (units/sec). Negative scrolls left.")]
    public float autoScrollX = 0f;

    [Tooltip("Leave null to auto-use main camera")]
    public Transform targetCamera;

    Transform _cam;
    Vector3 _prevCamPos;
    SpriteRenderer _src;
    float _tileWidth;

    Transform _L, _C, _R;

    void Awake()
    {
        if (!targetCamera && Camera.main) targetCamera = Camera.main.transform;
        _cam = targetCamera;
        _src = GetComponent<SpriteRenderer>();
        InitTiles();
        if (_cam) _prevCamPos = _cam.position;
    }

    void OnValidate()
    {
        if (!Application.isPlaying && GetComponent<SpriteRenderer>()) InitTiles();
    }

    void InitTiles()
    {
        if (!_src || !_src.sprite) return;
        _tileWidth = _src.bounds.size.x;

        // Destroy old clones
        foreach (Transform c in transform) DestroyImmediate(c.gameObject);

        // Create center from our own renderer data
        _C = new GameObject("Tile_C").transform;
        _C.SetParent(transform, false);
        var cR = _C.gameObject.AddComponent<SpriteRenderer>();
        CopyRenderer(_src, cR);

        _L = Instantiate(_C, transform); _L.name = "Tile_L";
        _R = Instantiate(_C, transform); _R.name = "Tile_R";
        _L.localPosition = Vector3.left * _tileWidth;
        _R.localPosition = Vector3.right * _tileWidth;

        _src.enabled = false; // hide source, use clones
    }

    void LateUpdate()
    {
        if (_cam == null) return;

        Vector3 camPos = _cam.position;
        Vector3 delta = camPos - _prevCamPos;

        transform.position += new Vector3(delta.x * multiplierX, delta.y * multiplierY, 0f);

        if (autoScrollX != 0f)
            transform.position += new Vector3(autoScrollX * Time.deltaTime, 0f, 0f);

        _prevCamPos = camPos;
        RecycleIfNeeded();
    }

    void RecycleIfNeeded()
    {
        Transform left = _L, center = _C, right = _R;
        OrderByX(ref left, ref center, ref right);

        float camX = _cam.position.x;
        float rightEdge = right.position.x - _tileWidth * 0.5f;
        float leftEdge  = left.position.x + _tileWidth * 0.5f;

        if (camX > rightEdge)
        {
            // move left tile to right
            left.position = right.position + Vector3.right * _tileWidth;
            (_L, _C, _R) = (_C, _R, left);
        }
        else if (camX < leftEdge - _tileWidth)
        {
            // move right tile to left
            right.position = left.position - Vector3.right * _tileWidth;
            (_L, _C, _R) = (right, _L, _C);
        }
    }

    static void OrderByX(ref Transform a, ref Transform b, ref Transform c)
    {
        if (a.position.x > b.position.x) Swap(ref a, ref b);
        if (b.position.x > c.position.x) Swap(ref b, ref c);
        if (a.position.x > b.position.x) Swap(ref a, ref b);
    }
    static void Swap(ref Transform x, ref Transform y) { (x, y) = (y, x); }

    static void CopyRenderer(SpriteRenderer src, SpriteRenderer dst)
    {
        dst.sprite = src.sprite;
        dst.color = src.color;
        dst.flipX = src.flipX; dst.flipY = src.flipY;
        dst.sortingLayerID = src.sortingLayerID;
        dst.sortingOrder = src.sortingOrder;
        dst.sharedMaterial = src.sharedMaterial;
    }
}
