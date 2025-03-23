
// Type: ReachHigh.Shared.DialogManager
// Assembly: ReachHigh.Windows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7AAE051-E6AD-4505-A6E0-970B36D3CBD7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PigeonProject.ContentManagement;
using System.IO;
using Windows.Globalization;

#nullable disable
namespace ReachHigh.Shared
{
  public class DialogManager
  {
    private string text;
    private string output;
    private string[] lines;
    private int lineCount;
    private float charCount;
    public bool hasEnded;
    public bool active;
    public bool finishLine;
    private Texture2D dialogbox;
    private readonly InputController geckoInput;
    private readonly InputController monkeyInput;

    public DialogManager(Gecko gecko, Monkey monkey)
    {
      this.geckoInput = gecko.InputController;
      this.monkeyInput = monkey.InputController;
      this.dialogbox = Globals.content.Load<Texture2D>("UI\\dialoguebox");
      this.Reset();
    }

    public void Update(float deltaSeconds, string type)
    {
      if (this.text == null)
      {
        switch (type)
        {
          case "Tutorial_Intro":
            using (StreamReader streamReader = new StreamReader(
                File.OpenRead("Content\\Text\\Dialog_Tutorial_Intro.txt")))
            {
              this.text = streamReader.ReadToEnd();
              break;
            }
        }
        this.active = true;
        this.lines = this.text.Split('|');
      }
      if (this.finishLine && (this.geckoInput.DialogueContinue() && this.lines[this.lineCount][2] == '!' 
                || this.monkeyInput.DialogueContinue() && this.lines[this.lineCount][2] == '#'))
      {
        ++this.lineCount;
        this.charCount = 1f;
        if (this.lineCount == this.lines.Length)
        {
          this.hasEnded = true;
          return;
        }
      }
      this.charCount += deltaSeconds * 30f;
      if ((double) this.charCount <= (double) (this.lines[this.lineCount].Substring(3).Length + 1))
      {
        this.output = this.lines[this.lineCount].Substring(3, (int) this.charCount);
        this.finishLine = false;
      }
      else
      {
        this.charCount -= deltaSeconds * 30f;
        this.finishLine = true;
      }
    }

    public void Draw(Vector2 characterPosOne, Vector2 characterPosTwo)
    {
      if (this.lines[this.lineCount][2] == '!')
      {
        Globals.spriteBatch.Draw(this.dialogbox, new Rectangle((int) characterPosOne.X, (int) characterPosOne.Y - 200, this.dialogbox.Width, this.dialogbox.Height), new Rectangle?(), Color.White, 0.0f, new Vector2((float) (this.dialogbox.Width / 2), (float) (this.dialogbox.Height / 2)), SpriteEffects.None, 0.0f);
        Globals.spriteBatch.DrawString(Globals.menuFont, this.output, new Vector2(characterPosOne.X - 50f, characterPosOne.Y - 200f), new Color(145, 182, 224), 0.0f, new Vector2((float) (this.dialogbox.Width / 2), (float) (this.dialogbox.Height / 2)), 0.5f, SpriteEffects.None, 0.0f);
      }
      else
      {
        if (this.lines[this.lineCount][2] != '#')
          return;
        Globals.spriteBatch.Draw(this.dialogbox, new Rectangle((int) characterPosTwo.X, (int) characterPosTwo.Y - 200, this.dialogbox.Width, this.dialogbox.Height), new Rectangle?(), Color.White, 0.0f, new Vector2((float) (this.dialogbox.Width / 2), (float) (this.dialogbox.Height / 2)), SpriteEffects.None, 0.0f);
        Globals.spriteBatch.DrawString(Globals.menuFont, this.output, new Vector2(characterPosTwo.X - 50f, characterPosTwo.Y - 200f), new Color(183, 155, 197), 0.0f, new Vector2((float) (this.dialogbox.Width / 2), (float) (this.dialogbox.Height / 2)), 0.5f, SpriteEffects.None, 0.0f);
      }
    }

    public void Reset()
    {
      this.hasEnded = false;
      this.text = (string) null;
      this.lines = (string[]) null;
      this.lineCount = 0;
      this.active = false;
      this.charCount = 1f;
    }
  }
}
