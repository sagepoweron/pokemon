using UnityEngine;

namespace MyGame
{
    public interface IBreakable
    {
        void Break();
    }

    public interface IBurnable
    {
        void Burn();
    }

    public interface IChoppable
    {
        void Chop(int attack);
    }

    public interface IDamageable
    {
        void Damage(int amount);
    }

    public interface IDigable
    {
        void Dig();
    }

    public interface IInteractable
    {
        void Interact(Vector2 direction);
    }

    public interface IStatus
    {
        bool IsShowing { get; set; }
        void Hide();
        void Show();
    }

}