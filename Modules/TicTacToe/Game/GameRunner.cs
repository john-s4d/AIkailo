namespace AIkailo.Modules.TicTacToe.Game
{
    class GameRunner
    {
        public GameStatus Status { get; private set; }
        public Token LastPlayer { get; private set; }

        private Grid grid;

        public GameRunner()
        {
            NewGame();
        }

        public void NewGame()
        {
            grid = new Grid();
            Status = new GameStatus();
            LastPlayer = Token.Empty;
        }

        public bool ExecuteMove(Token t, int x, int y, out GameStatus status)
        {
            status = Status;

            // Is this a valid move
            if (status.IsOver || x < 0 || x > 2 || y < 0 || y > 2 || t == Token.Empty || t == LastPlayer || grid.Get(x, y) != Token.Empty)
            {
                return false;
            }

            grid.Set(t, x, y);
            LastPlayer = t;

            DoStatus(grid);

            return true;
        }

        private void DoStatus(Grid grid)
        {
            // Vertical
            for (int x = 0; x < 3; x++)
            {
                Status.IsOver = IsWinner(grid.Get(x, 0), grid.Get(x, 1), grid.Get(x, 2), ref Status.Winner);
                if (Status.IsOver) { return; }
            }

            // Horizontal
            for (int y = 0; y < 3; y++)
            {
                Status.IsOver = IsWinner(grid.Get(0, y), grid.Get(1, y), grid.Get(2, y), ref Status.Winner);
                if (Status.IsOver) { return; }
            }

            // Diagonal TL-BR
            Status.IsOver = IsWinner(grid.Get(0, 0), grid.Get(1, 1), grid.Get(2, 2), ref Status.Winner);
            if (Status.IsOver) { return; }

            // Diagonal TR-BL
            Status.IsOver = IsWinner(grid.Get(2, 0), grid.Get(1, 1), grid.Get(0, 2), ref Status.Winner);
            if (Status.IsOver) { return; }

            // Moves Available?
            foreach (Token t in grid.Slots)
            {
                if (t == Token.Empty)
                {
                    Status.IsOver = false;
                    return;
                }
            }

            Status.IsOver = true;
        }

        private bool IsWinner(Token t0, Token t1, Token t2, ref Token winner)
        {
            if (Token.Empty != t0 && t0 == t1 && t1 == t2)
            {
                winner = t0;
                return true;
            }
            return false;
        }
    }
}
