using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;

namespace SAE12
{
    public class Boss
    {
        public Vector2 _pos;
        public Vector2 _posbefor;
        public Vector2 _posF;
        private AnimatedSprite[] _bossSprite;
        private SpriteSheet _sheet;

        private float moveSpeed = 0.5f;
        public bool isIdle = false;
        public bool interaction;
        public bool porte;
        public bool existe2;
        string animation;
        

        public Boss(Vector2 posP, bool existe)
        {
            _bossSprite = new AnimatedSprite[10];
            _pos = posP;
            interaction = false;
            porte = false;
            existe2 = existe;
            
        }
        public void Load(SpriteSheet[] spriteSheets)
        {
            for (int i = 0; i < spriteSheets.Length; i++)
            {
                _sheet = spriteSheets[i];
                _bossSprite[i] = new AnimatedSprite(_sheet);
            }

        }


        public void Update(GameTime gameTime, TiledMapTileLayer mapLayer, TiledMapTileLayer mapLayer2, TiledMapTileLayer mapLayer3, int TileSize, Player perso)
        {
           // playerSprite[0].Play("idle_R");
            isIdle = true;
            interaction = false;
            porte = false;
            _posbefor = _pos;
            
            animation = "idle";

             Follow( perso ,gameTime, mapLayer, 16);
             _bossSprite[1].Play(animation);
             
             _bossSprite[1].Update(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch, Matrix matrix)

        {
            spriteBatch.Begin(//All of these need to be here :(
                SpriteSortMode.Deferred,
                samplerState: SamplerState.PointClamp,
                effect: null,
                blendState: null,
                rasterizerState: null,
                depthStencilState: null,
                transformMatrix: matrix/*<-This is the main thing*/);

            spriteBatch.Draw(_bossSprite[1], _pos);

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
        public void Follow(Player perso, GameTime time, TiledMapTileLayer mapLayer, int taille)
        {
            _posF = _pos;
            if (perso._pos.X > _pos.X)
            {
                _posF.X = _pos.X + moveSpeed;
                animation = "walk_right";
            }
            else if (perso._pos.X < _pos.X)
            {
                _posF.X = _pos.X - moveSpeed;
                animation = "walk_left";
            }
            else if (perso._pos.Y > _pos.Y)
            {
                _posF.Y = _pos.Y + moveSpeed;
                animation = "walk_back";
            }
            else if (perso._pos.Y < _pos.Y)
            {
                _posF.Y = _pos.Y - moveSpeed;
                animation = "walk_up";
            }
            
            if(IsCollision(new Vector2(_posF.X+1,_posF.Y),mapLayer,16))
            {
                if(!IsCollision(new Vector2(_posF.X, _posF.Y+1), mapLayer, 16))
                    {
                    _posF.Y += moveSpeed;
                    //animation = "walk_back";
                }
                if (!IsCollision(new Vector2(_posF.X, _posF.Y - 1), mapLayer, 16))
                {
                    _posF.Y -= moveSpeed;
                    //animation = "walk_up";
                }
            }
            if (IsCollision(new Vector2(_posF.X - 1, _posF.Y), mapLayer, 16))
            {
                if (!IsCollision(new Vector2(_posF.X , _posF.Y - 1), mapLayer, 16))
                {
                    _posF.Y -= moveSpeed;
                   // animation = "walk_up";
                }
                 if(!IsCollision(new Vector2(_posF.X, _posF.Y + 1), mapLayer, 16))
                {
                    _posF.Y += moveSpeed;
                    //animation = "walk_back";
                }
            }
            if (IsCollision(new Vector2(_posF.X , _posF.Y+1), mapLayer, 16))
            {
                if (!IsCollision(new Vector2(_posF.X + 1, _posF.Y ), mapLayer, 16))
                {
                    _posF.X += moveSpeed;
                    //animation = "walk_right";
                }
                 if (!IsCollision(new Vector2(_posF.X-1, _posF.Y ), mapLayer, 16))
                {
                    _posF.X -= moveSpeed;
                   // animation = "walk_left";
                }
            }
            if (IsCollision(new Vector2(_posF.X , _posF.Y-1), mapLayer, 16))
            {
                if (!IsCollision(new Vector2(_posF.X - 1, _posF.Y ), mapLayer, 16))
                {
                    _posF.X -= moveSpeed;
                   // animation = "walk_left";
                }
                 if (!IsCollision(new Vector2(_posF.X+1, _posF.Y ), mapLayer, 16))
                {
                    _posF.X += moveSpeed;
                   // animation = "walk_right";
                }
            }
            if (!IsCollision(_posF, mapLayer, 16))
            {
                _pos = _posF;
            }

        }

    }
}
