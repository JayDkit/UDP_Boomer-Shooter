using GDLibrary.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GDLibrary.Managers
{
    public interface ITween
    {
        void Awake();
        void OnEnabled();
        void OnDisabled();
        void Update();

        void Pause();
        void Toggle();
        void Resume();
        void Cancel();
        void CancelOnComplete();
        void OnComplete(Action callback);
    }
    public interface IManageTweens
    {
        bool Add(ITween tween);
        bool Remove(ITween tween);
        bool Remove(Predicate<ITween> predicate);

        //void Pause();
        //void Toggle();
        //void Resume();
    }

    public class TweenManager : GameComponent, IManageTweens
    {
        private readonly int DEFAULT_LIST_SIZE = 10;
        private Dictionary<object, List<ITween>> tweens;
        private List<ITween> toAdd, toRemove;

        public TweenManager(Game game) : base(game)
        {
            tweens = new Dictionary<object, List<ITween>>();
            toAdd = new List<ITween>(DEFAULT_LIST_SIZE);
            toRemove = new List<ITween>(DEFAULT_LIST_SIZE);
        }

        public bool Add(ITween tween)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ITween tween)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Predicate<ITween> predicate)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}