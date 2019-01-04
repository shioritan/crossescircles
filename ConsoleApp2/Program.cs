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
            bool[] rules = { true, false };
            Game MyGame = new Game(boardWid, boardHei, symbols, rules);
            MyGame.Play();
        }
        class Game
        {
            Board Board;
            List<Player> Players;
            public Game(int boardWid, int boardHei, char[] symbols, bool[] rules)
            {
                Board = new Board(boardWid, boardHei);
                Players = new List<Player>();
                for (int i = 0; i < symbols.Length; i++) Players.Add(new Player(symbols[i], rules[i]));
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
            public bool CheckWin(List<Cell> cells) => CheckStraight(cells, true) || CheckStraight(cells, false) || 
                CheckDiagonal(cells, true) || CheckDiagonal(cells, false);
            bool CheckDiagonal(List<Cell> cells, bool main)
            {
                int limit = Param.Wid;
                return CheckLine(cells, limit, main ? CheckRuleType.DiagMain : CheckRuleType.DiagInverse, sum: main ? 0 : limit - 1);
            }
            bool CheckStraight(List<Cell> cells, bool horz)
            {
                bool lineExist = false;
                int limit = horz ? Param.Hei : Param.Wid;
                for (int i = 0; i < limit; i++)
                {
                    lineExist = CheckLine(cells, limit, horz ? CheckRuleType.Horz : CheckRuleType.Vert, line: i);
                    if (lineExist) break;
                }
                return lineExist;
            }
            bool CheckLine(List<Cell> cells, int limit, CheckRuleType ruleType, int line = 0, int sum = 0)
            {
                int cnt = 0;
                foreach (Cell curCell in cells) if (CheckRule(curCell, ruleType, line, sum)) cnt++;
                return cnt == limit;
            }
            enum CheckRuleType {Horz, Vert, DiagMain, DiagInverse}
            bool CheckRule(Cell cell, CheckRuleType ruleType, int line = 0, int sum = 0)
            {
                bool check = false;
                switch (ruleType)
                {
                    case CheckRuleType.Horz:
                        check = cell.X == line;
                        break;
                    case CheckRuleType.Vert:
                        check = cell.Y == line;
                        break;
                    case CheckRuleType.DiagMain:
                        check = cell.X == cell.Y;
                        break;
                    case CheckRuleType.DiagInverse:
                        check = cell.X + cell.Y == sum;
                        break;
                    default:
                        break;
                }
                return check;
            }
            public List<Cell> GetCells(char symbol = '\0', bool diagonal = false)
            {
                List<Cell> cells = new List<Cell>();
                foreach (Cell curCell in Cells) if (curCell.Symbol == symbol && (!diagonal ||
                        CheckRule(curCell, CheckRuleType.DiagMain) ||
                        CheckRule(curCell, CheckRuleType.DiagInverse, sum: Param.Wid - 1))) cells.Add(curCell);
                return cells;
            }
            public Cell GetRandomCell(List<Cell> cells) => cells.Count > 0 ? cells[PseudoRandom(cells.Count)] : null;
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
            public bool Filled;
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
                Cell emptyCell = curRule.GetCell(board, players[board.CurPlayer].Diagonal);
                bool nullCell = emptyCell == null;
                bool win = false;
                char curSymbol = players[board.CurPlayer].Symbol;
                if (!nullCell)
                {
                    board.Cells[emptyCell.X, emptyCell.Y].Fill(curSymbol);
                    board.WriteBoard();
                    Console.Write("\n");
                    win = board.CheckWin(board.GetCells(symbol: curSymbol));
                }
                board.EndGame = win || nullCell;
                if (board.EndGame) Console.Write(win ? curSymbol + " win" : "draw");
                board.PlayerChange(players.Count);
            }
        }
        class Player
        {
            public char Symbol;
            public Player(char symbol, bool diagonal) { Symbol = symbol; Diagonal = diagonal; }
            public bool Diagonal;
        }
        class PlayerRule
        {
            public PlayerRule() { }
            public Cell GetCell(Board board, bool diagonal) => board.GetRandomCell(board.GetCells(diagonal: diagonal));
        }
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
    }
}