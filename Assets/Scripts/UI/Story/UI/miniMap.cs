using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] Transform mapData;
    [SerializeField] Image image;

    [SerializeField] Color wallColor;//ミニマップで表示する壁の色
    [SerializeField] Color groundColor;//ミニマップで表示する地面の色
    [SerializeField] Color noneColor;//ミニマップで表示する何もないところの色

    // マップ用テクスチャ
    Texture2D texture;



    void Start()
    {
        Tilemap groundTilemap = mapData.Find("Ground").GetComponent<Tilemap>();
        Tilemap wallTilemap = mapData.Find("Wall").GetComponent<Tilemap>();

        // テクスチャ作成
        Vector3Int size = wallTilemap.size;
        texture = new Texture2D(size.x, size.y, TextureFormat.ARGB32, false);

        
        texture.filterMode = FilterMode.Point;// 画像のぼやけ防止

        Vector3Int origin = wallTilemap.origin;

        // テクスチャ座標ごとの色を求める
        for (int y = 0; y < size.y; ++y)
        {
            for (int x = 0; x < size.x; ++x)
            {
                // Tilemapのグリッド座標
                Vector3Int cellPos = new Vector3Int(origin.x + x, origin.y + y, 0);

                // 壁が存在した場合
                if (wallTilemap.GetTile(cellPos) != null)
                {
                    texture.SetPixel(x, y, wallColor);
                }

                // 地面が存在した場合
                else if (groundTilemap.GetTile(cellPos) != null)
                {
                    texture.SetPixel(x, y, groundColor);
                }

                // 何もない場合
                else
                {
                    texture.SetPixel(x, y, noneColor);
                }
            }
        }

        texture.Apply();// テクスチャ確定

        // テクスチャをImageに適用
        image.rectTransform.sizeDelta = new Vector2(size.x, size.y);
        image.sprite = Sprite.Create(texture, new Rect(0, 0, size.x, size.y), Vector2.zero);

        // _imageをGridの中心に移動
        Vector2 leftDownWorldPos = wallTilemap.CellToWorld(origin);
        Vector2 rightUpWorldPos = wallTilemap.CellToWorld(origin + size);
        image.transform.position = (leftDownWorldPos + rightUpWorldPos) * 0.5f;
    }

    private void OnDestroy()
    {
        Destroy(texture);
    }
}