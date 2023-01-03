namespace Chess
{
    internal class GameManager
    {
        private Form parent;
        private FieldUI front;
        private FieldData back;
        private Team turn;

        public Team Turn { get => this.turn; }
        public FieldData Data => this.back;

        public GameManager(Form parent)
        {
            this.back = new FieldData(this);
            this.front = new FieldUI(this, parent.ClientSize)
            {
                Parent = parent,
                Visible = true,
                Dock = DockStyle.Fill,
            };

            this.parent = parent;
            this.turn = Team.First;
        }

        public void ChangeTurn()
        {
            this.turn = (this.turn == Team.First ? Team.Last : Team.First);

            switch (this.back.IsMated())
            {
                case GameState.FirstWin: new Thread(() => { MessageBox.Show("First Win!"); }).Start(); break;
                case GameState.LastWin: new Thread(() => { MessageBox.Show("Last Win!"); }).Start(); break;
                case GameState.Draw: new Thread(() => { MessageBox.Show("Draw..."); }).Start(); break;
            }
        }
    }
}
