using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;

namespace SAE12
{
    public class Monstre1
    {
        public Vector2 _pos;
        public Vector2 _posFutur;
        private AnimatedSprite[] _sprites;
        private SpriteSheet _sheet;

        public bool isIdle = false;
        public bool interaction;
        public bool porte;
       
        public Monstre1(Vector2 posP)
        {
            _sprites = new AnimatedSprite[10];
            _pos = posP;
            interaction = false;
            porte = false;
        }
        public void Load(SpriteSheet[] spriteSheets)
        {
            for (int i = 0; i < spriteSheets.Length; i++)
            {
                _sheet = spriteSheets[i];
                _sprites[i] = new AnimatedSprite(_sheet);
            }
        }

        public void Update(GameTime gameTime, TiledMapTileLayer mapLayer, TiledMapTileLayer mapLayer2, TiledMapTileLayer mapLayer3, int TileSize, Player perso, Vector2 poseParametre, int who)
        {
            if (who == 1)
                _sprites[0].Play("idle_R");
            else if (who == 2)
                _sprites[0].Play("garde_idle");
            else
                _sprites[1].Play("idle");

            isIdle = true;
            interaction = false;
            porte = false;
            _posFutur = _pos;
            _pos = poseParametre;
            string animation = "";

            if (who == 1)
            {
                if (_posFutur.X == _pos.X - 1)
                    animation = "walk_R";
                else if (_posFutur.X == _pos.X + 1)
                    animation = "walk_L";
                else if (_posFutur.Y == _pos.Y - 1)
                    animation = "walk_down";
                else if (_posFutur.Y == _pos.Y + 1)
                    animation = "walk_up";
                else
                    animation = "idle_R";
            }
            else if (who == 2)
                animation = "garde_idle";


            if (who == 1)
                _sprites[1].Play(animation);

            _sprites[1].Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch, Matrix matrix)

        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                samplerState: SamplerState.PointClamp,
                effect: null,
                blendState: null,
                rasterizerState: null,
                depthStencilState: null,
                transformMatrix: matrix);

            spriteBatch.Draw(_sprites[1], _pos);

            spriteBatch.End();
        }
        private bool IsCollision(Vector2 position, TiledMapTileLayer mapLayer, int TileSize)
        {
            ushort posx = (ushort)(position.X / TileSize);
            ushort posy = (ushort)(position.Y / TileSize);
            TiledMapTile? tile;
            if (mapLayer.TryGetTile(posx, posy, out tile) == false)
                return false;
            if (!tile.Value.IsBlank)
                return true;
            return false;
        }

    }
}
