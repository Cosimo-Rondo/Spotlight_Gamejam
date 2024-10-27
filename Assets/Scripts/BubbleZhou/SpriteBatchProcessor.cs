using UnityEngine;
using UnityEditor;

public class SpriteBatchProcessor : EditorWindow
{
    [MenuItem("Tools/Sprite Batch Processor")]
    public static void ShowWindow()
    {
        GetWindow<SpriteBatchProcessor>("Sprite Batch Processor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Batch Convert Textures to Sprites"))
        {
            ProcessTextures();
        }
    }

    private void ProcessTextures()
    {
        // 获取所有选中的对象
        Object[] selectedTextures = Selection.objects;

        foreach (Object obj in selectedTextures)
        {
            if (obj is Texture2D texture)
            {
                // 获取纹理的路径
                string assetPath = AssetDatabase.GetAssetPath(texture);

                // 加载纹理导入器
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                if (textureImporter != null)
                {
                    // 设置为 Multiple 模式
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.spriteImportMode = SpriteImportMode.Single;

                    // 重新应用设置
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
            }
        }

        Debug.Log("All selected textures have been converted to Sprites!");
    }
}
