using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;

namespace SAE12
{
    public class Player
    {
        public Vector2 _pos;
        private AnimatedSprite[] _playerSprite;
        private SpriteSheet _sheet;

        private float moveSpeed = 1.5f;
        public bool isIdle = false;
        public bool interaction;
        public bool porte;
        public bool move;
        public bool map3;

        public string animation = "";

        public Player(Vector2 posP)
        {
            _playerSprite = new AnimatedSprite[10];
            _pos= posP;
            interaction = false;
            porte = false;
            map3 = false;
        }
        public void Load(SpriteSheet[] spriteSheets)
        {
            for (int i = 0; i < spriteSheets.Length; i++)
            {
                _sheet = spriteSheets[i];
                _playerSprite[i] = new AnimatedSprite(_sheet);
            }
 
        }

       
        public void Update(GameTime gameTime, TiledMapTileLayer mapLayer, TiledMapTileLayer mapLayer2,TiledMapTileLayer mapLayer3 ,int TileSize, bool move)
        {
            isIdle = true;
            interaction = false;
            porte = false;
            map3 = false;
            
            Vector2 _posAfter = _pos;

            _playerSprite[0].Play("idleDown");

            var keyboardstate = Keyboard.GetState();
            if (keyboardstate.IsKeyDown(Keys.D))
            {
                
                animation = "walkRight";
                _posAfter.X += moveSpeed;
                isIdle = false;
            }
            if (keyboardstate.IsKeyDown(Keys.Q))
            {
                animation = "walkLeft";
                _posAfter.X -= moveSpeed;
                isIdle = false;
            }
            if (keyboardstate.IsKeyDown(Keys.Z))
            {
                animation = "walkUp";
                _posAfter.Y -= moveSpeed;
                isIdle = false;
            }
            if (keyboardstate.IsKeyDown(Keys.S))
            {
                animation = "walkDown";
                _posAfter.Y += moveSpeed;
                isIdle = false;
            }
            if (!isIdle)
            {
                _playerSprite[1].Play(animation);
                _playerSprite[1].Update(gameTime);
            }
            //collisions
            if(!IsCollision(_posAfter,mapLayer,TileSize)&& move)
            {
                _pos = _posAfter;
            }
            //interractions
            if (IsCollision(_pos, mapLayer2, TileSize))
            {
                interaction = true;
            }
            //changements de maps trigger
            if(IsCollision(_pos, mapLayer3,TileSize))
            {
                porte = true;
                map3 = true;
            }

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
            
            if (isIdle)
                spriteBatch.Draw(_playerSprite[0], _pos);
            else
                spriteBatch.Draw(_playerSprite[1], _pos);

            spriteBatch.End();
        }
        private bool IsCollision(Vector2 position, TiledMapTileLayer mapLayer,int TileSize)
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
