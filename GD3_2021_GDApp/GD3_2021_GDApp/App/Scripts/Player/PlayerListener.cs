using GDLibrary;
using GDLibrary.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GDApp.Content.Scripts.Player
{
    public class PlayerListener : Component
    {

        public override void Update()
        {
            Application.playerListener.Position = gameObject.Transform.LocalTranslation;
        }
    }
}
