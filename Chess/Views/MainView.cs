namespace Views
{
    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();

            Chess.GameManager manager = new Chess.GameManager(this);
        }
    }
}