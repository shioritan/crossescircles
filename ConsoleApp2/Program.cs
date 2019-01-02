using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            int boardWid = 3, boardHei = 3;
            char[] symbols = { 'x', 'o' };
            Game MyGame = new Game(boardWid, boardHei, symbols);
            MyGame.Play();
        }
        class Game
        {
            Board Board;
            List<Player> Players;
            public Game(int boardWid, int boardHei, char[] symbols)
            {
                Board = new Board(boardWid, boardHei);
                Players = new List<Player>();
                foreach (char symbol in symbols) Players.Add(new Player(symbol));
            }
            public void Play()
            {
                do {
                    Turn CurTurn = new Turn();
                    CurTurn.Make(Board, Players);
                } while (!Board.EndGame);
            }
        }
        class Board
        {
            public Cell[,] Cells;
            public BoardParam Param;
            public int CurPlayer;
            public void PlayerChange(int colPlayers)
            {
                CurPlayer++;
                if (CurPlayer == colPlayers) CurPlayer = 0;
            }
            public Board(int wid, int hei)
            {
                Param = new BoardParam(wid, hei);
                Cells = new Cell[wid, hei];
                for (int i = 0; i < wid; i++)
                    for (int j = 0; j < hei; j++)
                        Cells[i, j] = new Cell(i, j);
            }

            public bool EndGame;
            public bool CheckWin()
            {

                return CheckLine();
            }
            bool CheckLine()
            {
                foreach (Cell curCell in Cells)
                {
                    if (curCell.X == 0) { }

                    for (int i = 0; i < Param.Wid; i++)
                    {
                        bool horzLine = false;
                        for (int j = 0; j < Param.Hei; j++)
                        {

                        }
                    }
                }
                return false;
            }
            public Cell GetEmptyCell()
            {
                List<Cell> cells = new List<Cell>();
                foreach (Cell curCell in Cells)
                    if (!curCell.Filled) cells.Add(curCell);
                Cell empCell = null;
                if (cells.Count > 0)
                {
                    empCell = cells[PseudoRandom(cells.Count)];
                    if (cells.Count == 1) empCell.Last = true;
                }
                return empCell;
            }
            public void WriteBoard()
            {
                foreach (Cell curCell in Cells)
                    Console.Write((curCell.Filled ? curCell.Symbol.ToString() : ".") + (curCell.Y + 1 == Param.Hei ? "\n" : ""));
            }
        }
        class Cell
        {
            public int X, Y;
            public char Symbol;
            public bool Filled, Last;
            public Cell(int x, int y) { X = x; Y = y; }
            public void Fill(char symbol)
            {
                Filled = true;
                Symbol = symbol;
            }
        }
        class Turn
        {
            public void Make(Board board, List<Player> players)
            {
                PlayerRule curRule = new PlayerRule();
                Cell emptyCell = curRule.GetCell(board);
                board.Cells[emptyCell.X, emptyCell.Y].Fill(players[board.CurPlayer].Symbol);
                board.WriteBoard();
                Console.Write("\n");
                board.EndGame = board.CheckWin() || emptyCell.Last;
                board.PlayerChange(players.Count);
            }
        }
        class Player
        {
            public char Symbol;
            public Player(char symbol) { Symbol = symbol; }
            public PlayerRule Rule;
        }
        class PlayerRule
        {
            public PlayerRule() { }
            public int Lenght;
            public bool Odd, FirstTurn;
            public void Diag() {}
            public void Random(){}
            public Cell GetCell(Board board) => board.GetEmptyCell();
        }
        class EndGameRule { }
        class BoardParam
        {
            public int Wid, Hei;
            public BoardParam(int wid, int hei) { Wid = wid; Hei = hei; }
        }
        static int PseudoRandom(int cellCount)
        {
            const int limit = 10;
            int random = DateTime.Now.Millisecond % limit, index = 0;
            while (cellCount > limit)
            {
                cellCount /= limit;
                index += random * cellCount;
            }
            if (cellCount > 0) index += random % cellCount;
            return index;
        }

        //======================================================

        static void XO1()
        {
            Console.WriteLine("x..\n...\n...\n\nxo.\n...\n...\n\nxo.\n.x.\n...\n\nxo.\n.x.\n..o\n\n" +
                "xox\n.x.\n..o\n\nxox\n.x.\n.oo\n\nxox\n.x.\nxoo\n\n");
        }

        static void PandaMarry()
        {
            Panda panda1 = new Panda("Ленивый");
            Panda panda2 = new Panda("Лариска");
            panda1.Marry(panda2); //женим их
            Console.WriteLine("у панды {0} партнер {1}", panda1.Name, panda1.Mate.Name);
            Console.WriteLine("у панды {0} партнер {1}", panda2.Name, panda2.Mate.Name);
        }
    }

    public class Panda
    {
        public string Name { get; set; }
        public Panda(string _name)
        {
            Name = _name;
        }
                public Panda Mate;
        public void Marry(Panda partner)
        {
            Mate = partner; // для этого экземпляра запоминаем в поле Mate другой экземпляр (партнер). Далее с ним можно работать
            partner.Mate = this; // в тоже время и в экземпляре партнер в ЕГО поле Mate запоминаем этот (this)  экземпляр
        }
    }

}
