
// Type: PigeonProject.Objects.GameObject
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.Animation;
using PigeonProject.ContentManagement;
using System;

#nullable disable
namespace PigeonProject.Objects
{
  public class GameObject : IGameObject
  {
    protected float _layerDepth = 0.75f;
    private float _opacity = 1f;
    private Rectangle _transform;
    private Vector2 _position;
    private Vector2 _direction = GameObject.standardDirection;
    private CollisionEffect CollisionEnterEffect;
    private float realRotation;
    protected Color _realColor = Color.White;
    protected static readonly Vector2 standardDirection = new Vector2(0.0f, -1f);

    public GameObjectManager GameObjectManager { get; private set; }

    public HitBox HitBox { get; private set; }

    public FrameAnimationHandler FrameAnimationHandler { get; set; }

    public SpriteDescriptor SpriteDescriptor { get; set; }

    public Texture2D Sprite { get; set; }

    public float SpriteRotation { get; set; }

    public float SpriteRealRotation => this.realRotation;

    public bool ApplyDirectionRotation { get; set; } = true;

    public Color Color { get; set; } = Color.White;

    public bool IsVisible { get; set; } = true;

    public virtual float LayerDepth
    {
      get => (float) (((double) this._layerDepth - 0.5) * 2.0);
      set => this._layerDepth = (float) (0.5 + (double) MathHelper.Clamp(value, 0.0f, 1f) * 0.5);
    }

    public float Opacity
    {
      get => this._opacity;
      set => this._opacity = MathHelper.Clamp(value, 0.0f, 1f);
    }

    public Rectangle Transform
    {
      get => this._transform;
      set
      {
        this._transform = value;
        this._position = this.Transform.Center.ToVector2();
      }
    }

    public int TransformWidth
    {
      set => this._transform.Width = value;
    }

    public int TransformHeight
    {
      set => this._transform.Height = value;
    }

    public virtual Vector2 Position
    {
      get => this._position;
      set
      {
        this._position = value;
        this._transform.Location = this._position.ToPoint() - new Point(this.Transform.Width / 2, this.Transform.Height / 2);
      }
    }

    public int PositionX
    {
      set => this.Position = new Vector2((float) value, this.Position.Y);
    }

    public int PositionY
    {
      set => this.Position = new Vector2(this.Position.X, (float) value);
    }

    public Vector2 Direction
    {
      get => this._direction;
      set
      {
        this._direction = value;
        this._direction.Normalize();
      }
    }

    public double LifeTime { get; set; } = -1.0;

    public string Tag { get; set; }

    public GameObject(
      Rectangle transform,
      Texture2D sprite,
      GameObjectManager manager,
      bool registerHitbox = false)
    {
      this.Transform = transform;
      this.Sprite = sprite;
      this.GameObjectManager = manager;
      this.GameObjectManager.RegisterGameObject((IGameObject) this);
      if (!registerHitbox)
        return;
      this.RegisterHitBox(transform.Width / 2);
    }

    public GameObject(
      Vector2 position,
      int width,
      int height,
      Texture2D sprite,
      GameObjectManager manager,
      bool registerHitbox = false)
      : this(new Rectangle((int) ((double) position.X - (double) (width / 2)), (int) ((double) position.Y - (double) (height / 2)), width, height), sprite, manager, registerHitbox)
    {
    }

    public GameObject(
      Vector2 position,
      int width,
      int height,
      Texture2D sprite,
      GameObjectManager manager,
      string animationkey,
      string collectionKey,
      bool registerHitbox = false)
      : this(position, width, height, sprite, manager, registerHitbox)
    {
      this.RegisterAnimationHandler(animationkey, collectionKey);
    }

    public void RegisterHitBox(int radius, bool isStatic = false)
    {
      if (this.GameObjectManager.HitBoxManager == null)
        return;
      this.HitBox = new HitBox(this, radius, this.GameObjectManager.HitBoxManager, isStatic);
    }

    public void RegisterAnimationHandler(string animationKey, string collectionKey)
    {
      this.FrameAnimationHandler = new FrameAnimationHandler(animationKey, collectionKey, this.GameObjectManager.FrameAnimationManager);
    }

    public virtual void Update(GameTime gameTime)
    {
      this.realRotation = this.ApplyDirectionRotation ? (float) Math.Acos((double) Vector2.Dot(GameObject.standardDirection, this.Direction)) : 0.0f;
      if ((double) this.Direction.X < 0.0)
        this.realRotation = -this.realRotation;
      this.realRotation += this.SpriteRotation;
      if (this.LifeTime == -1.0)
        return;
      this.LifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
      if (this.LifeTime > 0.0)
        return;
      this.Delete();
    }

    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      if (!this.IsVisible)
        return;
      this._realColor = this.Color * this.Opacity;
      Rectangle destinationRectangle = new Rectangle(this.Position.ToPoint(), this.Transform.Size);
      Rectangle rectangle = this.Sprite != null ? this.Sprite.Bounds : Rectangle.Empty;
      Texture2D texture;
      if (this.FrameAnimationHandler != null)
      {
        Rectangle sourceRectangle;
        texture = this.FrameAnimationHandler.GetCurrentFrameImage(out sourceRectangle);
        rectangle = sourceRectangle.IsEmpty ? texture.Bounds : sourceRectangle;
      }
      else if (this.SpriteDescriptor != null && this.SpriteDescriptor.SpriteSheet != null)
      {
        texture = this.SpriteDescriptor.SpriteSheet.Texture;
        rectangle = this.SpriteDescriptor.SpriteSheet.GetSourceRectangle(this.SpriteDescriptor.Index);
      }
      else
        texture = this.Sprite;
      if (texture == null)
        return;
      spriteBatch.Draw(texture, destinationRectangle, new Rectangle?(rectangle), this._realColor, this.realRotation, new Vector2((float) (rectangle.Width / 2), (float) (rectangle.Height / 2)), SpriteEffects.None, this._layerDepth);
    }

    public virtual void OnCollisionEnter(GameObject other, Vector2 collisionDirection)
    {
      CollisionEffect collisionEnterEffect = this.CollisionEnterEffect;
      if (collisionEnterEffect == null)
        return;
      collisionEnterEffect(other, collisionDirection);
    }

    public virtual void OnCollision(GameObject other, Vector2 collisionDirection)
    {
    }

    public virtual void OnCollisionExit(GameObject other, Vector2 collisionDirection)
    {
    }

    public virtual bool Delete()
    {
      if (this.HitBox != null)
        this.HitBox.Delete();
      return this.GameObjectManager.RemoveGameObject((IGameObject) this);
    }

    public void AddCollisionEnterEffect(CollisionEffect effect)
    {
      this.CollisionEnterEffect = effect;
    }
  }
}
