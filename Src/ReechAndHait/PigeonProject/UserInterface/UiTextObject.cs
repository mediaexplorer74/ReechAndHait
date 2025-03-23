
// Type: PigeonProject.UserInterface.UiTextObject
// Assembly: PigeonProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ED64DA4-BF9A-4998-A25D-EAF7C565AAB8
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.ContentManagement;
using PigeonProject.Objects;

#nullable disable
namespace PigeonProject.UserInterface
{
  public class UiTextObject : UiElement
  {
    private string _text;
    private SpriteFont _font;
    private TextAlignment _alignment;
    private float _lineWidth = -1f;
    private Vector2 offset = Vector2.Zero;

    public string Text
    {
      get => this._text;
      set
      {
        this._text = value;
        this.TransformWidth = (int) this.Font.MeasureString(this._text).X;
        this.TransformHeight = (int) this.Font.MeasureString(this._text).Y;
      }
    }

    public SpriteFont Font
    {
      get => this._font;
      set
      {
        this._font = value;
        this.TransformWidth = (int) this.Font.MeasureString(this._text).X;
        this.TransformHeight = (int) this.Font.MeasureString(this._text).Y;
        this.LineWidth = this._lineWidth;
      }
    }

    public TextAlignment Alignment
    {
      get => this._alignment;
      set
      {
        this._alignment = value;
        switch (this._alignment)
        {
          case TextAlignment.Centered:
            this.offset = this.Transform.Size.ToVector2() / 2f;
            break;
          case TextAlignment.Left:
            this.offset = Vector2.Zero;
            break;
          case TextAlignment.Right:
            this.offset = this.Transform.Size.ToVector2();
            break;
        }
      }
    }

    public float LineWidth
    {
      get => this._lineWidth;
      set
      {
        if ((double) value > 0.0)
        {
          this._lineWidth = value;
          this.TransformWidth = (int) this._lineWidth;
        }
        else
        {
          this._lineWidth = -1f;
          this.TransformWidth = (int) this.Font.MeasureString(this.Text).X;
        }
      }
    }

    public UiTextObject(
      string text,
      SpriteFont font,
      Vector2 relativePosition,
      GameObjectManager uiElementManager,
      TextAlignment alignment = TextAlignment.Centered)
      : base(relativePosition, (Texture2D) null, (int) font.MeasureString(text).X, (int) font.MeasureString(text).Y, uiElementManager)
    {
      this._text = text;
      this._font = font;
      this.Alignment = alignment;
    }

    public UiTextObject(
      string text,
      SpriteFont font,
      Vector2 relativePosition,
      PigeonProject.UserInterface.UiCollection uiCollection,
      GameObjectManager uiElementManager,
      TextAlignment alignment = TextAlignment.Centered)
      : base(relativePosition, (Texture2D) null, (int) font.MeasureString(text).X, (int) font.MeasureString(text).Y, (IUiCollection) uiCollection, uiElementManager)
    {
      this._text = text;
      this._font = font;
      this.Alignment = alignment;
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
      base.Draw(spriteBatch, gameTime);
      if (!this.IsVisible)
        return;
      if ((double) this.LineWidth <= 0.0)
        spriteBatch.DrawString(this.Font, this.Text, this.Position - this.offset, this._realColor);
      else
        spriteBatch.DrawString(this.Font, TextContentHandler.WrapText(this.Font, this.Text, this.LineWidth), this.Position - this.offset, this._realColor);
    }
  }
}
