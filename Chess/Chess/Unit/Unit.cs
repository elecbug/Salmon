namespace Chess
{
    internal abstract class Unit : IControl
    {
        protected Point location;
        protected Type type;
        protected Team team;
        protected bool is_alived;
        protected int move_count;

        public Point Location { get => this.location; set => this.location = value; }
        public Type Type { get => this.type; }
        public Team Team { get => this.team; }
        public int MoveCount { get => this.move_count; }

        public Unit(Point location, Type type, Team team, bool is_alived = true)
        {
            this.location = location;
            this.type = type;
            this.team = team;
            this.is_alived = is_alived;
            this.move_count = 0;
        }
        public Unit(Unit target)
        {
            this.location = target.location;
            this.type = target.type;
            this.team = target.team;
            this.is_alived = target.is_alived;
            this.move_count = target.move_count;
        }

        public void Kill() => this.is_alived = false;
        public void IncreaseMove() => this.move_count++;

        public abstract List<Point> AbleToMove(Unit?[,] unit_matrix);
        public abstract List<Point> AbleToAttack(Unit?[,] unit_matrix);

        public abstract Unit? Clone();
    }
}
