// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash





using Display;
using SFML.Graphics;
using System;
using System.ComponentModel;

namespace Map
{
    // Représente les directions de déplacement de l'ennemi 
    public enum Direction { Gauche, Haut, Droite, Bas, Null };
    // Représente ce que une case du tableau peut etre 
    // {Vide, Mur, Terre, Rocher, RocherTombant, Diamant}
    enum Objet { V, M, T, R, RT, D };
    class Map:Displayable 
    {
        private Cave[,] _Map;
        public Hero.Personnage RockFord { get; set; }
        public bool GameContinue=true;
        public Map(Objet[,] Map,RenderWindow Screen,Hero.Personnage RockFord) : base((Sprite)null, Screen, new Coord(0,0))
        {
            this.RockFord = RockFord;
            Console.WriteLine(Map.GetLength(0));

            _Map= new Cave[Map.GetLength(0), Map.GetLength(1)];
            Console.WriteLine(_Map.GetLength(0));
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    _Map[i, j] =new Cave(Map[i, j],Screen,new Coord(j,i));
                }
            }           
        }

        public Objet GetObjet(Coord XY)
        {

            return _Map[XY.Y,XY.X].tile;
        }
        public void SetObjet(Coord XY, Objet obj)
        {
            _Map[XY.Y,XY.X].SetTile(obj);    
        }
        public int GetWidth()
        {
            return _Map.GetLength(1);
        }
        public int GetHeight()
        {
            return _Map.GetLength(0);
        }
        public bool IsEmpty(Coord XY)
        {
            return GetObjet(XY) == Objet.V;
        }
        public override void Afficher()
        {
           
            bool test = true;
            //loop to draw every map part
            for (int i = 0; i < GetWidth(); i++)
            {
                for (int j = 0; j < GetHeight(); j++)
                {
                    Coord XY = new Coord(i, j);
                    Objet RN = GetObjet(XY);



                    if (RN == Objet.R)
                    {
                        MoveRock(XY, RN);
                    }


                    if (GetObjet(XY) == Objet.RT && test)
                    {
                        test = false;//to make one change of falling rock per turn last minute to fix rock dropping down instantly
                        if (IsEmpty(XY.Down()))
                        {
                            if (XY.Down().Equals(RockFord.XY))
                            {
                                GameContinue = false;//end is when RockFord die or win to freeze de game 

                            }
                            SetObjet(XY, Objet.V);
                            SetObjet(XY.Down(), Objet.RT);
                        }
                        else
                        {
                            SetObjet(XY, Objet.R);
                        }


                    }
                    _Map[XY.Y,XY.X].Afficher();

                }
            }
            
        
        }
        private void MoveRock( Coord XY, Objet RN)
        {
            if (IsEmpty(XY.Down()) && !RockFord.XY.Equals(XY.Down()))
            {
                SetObjet(XY, Objet.RT);
            }
            else if (!RockFord.XY.Equals(XY.Right()) && GetObjet(XY.Down()) == Objet.R && IsEmpty(XY.Right()) && IsEmpty(XY.Right().Down()))
            {

                SetObjet(XY.Right(), Objet.RT);
                SetObjet(XY, Objet.V);

            }
            else if (!RockFord.XY.Equals(XY.Left()) && GetObjet(XY.Down()) == Objet.R && IsEmpty(XY.Left()) && IsEmpty(XY.Left().Down()))
            {

                SetObjet(XY.Left(), Objet.RT);
                SetObjet(XY, Objet.V);
            }

        }

       
           
    }  
    public class Coord
    {

        public int X { get; set; }
        public int Y { get; set; }

        
        public Coord(int x, int y)//constructer
        {
            X = x;
            Y = y;
        }
        public Coord(Direction d)//constructeur whit direction
        {
            X = 0;
            Y = 0;
            if (d==Direction.Haut) { Y--; }
            else if (d == Direction.Bas) { Y++; }
            else if (d == Direction.Droite) { X++; }
            else if (d == Direction.Gauche) { X--; }

        }
        public Coord(Coord C) : this(C.X, C.Y) { }//constructeur de copy taking a cood as build 
        
            
        public Coord Clone()//cloning
        {
            return new Coord(this);
        }
        public Direction AsDirection()
        {
            for (int i = 0; i < 5; i++)
            {
                if (this.Equals(new Coord((Direction)i))) { return (Direction)i; }//create every direction as coords and check if = to self and return a direction 
            }
            return Direction.Null;
        }
        public bool Equals(Coord other)
        {//internet is my friend
            if (ReferenceEquals(other, null)) { return false; }//if other null
                
            if (ReferenceEquals(this, other)) { return true; }//if other self
            return X == other.X && Y == other.Y;//check if coords are equals
        }
        public Coord Add(Coord XY)
        {
            X += XY.X;
            Y += XY.Y;
            return this;
        }


        public Coord RotatedLeft()
        {
            int tempX=X;
            X=Y;
            Y=-tempX;
            return this;
        }
        public Coord RotatedRight()
        {
            int tempY = Y;
            Y = X;
            X = -tempY;
            return this;
        }
        //the function down below make copy ****
        public Coord Up()
        {
            return new Coord(X,Y-1);
        }
        public Coord Down()
        {
            return new Coord(X, Y + 1);
        }
        public Coord Left()
        {
            return new Coord(X-1, Y);
        }
        public Coord Right()
        {
            return new Coord(X+1, Y);
        }
    }
    
}
