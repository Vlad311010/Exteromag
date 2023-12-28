using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpellSet", order = 2)]
public class SpellSetSO : ScriptableObject, ISerializationCallbackReceiver
{
    public class SpellUpgradeNode
    {
        public string description = "";
        public SpellScriptableObject spell;
        public List<SpellUpgradeNode> children = new List<SpellUpgradeNode>();

        public void Add(SpellUpgradeNode node)
        {
            children.Add(node);
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

    SerializableNode SerizlizeNode(SpellUpgradeNode node)
    {
        SerializableNode serializedNode = new SerializableNode()
        {
            spell = node.spell,
            description = node.description,
            childCount = node.children.Count,

        };
        
        return serializedNode;
    }
    
    void AddNodeToSerializedNodes(SpellUpgradeNode node)
    {
        SerializableNode serializedNode = new SerializableNode()
        {
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

