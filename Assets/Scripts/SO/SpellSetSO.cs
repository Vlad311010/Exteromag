using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpellSet", order = 2)]
public class SpellSetSO : ScriptableObject, ISerializationCallbackReceiver
{
    public class SpellUpgradeNode
    {
        public Translator.Translation[] translation = new Translator.Translation[] { new Translator.Translation(SystemLanguage.English), new Translator.Translation(SystemLanguage.Ukrainian) };
        public string description = "";
        public SpellScriptableObject spell;
        public List<SpellUpgradeNode> children = new List<SpellUpgradeNode>();

        public void Add(SpellUpgradeNode node)
        {
            children.Add(node);
        }

        public int GetDescriptionTranslationIndex(SystemLanguage language)
        {
            for (int i = 0; i < translation.Length; i++)
            {
                if (translation[i].language == language)
                {
                    return i;
                }
            }

            return 0;
        }

        public int GetDescriptionTranslationIndex(string translationName)
        {
            for (int i = 0; i < translation.Length; i++)
            {
                if (translation[i].name == translationName)
                {
                    return i;
                }
            }

            return 0;
        }

    }

    public SpellUpgradeNode root;
    public List<SerializableNode> serializedNodes;
    int deserializationIndex = 0;

    public List<SpellUpgradeNode> upgrades { get => root.children; }
    public SpellScriptableObject spell { get => root.spell; }


    [Serializable]
    public struct SerializableNode
    {
        public Translator.Translation[] translation;
        public string description;
        public SpellScriptableObject spell;
        public int childCount;
    }


    public void OnBeforeSerialize()
    {
        if (serializedNodes == null)
            serializedNodes = new List<SerializableNode>();

        if (root == null)
            root = new SpellUpgradeNode();

        serializedNodes.Clear();
        AddNodeToSerializedNodes(root);
    }

    /*SerializableNode SerizlizeNode(SpellUpgradeNode node)
    {
        SerializableNode serializedNode = new SerializableNode()
        {
            translation = node.translation,
            spell = node.spell,
            description = node.description,
            childCount = node.children.Count,

        };
        
        return serializedNode;
    }*/
    
    void AddNodeToSerializedNodes(SpellUpgradeNode node)
    {
        SerializableNode serializedNode = new SerializableNode()
        {
            translation = node.translation,
            spell = node.spell,
            description = node.description,
            childCount = node.children.Count,
        };

        serializedNodes.Add(serializedNode);

        foreach (var child in node.children)
            AddNodeToSerializedNodes(child);
    }

    public void OnAfterDeserialize()
    {
        deserializationIndex = 0;
        if (serializedNodes.Count > 0)
            root = ReadNodeFromSerializedNodes();
        else
            root = new SpellUpgradeNode();
    }

    SpellUpgradeNode ReadNodeFromSerializedNodes()
    {
        var serializedNode = serializedNodes[deserializationIndex];
        deserializationIndex += 1;
        var children = new List<SpellUpgradeNode>();
        for (int i = 0; i < serializedNode.childCount; i++)
            children.Add(ReadNodeFromSerializedNodes());

        return new SpellUpgradeNode()
        {
            translation = serializedNode.translation,
            spell = serializedNode.spell,
            description = serializedNode.description,
            children = children
        };
    }

    public void Traverse(int childIdx)
    {
        root = root.children[childIdx];
    }
}

