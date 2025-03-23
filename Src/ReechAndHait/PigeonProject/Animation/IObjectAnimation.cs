﻿
// Type: PigeonProject.Animation.IObjectAnimation
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;

#nullable disable
namespace PigeonProject.Animation
{
  public interface IObjectAnimation
  {
    TerminateEffect TerminateEffect { set; }

    void Initialize();

    void Update(GameTime gameTime);
  }
}
