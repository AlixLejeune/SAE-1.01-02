using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace SAE12
{

    public class Texte
    {

        public SpriteFont _txt;
        public Texture2D _box;

        double wait = 2;
        public string text;
        public string fin = "";
        int count = 0;
        public bool fini = false;


        public Texte()
        {

        }

        public void Load(SpriteFont _txtBox, string contenu)
        {

            _txt = _txtBox;
            this.text = contenu;

            this.fin = "";
            this.count = 0;

        }

        public void Update(GameTime gameTime, SoundEffect son, double temp)
        {

            //écrit
            if (wait <= 0 && count < text.Length)
            {
                fin += text[count];

                if (text[count] == ',' || text[count] == '!' || text[count] == '.')
                    wait = 10;
                else
                    wait = temp;

                count++;

                son.Play();

            }

            if (count >= text.Length)
                fini = true;

            wait--;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {

            spriteBatch.Begin();
            spriteBatch.DrawString(_txt, fin, new Vector2(x, y), Color.White);
            spriteBatch.End();
        }

    }
}