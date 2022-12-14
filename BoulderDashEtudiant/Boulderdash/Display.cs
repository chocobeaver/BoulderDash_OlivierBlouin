// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash




//library
#region
using Item;
using Map;
using SFML.Graphics;
using SFML.System;
using System.ComponentModel;
#endregion

namespace Display
{
    //class cave heriting from  displayable that dictate what is showable
    #region
    class Cave :Displayable
    {
        // variable
        #region
        public Objet tile { get; private set; }
        #endregion

        //constructer
        #region
        //this constructer seem empty but has all the value from displayable that are already over therre
        public Cave(Objet tile,RenderWindow Screen,Coord XY):base(DisplayCase.GetSprite(tile),Screen,XY)//cave herit from displayable
            //and create sprite from texture got
        { 
            this.tile = tile;
        }
        #endregion

        //function
        #region
        //set a tile to smothing we give it in parameter
        public void SetTile(Objet tile)
        {
            this.tile = tile;
            S = DisplayCase.GetSprite(tile);
        }

        public virtual void ApplyEffect(Map.Map map)
        {
           
        }
        #endregion

    }
    #endregion

    //class displayable
    #region
    public abstract class Displayable
    {
        //variable
        #region
        public static int lvl { get; set; } = 0;   //the map we are on
        public const int NbPixelsParCase = 24;     // Nombre de pixels d'une case
        protected Sprite S;                        //object sprite as s taking the texture and giving it a position
        public RenderWindow Screen { get; set; }   //a screen to display on
        public Coord XY;                           // a position 
        #endregion

        //construster
        #region
        public Displayable(Sprite S, RenderWindow Screen,  Coord XY)
        {
            this.S = S;
            this.XY = XY;
            this.Screen = Screen;
            
        }
        #endregion

        //second constructer
        #region
        //make a displayable from img file and using  the other constructer to built it
        public Displayable(string fichierImg, RenderWindow Screen,Coord XY): this(new Sprite(new Texture(fichierImg)), Screen,XY)
        { }
        #endregion

        //function
        #region
        //show the instance of it on the screen at the position it has
        virtual public void Afficher()//virtual make it possible to overide
        {
            
            Vector2f position = new Vector2f(XY.X * NbPixelsParCase, XY.Y * NbPixelsParCase);
            S.Position = position;
            Screen.Draw(S);
        }
        #endregion

    }
    #endregion

    //classs displaycase
    #region
    class DisplayCase
    {
        //variable that are the sprite of what a tile can be
        #region
        //initialise wall    
        static private Sprite Wall = new Sprite(new Texture("images/mur24.bmp"));

        //initialise Ground
        static private Sprite Ground = new Sprite(new Texture("images/terre24.bmp"));

        //initialise Rock
        static private Sprite Rock = new Sprite(new Texture("images/roche24.bmp"));

        //initialise diamond
        static private Sprite Diamond = new Sprite(new Texture("images/diamant24.bmp"));
        //initialise diamond
        static private Sprite Door = new Sprite(new Texture("images/diamant24.bmp"));

        //initialise pedestal
        //get from item
        #endregion

        //constructer
        #region
        //none
        #endregion

        //function
        #region
        public static Sprite GetSprite(Objet RN)
        {
            if (RN == Objet.D) { return Diamond; }
            if (RN == Objet.T) { return Ground; }
            if (RN == Objet.R) { return Rock; }
            if (RN == Objet.M) { return Wall; }
            if (RN == Objet.RT) { return Rock; }
            if (RN == Objet.DoN||RN==Objet.DoB) { return Door; }
            return new Sprite();
        }
        #endregion
    }
    #endregion
}
