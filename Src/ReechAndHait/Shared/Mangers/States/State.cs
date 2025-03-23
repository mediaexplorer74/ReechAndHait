
// Type: ReachHigh.Shared.State
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace ReachHigh.Shared
{
  public class State
  {
    protected StateManager stateManager;
    protected Sprite background;
    protected bool useGrid;
    public List<Sprite> grid;

    protected bool StartSequence { get; set; }

    public State(StateManager STATEMANAGER, bool USECAM = true, bool NEWPOS = true)
    {
      this.stateManager = STATEMANAGER;
      Globals.camera = new Camera(USECAM, NEWPOS);
      this.useGrid = false;
      this.grid = new List<Sprite>();
      for (int index = -500; index <= 500; ++index)
      {
        this.grid.Add(new Sprite("Tileset\\Ground", new Vector2(-5000f, (float) (index * 60)), new Vector2(50000f, 4f)));
        this.grid.Add(new Sprite("Tileset\\Ground", new Vector2((float) (index * 60), -5000f), new Vector2(4f, 50000f)));
      }
    }

    public virtual void Initialize()
    {
    }

    public virtual void Terminate()
    {
    }

    public virtual void Update(float deltaSeconds)
    {
      Globals.audioManager.Update(deltaSeconds);
      Globals.debugMode = this.useGrid;
    }

    public virtual void Draw()
    {
      if (!this.useGrid)
        return;
      foreach (Sprite sprite in this.grid)
        sprite.Draw();
    }
  }
}
