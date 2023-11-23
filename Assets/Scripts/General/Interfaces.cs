using Structs;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Interfaces
{
    public interface IInteractable
    {
        int Prioriti { get; }
        bool InteractionsEnabled { get; set; }
        void Interact(CharacterInteraction interaction);
        void Highlight();
        void RemoveHighlight();
    }

    public interface IAim
    {
        public AimSnapshot TakeSnapshot();
    }

    public interface ISpellAttribute
    {
        public void GetAttributeParameters(SpellScriptableObject spell);
        public void OnCastEvent();
        public void OnHitEvent(CollisionData collisionData);
    }

    public interface ISpellSpawnAttribute
    {
        public void GetAttributeParameters(SpellScriptableObject spell);
        public SpellSpawnData SetProjectileSpawnParameters(SpellSpawnData spawnData);
    }

    public interface IDestroyable
    {
        void DestroyObject();
    }

    public interface IMoveAI
    {
        void AIUpdate();
    }

    public interface IAttackAI
    {
        void AIUpdate();
    }

}