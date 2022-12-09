// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash





using Map;
using SFML.Graphics;
using SFML.System;
using System.ComponentModel;

namespace Display
{
    
    class Cave:Displayable
    {
        
        public Cave(Objet tile,RenderWindow Screen,Coord XY):base(DisplayCase.GetSprite(tile),Screen,XY)//cave herit from displayable
            //and create sprite from texture got
        { 
            this.tile = tile;
        }  
        public Objet tile { get; private set; }
        public void SetTile(Objet tile)
        {
            this.tile = tile;
            S = DisplayCase.GetSprite(tile);
        }
        
    }
    public abstract class Displayable
    {
        public const int NbPixelsParCase = 24;     // Nombre de pixels d'une case
        protected Sprite S;
        private RenderWindow Screen;
        public Coord XY;
        public Displayable(Sprite S, RenderWindow Screen,  Coord XY)
        {
            this.S = S;
            this.XY = XY;
            this.Screen = Screen;
        }
        public Displayable(string fichierImg, RenderWindow Screen,Coord XY): this(new Sprite(new Texture(fichierImg)), Screen,XY)
        { }


        virtual public void Afficher()//virtual make it possible to overide
        {
            
            Vector2f position = new Vector2f(XY.X * NbPixelsParCase, XY.Y * NbPixelsParCase);
            S.Position = position;
            Screen.Draw(S);
        }
        
    }
    class DisplayCase
    {
        //initialise wall    
        static private Sprite Wall = new Sprite(new Texture("images/mur24.bmp"));

        //initialise Ground
        static private Sprite Ground = new Sprite(new Texture("images/terre24.bmp"));

        //initialise Rock
        static private Sprite Rock = new Sprite(new Texture("images/roche24.bmp"));

        //initialise diamond
        static private Sprite Diamond = new Sprite(new Texture("images/diamant24.bmp"));
        

        
        public static Sprite GetSprite(Objet RN)
        {
            if (RN == Objet.D) { return Diamond; }
            if (RN == Objet.T) { return Ground; }
            if (RN == Objet.R) { return Rock; }
            if (RN == Objet.M) { return Wall; }
            if (RN == Objet.RT) { return Rock; }
            return new Sprite();


        }
    }

   

}
