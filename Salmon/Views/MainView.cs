namespace Salmon.Views
{
    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();

            Chess.Field field = new Chess.Field(this.ClientSize)
            {
                Parent = this,
                Visible = true,
            };
        }
    }
}