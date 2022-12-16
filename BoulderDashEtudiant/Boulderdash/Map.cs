// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash




//library
#region
using Display;
using Item;
using SFML.Graphics;
using System;
using System.ComponentModel;
#endregion

namespace Map
{
    //enum
    #region
    // Represent direction ennemi can make
    public enum Direction { Gauche, Haut, Droite, Bas, Null };
    //check on which lvl to chow coresponding map
    
    // represent what each tile can be        
    // {empty, wall, ground, Rock, falling rock, Diamond}
    enum Objet { V, M, T, R, RT, D, P,DoN,DoB};
    #endregion

    //class map
    #region
    /// <summary>
    /// this is a class for the map it herit from displayable that dictate what to show
    /// </summary>
    class Map :Displayable 
    {
        
        //creating variable a map has
        #region
        private Random R = new Random();
        //a double array
        private Cave[,] _Map;
        //a character
        public Hero.Personnage RockFord { get; set; }
        //a check if the game still on
        public bool GameContinue=true;

        #endregion
        //constructer
        #region
        public Map(Objet[,] Map,RenderWindow Screen,Hero.Personnage RockFord) : base((Sprite)null, Screen, new Coord(0,0))
        {
           
            //*note this make sure we using the one the class has as parameter
            this.RockFord = RockFord;
           
            //getting the grid as a class cave
            _Map= new Cave[Map.GetLength(0), Map.GetLength(1)];

            //loop to take the double array from the csv reader and putting the the objet int the Cave double array
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if(Map[i,j] == Objet.P) { _Map[i, j] = new Pedestal(Items.M, new Coord(j, i), Screen); continue; }
                    _Map[i, j] =new Cave(Map[i, j],Screen,new Coord(j,i));
                }
            }           
        }
        #endregion

        //class function
        #region
        //get the objet in a specific tile
        public Objet GetObjet(Coord XY)
        {

            return GetCave(XY).tile;
        }
        //set objet in a specific tile
        public void SetObjet(Coord XY, Objet obj)
        {
            GetCave(XY).SetTile(obj);    
        }
        //get the width of the double array
        public int GetWidth()
        {
            return _Map.GetLength(1);
        }
        //get the height of the double array
        public int GetHeight()
        {
            return _Map.GetLength(0);
        }
        //check if a tile is empty and return a bool
        public bool IsEmpty(Coord XY)
        {
            return GetObjet(XY) == Objet.V;
        }

        public void ApplyPlayerEffect()
        {
            GetCave(RockFord.XY).ApplyEffect(this);
        }
        private Cave GetCave(Coord XY)
        {
            return _Map[XY.Y, XY.X];
        }
        //show the map and make the action of each tile
        public override void Afficher()
        {
            //test is a variable to make the falling rock morre consistant and progressive fall and not instant
            //by checking if a rock is already falling and not make any more fall till the end of the loop
            bool test = true;
            //double loop to draw every map part
            for (int i = 0; i < GetWidth(); i++)
            {
                for (int j = 0; j < GetHeight(); j++)
                {
                    //taking the position of the loop and converting them in vector Coord
                    //*because the double array of objet as coord and not 2 position
                    Coord XY = new Coord(i, j);
                    //taking the obj that the position is rn and making a copy
                    Objet RN = GetObjet(XY);

                
                    
                    //section rock calling the move rock function
                    if (RN == Objet.R)
                    {
                        MoveRock(XY, RN);
                    }

                    //section of the falling ronck and checking if a rock is already falling
                    if (GetObjet(XY) == Objet.RT && test)
                    {
                        test = false;//to make one change of falling rock per turn last minute to fix rock dropping down instantly
                        //if check if the tile below is empty
                        if (IsEmpty(XY.Down()))
                        {
                            //check if the character is below
                            if (XY.Down().Equals(RockFord.XY))
                            {
                                GameContinue = false;//end is when RockFord die or win to freeze de game 

                            }
                            //set the place the falling rock as empty
                            SetObjet(XY, Objet.V);
                            //set the tile bellow as the falling rock
                            SetObjet(XY.Down(), Objet.RT);
                        }
                        else//basacly if theres something below the falling rock it goes back to normal rock
                        {
                            SetObjet(XY, Objet.R);
                        }


                    }
                    //call the function to show the map
                    GetCave(XY).Afficher();
                    

                }
            }
            
        
        }
        //function that check when we move the rock
        private void MoveRock( Coord XY, Objet RN)
        {
            //if theres nothing under the rock become a falling rock
            if (IsEmpty(XY.Down()) && !RockFord.XY.Equals(XY.Down()))
            {
                SetObjet(XY, Objet.RT);
            }
            //rock slide event
            //if theres a rock under and the side is empty and rockford doesnt hold it move the rock to the unstable side and transfonrme it to falling rock
            else if (!RockFord.XY.Equals(XY.Right()) && GetObjet(XY.Down()) == Objet.R && IsEmpty(XY.Right()) && IsEmpty(XY.Right().Down()))
            {

                SetObjet(XY.Right(), Objet.RT);
                SetObjet(XY, Objet.V);

            }
            //check the other side in this case left
            else if (!RockFord.XY.Equals(XY.Left()) && GetObjet(XY.Down()) == Objet.R && IsEmpty(XY.Left()) && IsEmpty(XY.Left().Down()))
            {

                SetObjet(XY.Left(), Objet.RT);
                SetObjet(XY, Objet.V);
            }

        }

        #endregion

    }
    #endregion

    //class Coord 
    #region
    /// <summary>
    /// in summary this change every position to 2d  vector making it easier to work whit position
    /// </summary>
    public class Coord
    {
        //variable 
        #region
        public int X { get; set; }//its position in the line  
        public int Y { get; set; }//its position inthe row
        #endregion

        //constructer
        #region
        public Coord(int x, int y)//constructer
        {
            X = x;
            Y = y;
        }
        #endregion

        //second constructer
        #region
        /// <summary>
        /// construct a Coord whit a direction and add it +1
        /// </summary>
        public Coord(Direction d)//constructeur whit direction
        {
            X = 0;
            Y = 0;
            if (d==Direction.Haut) { Y--; }
            else if (d == Direction.Bas) { Y++; }
            else if (d == Direction.Droite) { X++; }
            else if (d == Direction.Gauche) { X--; }

        }
        #endregion

        //third constructer
        #region
        /// <summary>
        /// this constructer take a coors to make a coord so basacly use to make copy 
        /// </summary>
        public Coord(Coord C) : this(C.X, C.Y) { }
        #endregion

        //function
        #region
        //cloning
        public Coord Clone()
        {
            return new Coord(this);
        }
        //check a coord and return its direction
        public Direction AsDirection()
        {
            for (int i = 0; i < 5; i++)
            {
                if (this.Equals(new Coord((Direction)i))) { return (Direction)i; }//create every direction as coords and check if = to self and return a direction 
            }
            return Direction.Null;
        }
        //check if equal
        public bool Equals(Coord other)
        {//*note internet is my friend
            //this is use to check if we have a value same as an other and not needing a specific form can take one of the 3 constructer
            //check if the given coord is null or "itself"(more or less same value")
            if (ReferenceEquals(other, null)) { return false; }//if other null
                
            if (ReferenceEquals(this, other)) { return true; }//if other self
            return X == other.X && Y == other.Y;//check if coords are equals
        }
        //add a vector to an other
        public Coord Add(Coord XY)
        {
            X += XY.X;
            Y += XY.Y;
            return this;
        }

        //rotate left the direction/value of a vector and return it after the change
        public Coord RotatedLeft()
        {
            int tempX=X;
            X=Y;
            Y=-tempX;
            return this;
        }
        //rotate right the direction/value of a vector and return it after the change
        public Coord RotatedRight()
        {
            int tempY = Y;
            Y = X;
            X = -tempY;
            return this;
        }
        //return a copy of the tile above
        public Coord Up()
        {
            return new Coord(X,Y-1);
        }
        //return a copy of the tile below
        public Coord Down()
        {
            return new Coord(X, Y + 1);
        }
        //return a copy of the tile on the left
        public Coord Left()
        {
            return new Coord(X-1, Y);
        }
        //return a copy of the tile on the right
        public Coord Right()
        {
            return new Coord(X+1, Y);
        }
        #endregion
    }
    #endregion
}
