namespace Salmon.Views
{
    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();

            Chess.FieldUI field = new Chess.FieldUI(this.ClientSize)
            {
                Parent = this,
                Visible = true,
                Dock = DockStyle.Fill,
            };
        }
    }
}