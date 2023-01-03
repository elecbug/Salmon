namespace CutlassS
{
    public partial class MainForm : Form
    {
        private SplitContainer container;
        private RichTextBox log_box;
        private RichTextBox write_box;
        public Command.Manager manager;

        public MainForm()
        {
            InitializeComponent();

            this.Text = "Cutlass Server Main";
            this.ClientSize = new Size(800, 600);
            this.Visible = true;

            this.container = new SplitContainer()
            {
                Parent = this,
                Visible = true,
                Size = this.ClientSize,
                SplitterWidth = 5,
                Orientation = Orientation.Vertical,
                SplitterDistance = 600,
                Dock = DockStyle.Fill,
            };
            this.log_box = new RichTextBox()
            {
                Parent = this.container.Panel1,
                Visible = true,
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Consolas", 11, FontStyle.Regular),
                Size = this.container.Panel1.ClientSize,
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                LanguageOption = RichTextBoxLanguageOptions.UIFonts,
            };
            this.write_box = new RichTextBox()
            {
                Parent = this.container.Panel2,
                Visible = true,
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Consolas", 11, FontStyle.Regular),
                Size = this.container.Panel2.ClientSize,
                Multiline = true,
                Dock = DockStyle.Fill,
                LanguageOption = RichTextBoxLanguageOptions.UIFonts,
            };
            this.write_box.KeyPress += PressWriteBox;

            this.manager = Command.Manager.Instance();

            this.write_box.Focus();
        }

        private void PressWriteBox(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                this.manager.Command = this.write_box.Text;
                this.log_box.Text += "MANAGER >> " + this.write_box.Text; 
                this.write_box.Text = "";
                this.log_box.Text += "SYSTEMS >> " + this.manager.ExcuteCommand() + "\r\n";
            }
        }
    }
}