using UnityEditor;
using UnityEngine;
using System.IO;

public class CsvToSO : EditorWindow
{
    private string csvFolderPath = "Assets/Resources/CSV";
    private string savePath = "Assets/Scripts/ScriptableObjects/ItemSO";

    [MenuItem("Tools/Generate ItemData from CSV")]
    public static void ShowWindow()
    {
        GetWindow<CsvToSO>("CSV → SO 생성기");
    }

    void OnGUI()
    {
        GUILayout.Label("CSV 폴더 경로", EditorStyles.boldLabel);
        csvFolderPath = EditorGUILayout.TextField("경로", csvFolderPath);

        if (GUILayout.Button("ScriptableObject 생성"))
        {
            GenerateAllItems();
        }
    }

    private void GenerateAllItems()
    {
        CreateItemData(Path.Combine(csvFolderPath, "3-2.Material.csv"), ItemCategory.Material);
        CreateItemData(Path.Combine(csvFolderPath, "3-3.Equipment.csv"), ItemCategory.Equipment);
        CreateItemData(Path.Combine(csvFolderPath, "3-4.Consumable.csv"), ItemCategory.Consumable);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("모든 ItemData ScriptableObject 생성 완료");
    }

    private void CreateItemData(string path, ItemCategory categoryType)
    {
        var lines = File.ReadAllLines(path);
        for (int i = 2; i < lines.Length; i++) // 2행부터 시작
        {
            var cols = lines[i].Split(',');
            if (cols.Length < 7) continue;

            ScriptableObject so;
            int id;

            if (!int.TryParse(cols[0].Trim(), out id)) continue;

            string fileName = $"{id}.asset";
            string filePath = Path.Combine(savePath, fileName);
            Directory.CreateDirectory(savePath);

            var existing = AssetDatabase.LoadAssetAtPath<ItemSO>(filePath);

            if (categoryType == ItemCategory.Equipment)
            {
                var newSO = ScriptableObject.CreateInstance<EquipmentItemSO>();
                newSO.ID = id;
                newSO.category = categoryType;

                int.TryParse(cols[3], out newSO.maxStack);
                int.TryParse(cols[6], out newSO.price);

                string categoryStr = cols[4].Trim();
                if (categoryStr.Equals("Weapon", System.StringComparison.OrdinalIgnoreCase))
                {
                    newSO.equipmentType = EquipmentType.Weapon;

                    // WeaponType 지정
                    if (System.Enum.TryParse(cols[7].Trim(), true, out WeaponType parsedWeaponType))
                        newSO.weaponType = parsedWeaponType;
                    else
                        newSO.weaponType = WeaponType.None;
                }
                else
                {
                    if (System.Enum.TryParse(cols[7].Trim(), true, out EquipmentType parsedSlot))
                        newSO.equipmentType = parsedSlot;
                    else
                        newSO.equipmentType = EquipmentType.None;

                    newSO.weaponType = WeaponType.None;
                }

                int.TryParse(cols[8], out newSO.essenceSlotAmount);
                float.TryParse(cols[9], out newSO.atk);
                float.TryParse(cols[10], out newSO.def);
                float.TryParse(cols[12], out newSO.resistance);
                float.TryParse(cols[13], out newSO.atkSpeed);
                float.TryParse(cols[14], out newSO.critChance);
                float.TryParse(cols[15], out newSO.critDamage);
                float.TryParse(cols[16], out newSO.attackRange);

                so = newSO;
            }
            else
            {
                var newSO = ScriptableObject.CreateInstance<ItemSO>();
                newSO.ID = id;
                newSO.category = categoryType;
                int.TryParse(cols[3], out newSO.maxStack);
                int.TryParse(cols[6], out newSO.price);

                so = newSO;
            }

            if (existing != null)
            {
                Sprite oldImage = existing.image;

                bool isSame = categoryType == ItemCategory.Equipment
                    ? IsSameEquipment(existing as EquipmentItemSO, so as EquipmentItemSO)
                    : IsSameBasic(existing, so as ItemSO);

                if (!isSame)
                {
                    (so as ItemSO).image = oldImage;
                    EditorUtility.CopySerialized(so, existing);
                    EditorUtility.SetDirty(existing);
                }
            }
            else
            {
                AssetDatabase.CreateAsset(so, filePath);
            }
        }
    }

    private bool IsSameBasic(ItemSO a, ItemSO b)
    {
        return a.ID == b.ID &&
               a.category == b.category &&
               a.price == b.price &&
               a.maxStack == b.maxStack;
    }

    private bool IsSameEquipment(EquipmentItemSO a, EquipmentItemSO b)
    {
        return IsSameBasic(a, b) &&
               a.equipmentType == b.equipmentType &&
               a.atk == b.atk &&
               a.def == b.def &&
               a.critChance == b.critChance &&
               a.critDamage == b.critDamage &&
               a.atkSpeed == b.atkSpeed &&
               a.attackRange == b.attackRange &&
               a.essenceSlotAmount == b.essenceSlotAmount &&
               a.resistance == b.resistance;
    }
}

