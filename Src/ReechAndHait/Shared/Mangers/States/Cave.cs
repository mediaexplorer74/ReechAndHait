
// Type: ReachHigh.Shared.Cave
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;

#nullable disable
namespace ReachHigh.Shared
{
  public class Cave : State
  {
    public Gecko gecko;
    public Monkey monkey;
    public Bumerang bumerang;
    public Target target;
    public List<Sprite> collidingEnvironment;
    public List<Sprite> environment;
    public List<Sprite> foreEnvironment;
    public List<Box> boxes;
    public List<Lever> levers;
    public List<Gate> gates;
    public List<Bar> bars;
    public List<GateCollider> gateCollider;
    private List<RadialSound> radialSounds;
    private List<WorldTexture> worldTextures;
    private List<string[,,]> worldTexturePaths;
    private int gateCount;
    private SoundEffectInstance gateOpenInstance;
    private SoundEffectInstance gateCloseInstance;
    private float exitReach;
    private bool exitCave;
    private int saveData;
    private bool perspective;
    public List<Rectangle> tutorialPrompts;
    private const float tileSize = 60f;

    public List<Sprite> SpecialEnvironment { get; set; }

    public Cave(StateManager STATEMANAGER, bool STARTSEQUENCE)
      : base(STATEMANAGER)
    {
    StorageFolder AppFolder = ApplicationData.Current.LocalFolder;
      this.collidingEnvironment = new List<Sprite>();
      this.SpecialEnvironment = new List<Sprite>();
      this.environment = new List<Sprite>();
      this.foreEnvironment = new List<Sprite>();
      this.boxes = new List<Box>();
      this.levers = new List<Lever>();
      this.gates = new List<Gate>();
      this.bars = new List<Bar>();
      this.gateCollider = new List<GateCollider>();
      this.gateOpenInstance = Globals.audioManager.sounds["Gate_Open"].CreateInstance();
      this.gateCloseInstance = Globals.audioManager.sounds["Gate_Close"].CreateInstance();
      this.StartSequence = STARTSEQUENCE;
      this.tutorialPrompts = new List<Rectangle>();
      this.SetRadialSounds();
      this.SetWorldTextures();
      Worldbuilding.CollidingEnvironment = this.collidingEnvironment;
      Worldbuilding.DrawEnvironment = this.environment;
      GateAnimation.ResetManager();
      Worldbuilding.CreateCollider(new Vector2(-1860f, -5340f), 7140f, 60f, "ceiling 1", "");
      Worldbuilding.CreateCollider(new Vector2(-1320f, 0.0f), 480f, 1980f, "ceiling 2", "");
      Worldbuilding.CreateCollider(660f, 0.0f, 300f, 900f, "ceiling 8", "");
      Worldbuilding.CreateCollider(-1320f, -5340f, 5340f, 60f, "ceiling 7", "");
      Worldbuilding.CreateCollider(new Vector2(2760f, 0.0f), 360f, 420f, "ceiling 3", "");
      Worldbuilding.CreateCollider(new Vector2(3180f, 0.0f), 840f, 120f, "ceiling 4", "");
      Worldbuilding.CreateCollider(new Vector2(3300f, 420f), 60f, 60f, "ceiling 5", "");
      Worldbuilding.CreateCollider(new Vector2(3300f, -60f), 540f, 980f, "ceiling 6", "");
      Worldbuilding.CreateClimbWall(new Vector2(3340f, 480f), 360f, "climb 4");
      Worldbuilding.CreateStickyCeiling(new Vector2(1560f, 60f), 1200f, "sticky 1");
      Worldbuilding.CreateStickyCeiling(new Vector2(3180f, 840f), 180f, "sticky 2");
      Worldbuilding.CreateStickyCeiling(new Vector2(3400f, 450f), 400f, "sticky 3");
      Worldbuilding.CreateCollider(new Vector2(-1800f, 720f), 1440f, 2040f, "floor 1", "Grass");
      Worldbuilding.CreateCollider(new Vector2(180f, 1440f), 720f, 300f, "floor 2", "");
      Worldbuilding.CreateCollider(new Vector2(480f, 2040f), 120f, 60f, "floor 3", "");
      Worldbuilding.CreateCollider(new Vector2(540f, 2050f), 150f, 960f, "floor 4", "");
      Worldbuilding.CreateCollider(new Vector2(1500f, 2180f), 40f, 900f, "floor 5", "");
      Worldbuilding.CreateCollider(new Vector2(1620f, 1950f), 100f, 300f, "floor 6", "");
      Worldbuilding.CreateCollider(new Vector2(1920f, 1800f), 240f, 120f, "floor 7", "");
      Worldbuilding.CreateCollider(new Vector2(2040f, 1740f), 240f, 120f, "floor 8", "");
      Worldbuilding.CreateCollider(new Vector2(2160f, 960f), 450f, 120f, "floor 9", "");
      Worldbuilding.CreateCollider(new Vector2(2160f, 1380f), 570f, 120f, "floor 10", "");
      Worldbuilding.CreateCollider(new Vector2(1920f, 1950f), 100f, 360f, "floor 11", "");
      Worldbuilding.CreateCollider(new Vector2(1560f, 970f), 160f, 600f, "floor 12", "");
      Worldbuilding.CreateClimbWall(new Vector2(480f, 1440f), 600f, "climb 1");
      Worldbuilding.CreateStickyCeiling(new Vector2(1560f, 1120f), 600f, "sticky 4");
      Worldbuilding.CreateCollider(new Vector2(1500f, 2050f), -1f, 120f, typeof (Floaty), "floaty 1");
      Worldbuilding.CreateCollider(new Vector2(870f, 1030f), -1f, 180f, typeof (Floaty), "floaty 2");
      Worldbuilding.CreateCollider(new Vector2(1110f, 1210f), -1f, 180f, typeof (Floaty), "floaty 3");
      Worldbuilding.CreateCollider(new Vector2(1380f, 1390f), -1f, 180f, typeof (Floaty), "floaty 4");
      Worldbuilding.CreateCollider(new Vector2(2040f, 1380f), -1f, 120f, typeof (Floaty), "floaty 5");
      Worldbuilding.CreateCollider(new Vector2(2040f, 1500f), -1f, 120f, typeof (Floaty), "floaty 6");
      Worldbuilding.CreateCollider(new Vector2(2040f, 1620f), -1f, 120f, typeof (Floaty), "floaty 7");
      Worldbuilding.CreateCollider(new Vector2(1800f, 720f), -1f, 180f, typeof (Floaty), "floaty 8");
      Worldbuilding.CreateCollider(new Vector2(1800f, 840f), -1f, 180f, typeof (Floaty), "floaty 9");
      Worldbuilding.CreateCollider(new Vector2(2390f, 1450f), -1f, 180f, typeof (Floaty), "floaty 10");
      Worldbuilding.CreateCollider(new Vector2(2930f, 1520f), -1f, 180f, typeof (Floaty), "floaty 11");
      Worldbuilding.CreateCollider(new Vector2(3420f, 1080f), -1f, 210f, typeof (Floaty), "floaty 12");
      Worldbuilding.CreateCollider(new Vector2(780f, 1560f), 240f, 120f, "platform 1", "");
      Worldbuilding.CreateCollider(new Vector2(1050f, 1680f), 120f, 120f, "platform 2", "");
      Worldbuilding.CreateCollider(new Vector2(1380f, 1650f), 270f, 120f, "platform 3", "");
      Worldbuilding.CreateCollider(new Vector2(1500f, 1650f), 90f, 180f, "platform 4", "");
      Worldbuilding.CreateCollider(new Vector2(1680f, 1380f), 330f, 120f, "platform 5", "");
      Worldbuilding.CreateCollider(new Vector2(1800f, 1380f), 200f, 120f, "platform 6", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1920f, 1380f), 120f, 120f, "platform 7", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1350f, 720f), 90f, 450f, "platform 8", "");
      Worldbuilding.CreateCollider(new Vector2(2280f, 420f), 960f, 100f, "platform 9", "Grass");
      Worldbuilding.CreateCollider(new Vector2(2400f, 2060f), 100f, 240f, "platform 10", "");
      Worldbuilding.CreateCollider(new Vector2(2640f, 2040f), 120f, 60f, "platform 11", "");
      Worldbuilding.CreateCollider(new Vector2(2640f, 1620f), 180f, 60f, "platform 12", "");
      Worldbuilding.CreateCollider(new Vector2(2700f, 1620f), 540f, 120f, "platform 13", "");
      Worldbuilding.CreateCollider(new Vector2(2820f, 1880f), 280f, 360f, "platform 14", "");
      Worldbuilding.CreateCollider(new Vector2(3170f, 1720f), 440f, 190f, "platform 15", "");
      Worldbuilding.CreateCollider(new Vector2(3360f, 1260f), 900f, 120f, "platform 16", "");
      Worldbuilding.CreateCollider(new Vector2(3480f, 1380f), 780f, 120f, "platform 17", "");
      Worldbuilding.CreateCollider(new Vector2(3600f, 780f), 1380f, 660f, "platform 18", "");
      Worldbuilding.CreateStickyCeiling(new Vector2(1380f, 800f), 420f, "sticky 5");
      Worldbuilding.CreateClimbWall(new Vector2(2380f, 420f), 900f, "climb 2", "Grass");
      Worldbuilding.CreateClimbWall(new Vector2(2640f, 1620f), 420f, "climb 3");
      this.gates.Add(new Gate("Tileset\\Gate", new Vector2(3900f, 240f), new Vector2(180f, 600f), "right"));
      Worldbuilding.CreateCollider(new Vector2(4220f, -120f), 60f, 2320f, "ceiling 1", "");
      Worldbuilding.CreateCollider(new Vector2(6540f, -120f), 660f, 1440f, "ceiling 2", "");
      Worldbuilding.CreateStickyCeiling(new Vector2(6720f, 480f), 660f, "sticky 1");
      Worldbuilding.CreateCollider(new Vector2(4260f, 960f), 420f, 60f, "floor 1", "");
      Worldbuilding.CreateCollider(new Vector2(4320f, 1320f), 60f, 900f, "floor 2", "");
      Worldbuilding.CreateCollider(new Vector2(4820f, 620f), 220f, 400f, "floor 3", "Grass");
      Worldbuilding.CreateCollider(new Vector2(5220f, 660f), 180f, 60f, "floor 4", "Grass");
      Worldbuilding.CreateCollider(new Vector2(5280f, 660f), 120f, 180f, "floor 5", "Grass");
      Worldbuilding.CreateCollider(new Vector2(5040f, 1020f), 120f, 420f, "floor 6", "");
      Worldbuilding.CreateCollider(new Vector2(5220f, 1140f), 240f, 240f, "floor 7", "");
      Worldbuilding.CreateCollider(new Vector2(5460f, 900f), 240f, 120f, "floor 8", "");
      Worldbuilding.CreateCollider(new Vector2(5580f, 780f), 360f, 240f, "floor 9", "Grass");
      Worldbuilding.CreateCollider(new Vector2(5360f, 240f), 240f, 400f, "floor 10", "");
      Worldbuilding.CreateClimbWall(new Vector2(4260f, 780f), 180f, "climb 1");
      Worldbuilding.CreateClimbWall(new Vector2(4780f, 620f), 220f, "climb 2");
      Worldbuilding.CreateClimbWall(new Vector2(5320f, 240f), 240f, "climb 3");
      Worldbuilding.CreateClimbWall(new Vector2(5760f, 240f), 240f, "climb 4");
      Worldbuilding.CreateStickyCeiling(new Vector2(4800f, 820f), 480f, "sticky 2");
      Worldbuilding.CreateStickyCeiling(new Vector2(5340f, 480f), 480f, "sticky 3");
      Worldbuilding.CreateStickyCeiling(new Vector2(4460f, 1380f), 960f, "sticky 4");
      this.boxes.Add(new Box("Tileset\\Box", new Vector2(5045f, 1140f), new Vector2(175f, 175f), this.collidingEnvironment, this.monkey));
      Worldbuilding.CreateCollider(new Vector2(4260f, 1380f), 780f, 200f, "platform 1", "");
      Worldbuilding.CreateCollider(new Vector2(4440f, 1680f), 480f, 340f, "platform 2", "");
      Worldbuilding.CreateCollider(new Vector2(4800f, 2040f), 120f, 420f, "platform 3", "Grass");
      Worldbuilding.CreateCollider(new Vector2(5280f, 1680f), 480f, 180f, "platform 4", "Grass");
      Worldbuilding.CreateCollider(new Vector2(5460f, 2000f), 160f, 180f, "platform 5", "");
      Worldbuilding.CreateCollider(new Vector2(5640f, 1910f), 250f, 520f, "platform 6", "");
      Worldbuilding.CreateCollider(new Vector2(6150f, 1840f), 320f, 270f, "platform 7", "");
      Worldbuilding.CreateCollider(new Vector2(6420f, 1740f), 420f, 540f, "platform 8", "");
      Worldbuilding.CreateCollider(new Vector2(6800f, 1140f), 420f, 280f, "platform 9", "Grass");
      Worldbuilding.CreateCollider(new Vector2(6960f, 1560f), 600f, 120f, "platform 10", "");
      Worldbuilding.CreateCollider(new Vector2(7140f, 840f), 1320f, 960f, "platform 11", "");
      Worldbuilding.CreateClimbWall(new Vector2(6740f, 1140f), 420f, "climb 5");
      Worldbuilding.CreateClimbWall(new Vector2(5220f, 1680f), 360f, "climb 7");
      Worldbuilding.CreateClimbWall(new Vector2(7080f, 840f), 300f, "climb 8");
      this.boxes.Add(new Box("Tileset\\Box", new Vector2(6780f, 1560f), new Vector2(175f, 175f), this.collidingEnvironment, this.monkey));
      this.levers.Add(new Lever("Tileset\\Lever", new Vector2(4610f, 1560f), new Vector2(80f, 120f), "Lever"));
      this.gates.Add(new Gate("Tileset\\Gate", new Vector2(7560f, 300f), new Vector2(180f, 600f), "right", true));
      this.gateCollider.Add(new GateCollider("Tileset\\Barrier", new Vector2(7400f, 300f), new Vector2(60f, 600f), this.levers[0]));
      Worldbuilding.CreateCollider(new Vector2(7080f, 1140f), 1020f, 120f);
      Worldbuilding.CreateCollider(new Vector2(4740f, 2040f), 120f, 60f);
      Worldbuilding.CreateCollider(new Vector2(5220f, 2040f), 120f, 60f);
      Worldbuilding.CreateCollider(new Vector2(6560f, 1080f), -1f, 180f, typeof (Floaty), "floaty 1");
      Worldbuilding.CreateCollider(new Vector2(6360f, 900f), -1f, 140f, typeof (Floaty), "floaty 2");
      Worldbuilding.CreateCollider(new Vector2(6060f, 780f), -1f, 160f, typeof (Floaty), "floaty 3");
      Worldbuilding.CreateCollider(new Vector2(5820f, 600f), -1f, 160f, typeof (Floaty), "floaty 4");
      Worldbuilding.CreateCollider(new Vector2(8340f, 720f), 120f, 600f, "right 1", "");
      Worldbuilding.CreateCollider(new Vector2(8940f, 600f), 120f, 120f, "right 2", "");
      Worldbuilding.CreateCollider(new Vector2(9060f, 480f), 120f, 120f, "right 3", "");
      Worldbuilding.CreateCollider(new Vector2(8580f, 480f), 120f, 240f, "right 4", "");
      Worldbuilding.CreateCollider(new Vector2(8520f, 0.0f), 600f, 60f, "right 5", "");
      Worldbuilding.CreateCollider(new Vector2(8460f, 0.0f), 180f, 60f, "right 6", "");
      Worldbuilding.CreateCollider(new Vector2(8580f, 0.0f), 120f, 120f, "right 7", "");
      Worldbuilding.CreateCollider(new Vector2(8700f, 240f), 80f, 360f, "right 8", "");
      Worldbuilding.CreateCollider(new Vector2(9060f, -60f), 360f, 120f, "right 9", "");
      Worldbuilding.CreateCollider(new Vector2(9180f, -60f), 660f, 120f, "right 10", "");
      Worldbuilding.CreateCollider(new Vector2(9420f, -180f), 1200f, 360f, "right 11", "Grass");
      Worldbuilding.CreateCollider(new Vector2(9300f, -60f), 600f, 120f, "right 12", "");
      Worldbuilding.CreateCollider(new Vector2(9780f, -840f), 660f, 60f, "right 13", "");
      Worldbuilding.CreateCollider(new Vector2(8820f, -600f), 80f, 360f, "right 14", "");
      Worldbuilding.CreateCollider(new Vector2(8580f, -600f), 120f, 240f, "right 15", "");
      Worldbuilding.CreateCollider(new Vector2(9000f, -1200f), 300f, 320f, "right 16", "");
      Worldbuilding.CreateCollider(8940f, -1200f, 360f, 360f, "right 22", "Grass");
      Worldbuilding.CreateCollider(new Vector2(9320f, -1380f), 480f, 160f, "right 17", "");
      Worldbuilding.CreateCollider(new Vector2(9480f, -1920f), 480f, 60f, "right 18", "");
      Worldbuilding.CreateCollider(new Vector2(9120f, -2220f), 300f, 360f, "right 19", "");
      Worldbuilding.CreateCollider(new Vector2(8990f, -1530f), 120f, 120f, "right 20", "");
      Worldbuilding.CreateCollider(new Vector2(9300f, 540f), 180f, 120f, "right 21", "");
      Worldbuilding.CreateCollider(8100f, 840f, 100f, 240f, "right 23", "Grass");
      Worldbuilding.CreateClimbWall(new Vector2(8460f, 180f), 420f, "climb 1");
      Worldbuilding.CreateClimbWall(new Vector2(8940f, -1200f), 240f, "climb 2");
      Worldbuilding.CreateStickyCeiling(new Vector2(8700f, 310f), 480f, "sticky 1");
      Worldbuilding.CreateStickyCeiling(new Vector2(8820f, -540f), 360f, "sticky 2");
      this.bars.Add(new Bar("Tileset\\Bar_Vertical", new Vector2(9300f, -60f), new Vector2(120f, 420f), "up"));
      this.levers.Add(new Lever("Tileset\\Pulllever", new Vector2(9705f, -300f), new Vector2(180f, 90f), "Pulllever"));
      Worldbuilding.CreateCollider(new Vector2(7920f, -600f), 840f, 180f, "left 1", "");
      Worldbuilding.CreateCollider(new Vector2(8100f, -600f), 480f, 60f, "left 2", "");
      Worldbuilding.CreateCollider(new Vector2(8160f, -700f), 280f, 100f, "left 3", "");
      Worldbuilding.CreateCollider(new Vector2(7200f, -1680f), 1560f, 720f, "left 4", "");
      Worldbuilding.CreateCollider(new Vector2(7920f, -1680f), 360f, 240f, "left 5", "");
      Worldbuilding.CreateCollider(new Vector2(8160f, -1080f), 240f, 420f, "left 6", "");
      Worldbuilding.CreateCollider(new Vector2(8640f, -1260f), 180f, 60f, "left 7", "");
      Worldbuilding.CreateCollider(new Vector2(8640f, -1440f), 180f, 60f, "left 8", "");
      Worldbuilding.CreateCollider(new Vector2(8580f, -1080f), 240f, 60f, "left 9", "");
      Worldbuilding.CreateClimbWall(new Vector2(8100f, -240f), 360f, "climb 3");
      Worldbuilding.CreateClimbWall(new Vector2(8640f, -1080f), 260f, "climb 4");
      Worldbuilding.CreateClimbWall(new Vector2(8580f, -1440f), 360f, "climb 5");
      Worldbuilding.CreateStickyCeiling(new Vector2(8160f, -880f), 480f, "sticky 3");
      Worldbuilding.CreateStickyCeiling(new Vector2(8040f, -2200f), 1120f, "sticky 4");
      this.gates.Add(new Gate("Tileset\\Gate", new Vector2(7500f, -2220f), new Vector2(180f, 600f), "left"));
      Worldbuilding.CreateCollider(new Vector2(8460f, -1440f), -1f, 120f, typeof (Floaty), "floaty 1");
      Worldbuilding.CreateCollider(new Vector2(8040f, -1080f), -1f, 120f, typeof (Floaty), "floaty 2");
      Worldbuilding.CreateCollider(new Vector2(8040f, -960f), -1f, 120f, typeof (Floaty), "floaty 3");
      Worldbuilding.CreateCollider(new Vector2(7920f, -840f), -1f, 120f, typeof (Floaty), "floaty 4");
      Worldbuilding.CreateCollider(new Vector2(7920f, -720f), -1f, 120f, typeof (Floaty), "floaty 5");
      Worldbuilding.CreateCollider(new Vector2(8580f, 240f), -1f, 120f, typeof (Floaty), "floaty 6");
      Worldbuilding.CreateCollider(new Vector2(8580f, 360f), -1f, 120f, typeof (Floaty), "floaty 7");
      Worldbuilding.CreateCollider(new Vector2(9060f, 600f), 120f, 240f, "platform 2", "");
      Worldbuilding.CreateCollider(new Vector2(9480f, -2220f), 1320f, 120f, "platform 3", "");
      Worldbuilding.CreateCollider(new Vector2(9600f, -2220f), 1320f, 180f, "platform 4", "");
      Worldbuilding.CreateCollider(new Vector2(9780f, -2220f), 4380f, 180f, "platform 5", "");
      Worldbuilding.CreateCollider(new Vector2(7020f, -2160f), 180f, 960f, "4.24", "");
      Worldbuilding.CreateCollider(new Vector2(6900f, -2340f), 180f, 1200f, "4.23", "");
      Worldbuilding.CreateCollider(new Vector2(6540f, -3060f), 720f, 420f, "4.22", "");
      Worldbuilding.CreateCollider(new Vector2(4920f, -3060f), 540f, 1620f, "4.21", "");
      Worldbuilding.CreateCollider(new Vector2(3180f, -3060f), 1140f, 1740f, "4.20", "");
      Worldbuilding.CreateCollider(new Vector2(4500f, -1920f), 60f, 420f, "4.19", "");
      Worldbuilding.CreateCollider(new Vector2(3840f, -1920f), 60f, 660f, "4.18", "");
      Worldbuilding.CreateStickyCeiling(new Vector2(3180f, -1920f), 660f);
      Worldbuilding.CreateCollider(new Vector2(6000f, -1020f), 900f, 600f, "4.11", "Water");
      Worldbuilding.CreateCollider(new Vector2(6600f, -1020f), 900f, 600f, "4.112", "");
      Worldbuilding.CreateCollider(new Vector2(5460f, -1200f), 180f, 540f, "4.10", "");
      Worldbuilding.CreateCollider(new Vector2(5460f, -1020f), 900f, 540f, "4.9", "");
      Worldbuilding.CreateCollider(new Vector2(5100f, -1020f), 900f, 360f, "4.8", "Water");
      Worldbuilding.CreateCollider(new Vector2(4980f, -1560f), 1440f, 120f, "4.7", "");
      Worldbuilding.CreateCollider(new Vector2(4680f, -1560f), 120f, 300f, "4.6", "Grass");
      Worldbuilding.CreateCollider(new Vector2(4140f, -1560f), 1440f, 360f, "4.5", "Grass");
      Worldbuilding.CreateCollider(new Vector2(4500f, -1440f), 360f, 60f, "4.4", "");
      Worldbuilding.CreateCollider(new Vector2(4500f, -1080f), 960f, 60f, "4.3", "");
      Worldbuilding.CreateCollider(new Vector2(4020f, -1560f), 1440f, 120f, "4.2", "");
      Worldbuilding.CreateCollider(new Vector2(4560f, -1080f), 960f, 420f, "4.1", "Water");
      this.levers.Add(new Lever("Tileset\\ButtonRight", new Vector2(6510f, -1230f), new Vector2(90f, 60f), "Button"));
      this.levers.Add(new Lever("Tileset\\Pulllever", new Vector2(5340f, -1140f), new Vector2(180f, 90f), "Pulllever"));
      this.levers.Add(new Lever("Tileset\\Lever", new Vector2(4730f, -1200f), new Vector2(80f, 120f), "Lever"));
      this.bars.Add(new Bar("Tileset\\Bar_Right", new Vector2(5580f, -1140f), new Vector2(420f, 120f), "right"));
      this.bars.Add(new Bar("Tileset\\Bar_Vertical", new Vector2(5100f, -1020f), new Vector2(120f, 420f), "up"));
      this.bars.Add(new Bar("Tileset\\Bar_Right", new Vector2(4020f, -1440f), new Vector2(420f, 120f), "right"));
      this.gates.Add(new Gate("Tileset\\Gate", new Vector2(4200f, -2100f), new Vector2(180f, 600f), "left", true));
      this.gateCollider.Add(new GateCollider("Tileset\\Barrier", new Vector2(4460f, -2100f), new Vector2(60f, 600f), this.levers[4]));
      Worldbuilding.CreateCollider(new Vector2(6540f, -1680f), 540f, 60f, "4.17", "");
      Worldbuilding.CreateCollider(new Vector2(5880f, -1620f), 240f, 660f, "4.16", "");
      Worldbuilding.CreateCollider(new Vector2(5880f, -2160f), 360f, 180f, "4.15", "");
      Worldbuilding.CreateCollider(new Vector2(5760f, -2160f), 780f, 120f, "4.14", "Grass");
      Worldbuilding.CreateCollider(new Vector2(5640f, -2160f), 780f, 120f, "4.13", "Grass");
      Worldbuilding.CreateCollider(new Vector2(5580f, -2160f), 360f, 60f, "4.13.1", "");
      Worldbuilding.CreateCollider(new Vector2(5760f, -1380f), 60f, 780f, "4.12", "");
      Worldbuilding.CreateClimbWall(new Vector2(6600f, -1680f), 540f);
      this.bars.Add(new Bar("Tileset\\Bar_Right", new Vector2(5580f, -1980f), new Vector2(420f, 120f), "right"));
      this.boxes.Add(new Box("Tileset\\Box", new Vector2(5880f, -1800f), new Vector2(175f, 175f), this.collidingEnvironment, this.monkey));
      Worldbuilding.CreateCollider(new Vector2(6420f, -1140f), -1f, 120f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(4560f, -1200f), -1f, 120f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(4560f, -1320f), -1f, 240f, typeof (Floaty));
      this.bars.Add(new Bar("Tileset\\Bar_Right", new Vector2(4440f, -1200f), new Vector2(420f, 120f), "left", BSPEED: 15f));
      Worldbuilding.CreateCollider(new Vector2(2880f, -3180f), 300f, 300f, "1", "");
      Worldbuilding.CreateCollider(new Vector2(2820f, -3180f), 60f, 60f, "1.5", "");
      Worldbuilding.CreateCollider(new Vector2(3060f, -2880f), 300f, 60f, "1s", "");
      Worldbuilding.CreateCollider(new Vector2(3120f, -2880f), 900f, 60f, "1f", "");
      Worldbuilding.CreateCollider(new Vector2(1740f, -3300f), 60f, 600f, "2", "");
      Worldbuilding.CreateCollider(new Vector2(1140f, -3540f), 360f, 360f, "3", "");
      Worldbuilding.CreateCollider(new Vector2(2340f, -3540f), 300f, 240f, "4", "");
      Worldbuilding.CreateCollider(new Vector2(780f, -4320f), 1860f, 240f, "5", "Grass");
      Worldbuilding.CreateCollider(new Vector2(720f, -4320f), 1860f, 60f, "6", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1020f, -4320f), 1740f, 60f, "7", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1080f, -4320f), 900f, 1500f, "8", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1080f, -3540f), 780f, 60f, "9", "");
      Worldbuilding.CreateCollider(new Vector2(720f, -2340f), 240f, 180f, "10", "");
      Worldbuilding.CreateCollider(new Vector2(900f, -2340f), 240f, 240f, "11", "");
      Worldbuilding.CreateClimbWall(new Vector2(1080f, -2880f), 420f);
      Worldbuilding.CreateStickyCeiling(new Vector2(1740f, -3240f), 1140f);
      this.levers.Add(new Lever("Tileset\\Timelever", new Vector2(2210f, -3390f), new Vector2(80f, 120f), "Timer"));
      this.gates.Add(new Gate("Tileset\\Gate", new Vector2(1020f, -4860f), new Vector2(180f, 600f), "right"));
      Worldbuilding.CreateCollider(new Vector2(3240f, -1260f), 1260f, 360f, "1", "Water");
      Worldbuilding.CreateCollider(new Vector2(3600f, -1260f), 1260f, 420f, "1.11", "");
      Worldbuilding.CreateCollider(new Vector2(2400f, -1620f), 900f, 420f, "2", "");
      Worldbuilding.CreateCollider(new Vector2(2820f, -1620f), 900f, 480f, "2.5", "Grass");
      Worldbuilding.CreateCollider(new Vector2(2160f, -1800f), 900f, 240f, "3", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1680f, -2220f), 1200f, 420f, "4", "Grass");
      Worldbuilding.CreateCollider(new Vector2(2040f, -2880f), 480f, 60f, "5", "");
      Worldbuilding.CreateCollider(new Vector2(1680f, -2760f), 120f, 120f, "6", "");
      Worldbuilding.CreateCollider(new Vector2(1620f, -2760f), 540f, 60f, "7", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1560f, -2760f), 1200f, 60f, "8", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1500f, -2640f), 1800f, 60f, "9", "");
      Worldbuilding.CreateCollider(new Vector2(1440f, -2640f), 300f, 60f, "10", "");
      Worldbuilding.CreateCollider(new Vector2(1440f, -1800f), 300f, 60f, "11", "");
      Worldbuilding.CreateClimbWall(new Vector2(2100f, -2220f), 420f);
      Worldbuilding.CreateClimbWall(new Vector2(1980f, -2880f), 480f);
      Worldbuilding.CreateClimbWall(new Vector2(1440f, -2340f), 540f);
      Worldbuilding.CreateCollider(new Vector2(-360f, -1800f), 900f, 1440f, "0", "Grass");
      Worldbuilding.CreateCollider(new Vector2(1080f, -1800f), 900f, 360f, "1", "");
      Worldbuilding.CreateCollider(new Vector2(-420f, -4680f), 2880f, 60f);
      Worldbuilding.CreateCollider(new Vector2(-360f, -4680f), 60f, 7200f);
      Worldbuilding.CreateCollider(new Vector2(360f, -2690f), 180f, 60f);
      Worldbuilding.CreateCollider(new Vector2(-360f, -2940f), 360f, 60f);
      Worldbuilding.CreateCollider(new Vector2(-60f, -3040f), 120f, 540f);
      Worldbuilding.CreateCollider(new Vector2(-60f, -3480f), 180f, 120f);
      Worldbuilding.CreateCollider(new Vector2(-120f, -3720f), 60f, 480f);
      Worldbuilding.CreateCollider(new Vector2(360f, -3780f), 120f, 180f);
      Worldbuilding.CreateCollider(new Vector2(-120f, -4380f), 60f, 300f);
      Worldbuilding.CreateCollider(new Vector2(-360f, -4060f), 120f, 120f);
      Worldbuilding.CreateClimbWall(new Vector2(-420f, -3660f), 180f);
      Worldbuilding.CreateClimbWall(new Vector2(-300f, -2940f), 360f);
      Worldbuilding.CreateStickyCeiling(new Vector2(-360f, -4620f), 660f);
      Worldbuilding.CreateStickyCeiling(new Vector2(-60f, -2930f), 540f);
      Worldbuilding.CreateStickyCeiling(new Vector2(-120f, -3660f), 660f);
      Worldbuilding.CreateStickyCeiling(new Vector2(-120f, -4320f), 300f);
      Worldbuilding.CreateCollider(new Vector2(-240f, -3300f), -1f, 180f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(-240f, -3480f), -1f, 180f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(-360f, -2160f), -1f, 180f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(0.0f, -1980f), -1f, 180f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(420f, -2580f), -1f, 300f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(600f, -3480f), -1f, 120f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(600f, -3600f), -1f, 120f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(540f, -3720f), -1f, 120f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(-360f, -4200f), -1f, 120f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(-360f, -4320f), -1f, 120f, typeof (Floaty));
      this.bars.Add(new Bar("Tileset\\Bar_Left", new Vector2(720f, -2280f), new Vector2(420f, 120f), "left"));
      this.bars.Add(new Bar("Tileset\\Bar_Right", new Vector2(-780f, -2580f), new Vector2(420f, 120f), "right"));
      this.bars.Add(new Bar("Tileset\\Bar_Left", new Vector2(720f, -3180f), new Vector2(420f, 120f), "left"));
      this.bars.Add(new Bar("Tileset\\Bar_Left", new Vector2(300f, -3360f), new Vector2(420f, 120f), "right", 130f));
      this.bars.Add(new Bar("Tileset\\Bar_Right", new Vector2(-1080f, -3300f), new Vector2(420f, 120f), "right"));
      this.bars.Add(new Bar("Tileset\\Bar_Left", new Vector2(840f, -4200f), new Vector2(420f, 120f), "left"));
      this.levers.Add(new Lever("Tileset\\ButtonLeft", new Vector2(-360f, -3420f), new Vector2(60f, 60f), "Button"));
      this.levers.Add(new Lever("Tileset\\ButtonRight", new Vector2(-120f, -3420f), new Vector2(60f, 60f), "Button"));
      Worldbuilding.CreateCollider(new Vector2(1620f, -3180f), -1f, 120f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(1140f, -2940f), -1f, 120f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(1140f, -2340f), -1f, 120f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(2340f, -2220f), -1f, 240f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(2460f, -2580f), -1f, 240f, typeof (Floaty));
      Worldbuilding.CreateCollider(new Vector2(2100f, -1860f), 720f, 60f);
      Worldbuilding.CreateCollider(new Vector2(1620f, -2220f), 720f, 60f);
      Worldbuilding.CreateCollider(new Vector2(1320f, -1800f), 600f, 60f);
      Worldbuilding.CreateCollider(new Vector2(-120f, -1560f), 1560f, 3360f);
      Worldbuilding.CreateCollider(new Vector2(2580f, -4320f), 1080f, 7380f, "", "Grass");
      this.gates.Add(new Gate("Tileset\\Gate", new Vector2(100020f, -4860f), new Vector2(180f, 600f), "right"));
      TutorialPrompt.ResetManager();
      this.tutorialPrompts.Add(new Rectangle(1700, 1460, 540, 600));
      TutorialPrompt tutorialPrompt1 = new TutorialPrompt(new Rectangle(1700, 1460, 540, 600), TutorialPrompts.Jump, Players.Gecko);
      this.tutorialPrompts.Add(new Rectangle(4540, 1180, 540, 120));
      TutorialPrompt tutorialPrompt2 = new TutorialPrompt(new Rectangle(4540, 1180, 540, 120), TutorialPrompts.Interact, Players.Monkey);
      this.tutorialPrompts.Add(new Rectangle(4590, 1590, 120, 90));
      TutorialPrompt tutorialPrompt3 = new TutorialPrompt(new Rectangle(4590, 1590, 120, 90), TutorialPrompts.Interact);
      this.tutorialPrompts.Add(new Rectangle(9690, -300, 120, 120));
      TutorialPrompt tutorialPrompt4 = new TutorialPrompt(new Rectangle(9690, -300, 120, 120), TutorialPrompts.Interact, Players.Monkey);
      this.tutorialPrompts.Add(new Rectangle(6450, -1290, 90, 120));
      TutorialPrompt tutorialPrompt5 = new TutorialPrompt(new Rectangle(6450, -1290, 90, 120), TutorialPrompts.Interact, Players.Gecko);
      this.monkey = new Monkey("Animations\\Monkey\\monkey_idle", new Vector2(-1700f, -5000f), new Vector2(100f, 160f), this.collidingEnvironment, this.boxes, this.levers, this.bars);
      this.gecko = new Gecko("Animations\\Gecko\\gecko_idle", new Vector2(-1600f, -5000f), new Vector2(80f, 100f), this.collidingEnvironment, this.levers, this.bars);
      this.bumerang = new Bumerang("Animations\\Gecko\\bumerang_idle", this.gecko.pos, new Vector2(40f, 40f), this.gecko, this.collidingEnvironment);
      this.target = new Target("Tileset\\Cursor", new Vector2(450f, 100f), new Vector2(30f, 30f));
      this.monkey.partner = this.gecko;
      this.gecko.partner = this.monkey;
      this.monkey.bumerang = this.bumerang;
      foreach (Sprite box in this.boxes)
        this.collidingEnvironment.Add(box);
      foreach (Sprite bar in this.bars)
        this.collidingEnvironment.Add(bar);
      foreach (Sprite lever in this.levers)
        this.environment.Add(lever);
      foreach (Sprite sprite in this.SpecialEnvironment)
        this.collidingEnvironment.Add(sprite);
      foreach (Sprite gate in this.gates)
        this.collidingEnvironment.Add(gate);
      foreach (Sprite sprite in this.gateCollider)
        this.collidingEnvironment.Add(sprite);
      this.collidingEnvironment.Add((Sprite) this.gecko);
      this.collidingEnvironment.Add((Sprite) this.monkey);
      using (StreamReader streamReader = new StreamReader(File.OpenRead(AppFolder.Path + "/" + "SaveData.txt")))
        this.saveData = int.Parse(streamReader.ReadLine());
      switch (this.saveData)
      {
        case 1:
          this.monkey.pos = new Vector2(0.0f, 500f);
          this.gecko.pos = new Vector2(0.0f, 600f);
          this.gateCount = 0;
          break;
        case 2:
          this.monkey.pos = new Vector2(this.gates[0].pos.X + 300f, this.gates[0].pos.Y + 300f);
          this.gecko.pos = new Vector2(this.gates[0].pos.X + 400f, this.gates[0].pos.Y + 300f);
          this.gateCount = 1;
          break;
        case 3:
          this.monkey.pos = new Vector2(this.gates[1].pos.X + 300f, this.gates[1].pos.Y + 300f);
          this.gecko.pos = new Vector2(this.gates[1].pos.X + 300f, this.gates[1].pos.Y + 300f);
          this.gateCount = 2;
          break;
        case 4:
          this.monkey.pos = new Vector2(this.gates[2].pos.X - 250f, this.gates[2].pos.Y + 300f);
          this.gecko.pos = new Vector2(this.gates[2].pos.X - 250f, this.gates[2].pos.Y + 300f);
          this.gateCount = 3;
          break;
        case 5:
          this.monkey.pos = new Vector2(this.gates[3].pos.X - 250f, this.gates[3].pos.Y + 300f);
          this.gecko.pos = new Vector2(this.gates[3].pos.X - 250f, this.gates[3].pos.Y + 300f);
          this.gateCount = 4;
          break;
        case 6:
          this.monkey.pos = new Vector2(this.gates[4].pos.X + 300f, this.gates[4].pos.Y + 300f);
          this.gecko.pos = new Vector2(this.gates[4].pos.X + 300f, this.gates[4].pos.Y + 300f);
          this.gateCount = 5;
          this.exitCave = true;
          break;

        // RnD ("Secret level 7?")
        case 7:
            this.monkey.pos = new Vector2(this.gates[4].pos.X + 300f, this.gates[4].pos.Y + 300f);
            this.gecko.pos = new Vector2(this.gates[4].pos.X + 300f, this.gates[4].pos.Y + 300f);
            this.gateCount = 6;
            this.exitCave = true;
            break;

        // RnD ("Secret level 8?")
        case 8:
            this.monkey.pos = new Vector2(this.gates[4].pos.X + 300f, this.gates[4].pos.Y + 300f);
            this.gecko.pos = new Vector2(this.gates[4].pos.X + 300f, this.gates[4].pos.Y + 300f);
            this.gateCount = 7;
            this.exitCave = true;
            break;
      }

      if (this.StartSequence || this.saveData > 4 || this.saveData < 1)
        return;
      Globals.audioManager.PlaySoundSong(Globals.audioManager.soundSongs[string.Format("CaveTheme_{0}", (object) this.saveData)], Globals.audioManager.soundSongs[string.Format("CaveTheme_{0}_Loop", (object) this.saveData)]);
    }

    private void SetRadialSounds()
    {
      this.radialSounds = new List<RadialSound>();
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Water_Run"], new Vector2(8340f, 720f), 1100f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Waterfall_Small"], new Vector2(2000f, -3000f), 500f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Waterfall_Mid"], new Vector2(120f, -4720f), 1500f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Waterfall_Ground_1"], new Vector2(300f, -1880f), 1000f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Waterfall_Ground_2"], new Vector2(300f, -1880f), 900f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Waterfall_Top"], new Vector2(120f, -3860f), 3000f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Radial_1"], new Vector2(3000f, 1360f), 3000f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Radial_2"], new Vector2(5660f, -1800f), 1000f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Radial_3"], new Vector2(2880f, -1800f), 1400f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Water_Run"], new Vector2(2580f, -2580f), 1000f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Radial_2"], new Vector2(780f, -4320f), 700f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Waterfall_Ground_2"], new Vector2(5600f, -1340f), 900f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Waterfall_Ground_1"], new Vector2(600f, -3600f), 1500f));
      this.radialSounds.Add(new RadialSound(Globals.audioManager.sounds["Waterfall_Ground_2"], new Vector2(600f, -3600f), 1500f));
    }

    private void SetWorldTextures()
    {
      this.worldTexturePaths = new List<string[,,]>();
      this.worldTexturePaths.Add(new string[4, 2, 1]
      {
        {
          {
            "Stage\\Stage1-2\\stage_1-2_0_(1,0)"
          },
          {
            "Stage\\Stage1-2\\stage_1-2_0_(1,1)"
          }
        },
        {
          {
            "Stage\\Stage1-2\\stage_1-2_1_(1,0)"
          },
          {
            "Stage\\Stage1-2\\stage_1-2_1_(1,1)"
          }
        },
        {
          {
            "Stage\\Stage1-2\\stage_1-2_2_(1,0)"
          },
          {
            "Stage\\Stage1-2\\stage_1-2_2_(1,1)"
          }
        },
        {
          {
            "Stage\\Stage1-2\\stage_1-2_3_(1,0)"
          },
          {
            "Stage\\Stage1-2\\stage_1-2_3_(1,1)"
          }
        }
      });
      for (int index = 2; index <= 7; ++index)
        this.worldTexturePaths.Add(new string[4, 2, 2]
        {
          {
            {
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_0_(0,0)",
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_0_(0,1)"
            },
            {
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_0_(1,0)",
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_0_(1,1)"
            }
          },
          {
            {
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_1_(0,0)",
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_1_(0,1)"
            },
            {
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_1_(1,0)",
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_1_(1,1)"
            }
          },
          {
            {
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_2_(0,0)",
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_2_(0,1)"
            },
            {
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_2_(1,0)",
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_2_(1,1)"
            }
          },
          {
            {
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_3_(0,0)",
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_3_(0,1)"
            },
            {
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_3_(1,0)",
              "Stage\\Stage" + index.ToString() + "\\stage_" + index.ToString() + "_3_(1,1)"
            }
          }
        });
      this.worldTexturePaths.Add(new string[4, 1, 4]
      {
        {
          {
            "Stage\\Stage8\\stage_8_0_(0,0)",
            "Stage\\Stage8\\stage_8_0_(0,1)",
            "Stage\\Stage8\\stage_8_0_(1,0)",
            "Stage\\Stage8\\stage_8_0_(1,1)"
          }
        },
        {
          {
            "Stage\\Stage8\\stage_8_1_(0,0)",
            "Stage\\Stage8\\stage_8_1_(0,1)",
            "Stage\\Stage8\\stage_8_1_(1,0)",
            "Stage\\Stage8\\stage_8_1_(1,1)"
          }
        },
        {
          {
            "Stage\\Stage8\\stage_8_2_(0,0)",
            "Stage\\Stage8\\stage_8_2_(0,1)",
            "Stage\\Stage8\\stage_8_2_(1,0)",
            "Stage\\Stage8\\stage_8_2_(1,1)"
          }
        },
        {
          {
            "Stage\\Stage8\\stage_8_3_(0,0)",
            "Stage\\Stage8\\stage_8_3_(0,1)",
            "Stage\\Stage8\\stage_8_3_(1,0)",
            "Stage\\Stage8\\stage_8_3_(1,1)"
          }
        }
      });
      this.worldTextures = new List<WorldTexture>();
      this.worldTextures.Add(new WorldTexture(this.worldTexturePaths[0], new Vector2(-2274f, -4593f), new Vector2(2522f, 5808f)));
      this.worldTextures.Add(new WorldTexture(this.worldTexturePaths[1], new Vector2(-1000f, -460f), new Vector2(5270f, 2970f)));
      this.worldTextures.Add(new WorldTexture(this.worldTexturePaths[2], new Vector2(3529f, -513f), new Vector2(4297f, 2944f)));
      this.worldTextures.Add(new WorldTexture(this.worldTexturePaths[3], new Vector2(7144f, -2505f), new Vector2(3070f, 3666f)));
      this.worldTextures.Add(new WorldTexture(this.worldTexturePaths[4], new Vector2(4115f, -2884f), new Vector2(3683f, 2176f)));
      this.worldTextures.Add(new WorldTexture(this.worldTexturePaths[5], new Vector2(871f, -3757f), new Vector2(3645f, 2804f)));
      this.worldTextures.Add(new WorldTexture(this.worldTexturePaths[6], new Vector2(-903f, -4859f), new Vector2(2297f, 3362f)));
      this.worldTextures.Add(new WorldTexture(this.worldTexturePaths[7], new Vector2(1074f, -4909f), new Vector2(6544f, 871f)));
    }

    public override void Update(float deltaSeconds)
    {
      StorageFolder AppFolder = ApplicationData.Current.LocalFolder;

      if (this.StartSequence && this.monkey.previousCollisionBottom)
      {
        Globals.audioManager.PlaySoundSong(Globals.audioManager.soundSongs["CaveTheme_1"], Globals.audioManager.soundSongs["CaveTheme_1_Loop"]);
        this.StartSequence = false;
        Tutorial.fallingInstance.Stop();
      }
      if (Globals.keyboard.IsKeyDown(Keys.RightControl) && !Globals.previousKeyboard.IsKeyDown(Keys.RightControl))
        this.perspective = !this.perspective;
      Globals.nextGate = this.gates[this.gateCount];
      foreach (Sprite box in this.boxes)
        box.Update(deltaSeconds);
      foreach (Sprite bar in this.bars)
        bar.Update(deltaSeconds);
      this.gecko.Update(deltaSeconds);
      this.monkey.Update(deltaSeconds);
      this.target.Update(deltaSeconds);
      this.bumerang.Update(deltaSeconds);
      this.gates[this.gateCount].Update(deltaSeconds);
      if (this.gates[this.gateCount].activated)
      {
        this.gecko.sequence = false;
        this.monkey.sequence = false;
        if ((double) this.gates[this.gateCount].timer < 5.0 && (double) this.gates[this.gateCount].timer > 4.6999998092651367)
        {
          this.gateOpenInstance.Play();
          this.gates[this.gateCount].OpenGate();
          if (this.gateCount == 3)
            Globals.audioManager.FadeOutSoundSong(this.gates[this.gateCount].timer);
        }
        if ((double) this.gates[this.gateCount].timer < 1.5 && (double) this.gates[this.gateCount].timer > 1.0)
        {
          this.gateCloseInstance.Play();
          this.gates[this.gateCount].CloseGate();
        }
        if (this.gates[this.gateCount].type == "right" && (double) this.gates[this.gateCount].timer < 3.5 && (double) this.gates[this.gateCount].timer > 1.5)
        {
          this.gecko.pos.X += 150f * deltaSeconds;
          this.gecko.animationManager.PlayAnimation(this.gecko.animations["Run"], SpriteEffects.None);
          this.gecko.sequence = true;
          this.monkey.pos.X += 170f * deltaSeconds;
          this.monkey.animationManager.PlayAnimation(this.monkey.animations["Run"], SpriteEffects.None);
          this.monkey.sequence = true;
        }
        else if (this.gates[this.gateCount].type == "left" && (double) this.gates[this.gateCount].timer < 3.5 && (double) this.gates[this.gateCount].timer > 1.5)
        {
          this.gecko.pos.X -= 150f * deltaSeconds;
          this.gecko.animationManager.PlayAnimation(this.gecko.animations["Run"], SpriteEffects.FlipHorizontally);
          this.gecko.sequence = true;
          this.monkey.pos.X -= 170f * deltaSeconds;
          this.monkey.animationManager.PlayAnimation(this.monkey.animations["Run"], SpriteEffects.FlipHorizontally);
          this.monkey.sequence = true;
        }
        this.gecko.stunned = true;
        this.monkey.stunned = true;

        if ((double) this.gates[this.gateCount].timer <= 0.0)
        {
          ++this.gateCount;
          Globals.nextGate = this.gates[this.gateCount];
          this.gecko.stunned = false;
          this.monkey.stunned = false;

          using (StreamWriter streamWriter = new StreamWriter(File.OpenRead(AppFolder.Path + "/" + "SaveData.txt")))
            streamWriter.Write(this.gateCount + 1);

          if (this.gateCount == 5)
          {
            Globals.audioManager.PlayTheme(Globals.audioManager.songs["ExitCave"], true);
            this.exitCave = true;
          }
          else if (this.gateCount == 4)
            Globals.audioManager.StopSoundSong();
          else
            Globals.audioManager.Transition(Globals.audioManager.soundSongs[string.Format("CaveTheme_{0}", 
                (object) (this.gateCount + 1))], Globals.audioManager.soundSongs[string.Format("CaveTheme_{0}_Loop",
                (object) (this.gateCount + 1))]);
        }
      }

      if (this.exitCave)
        this.exitReach = (float) (((double) Camera.mid.X - 1020.0) / 5200.0);
      
      this.CheckMechanisms();
      
      foreach (Sprite lever in this.levers)
        lever.Update(deltaSeconds);
      foreach (Sprite sprite in this.gateCollider)
        sprite.Update(deltaSeconds);
      foreach (RadialSound radialSound in this.radialSounds)
        radialSound.Update();
      base.Update(deltaSeconds);
      if (Globals.IsSequenceActive || !this.gecko.InputController.TogglePause() 
                && !this.monkey.InputController.TogglePause())
        return;
      this.stateManager.StartState((State) new Pause(this.stateManager), false);
      MediaPlayer.Pause();
    }

    public override void Draw()
    {
      for (int layer = 0; layer < 2; ++layer)
      {
        for (int index = 0; index < this.worldTextures.Count<WorldTexture>(); ++index)
          this.worldTextures[index].Draw(layer);
      }
      foreach (Sprite sprite in this.environment)
        sprite.Draw();
      foreach (Sprite sprite in this.collidingEnvironment)
      {
        switch (sprite)
        {
          case Monkey _:
          case Gecko _:
            continue;
          default:
            sprite.Draw();
            continue;
        }
      }
      for (int index = 0; index < this.worldTextures.Count<WorldTexture>(); ++index)
        this.worldTextures[index].Draw(2);
      this.monkey.Draw();
      this.gecko.Draw();
      foreach (WorldTexture worldTexture in this.worldTextures)
        worldTexture.Draw(3);
      if (Target.active)
        this.target.Draw();
      if (Bumerang.loose && !this.monkey.holdOn)
        this.bumerang.Draw();
      Globals.spriteBatch.Draw(Globals.rect, new Rectangle(5100, -820, 300, 300), Color.Black);
      foreach (Sprite sprite in this.foreEnvironment)
        sprite.Draw();
      if (this.exitCave)
      {
        MediaPlayer.Volume = this.exitReach;
        if ((double) this.exitReach >= 1.0)
          this.stateManager.StartState((State) new Titlescreen(this.stateManager, true));
      }
      base.Draw();
      GateAnimation.DrawManager(Globals.spriteBatch, Globals.DrawTime);
      TutorialPrompt.DrawManager(Globals.spriteBatch, Globals.DrawTime);
    }

    private void CheckMechanisms()
    {
      if (this.levers[0].activated && !this.levers[0].previousActivated)
      {
        this.gates[1].UnlockGate();
        Camera.Highlight(this.gates[1].pos, 2f);
      }
      if (this.levers[1].pullActivation)
      {
        this.bars[0].getPulled = true;
        this.levers[1].pullActivation = false;
        Camera.Highlight(new Vector2(9300f, -360f), 2f);
      }
      if (this.levers[2].pullActivation)
      {
        this.bars[4].getPulled = true;
        this.bars[5].getPulled = true;
        this.levers[2].pullActivation = false;
        Camera.Highlight(new Vector2(5200f, -1880f), 2f, 0.7f);
      }
      if (this.levers[3].pullActivation)
      {
        this.bars[1].getPulled = true;
        this.bars[2].getPulled = true;
        this.bars[3].getPulled = true;
        this.levers[3].pullActivation = false;
        Camera.Highlight(new Vector2(5200f, -1880f), 2f, 0.7f);
      }
      if (this.levers[4].activated && !this.levers[4].previousActivated)
      {
        this.gates[3].UnlockGate();
        Camera.Highlight(new Vector2(4500f, -1600f), 2f);
      }
      if (this.levers[5].timerActivation)
      {
        this.bars[6].getPulled = true;
        this.bars[7].getPulled = true;
        this.bars[10].getPulled = true;
        this.bars[11].getPulled = true;
      }
      if (this.levers[5].timerActivation && !this.levers[5].previousTimerActivation)
        Camera.Highlight(new Vector2(15f, -4332f), 10f, 0.3166f);
      if (this.levers[6].pullActivation)
      {
        this.bars[9].getPulled = true;
        this.levers[6].pullActivation = false;
      }
      if (!this.levers[7].pullActivation)
        return;
      this.bars[8].getPulled = true;
      this.levers[7].pullActivation = false;
    }
  }
}
