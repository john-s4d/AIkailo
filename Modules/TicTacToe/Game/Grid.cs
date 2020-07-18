namespace AIkailo.Modules.TicTacToe.Game
{
    public class Grid
    {
        public Token[,] Slots = new Token[3, 3];

        public Token Get(int x, int y)
        {
            return Slots[x, y];
        }

        public void Set(Token t, int x, int y)
        {
            Slots[x, y] = t;
        }
    }
}
