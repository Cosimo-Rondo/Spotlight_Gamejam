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
        // ��ȡ����ѡ�еĶ���
        Object[] selectedTextures = Selection.objects;

        foreach (Object obj in selectedTextures)
        {
            if (obj is Texture2D texture)
            {
                // ��ȡ�����·��
                string assetPath = AssetDatabase.GetAssetPath(texture);

                // ������������
                TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                if (textureImporter != null)
                {
                    // ����Ϊ Multiple ģʽ
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.spriteImportMode = SpriteImportMode.Single;

                    // ����Ӧ������
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
            }
        }

        Debug.Log("All selected textures have been converted to Sprites!");
    }
}
