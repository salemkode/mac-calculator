using System.Runtime.InteropServices;

namespace calc_ios
{
    public partial class calc : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        public calc()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            this.KeyPreview = true;
            b0.Click += NumberButton_Click;
            b1.Click += NumberButton_Click;
            b2.Click += NumberButton_Click;
            b3.Click += NumberButton_Click;
            b4.Click += NumberButton_Click;
            b5.Click += NumberButton_Click;
            b6.Click += NumberButton_Click;
            b7.Click += NumberButton_Click;
            b8.Click += NumberButton_Click;
            b9.Click += NumberButton_Click;

            // Dot click
            b_dot.Click += DotButton_Click;

            // Operator Click
            b_plus.Click += OperatorButton_Click("+");
            b_subtract.Click += OperatorButton_Click("-");
            b_multi.Click += OperatorButton_Click("x");
            b_div.Click += OperatorButton_Click("/");
            b_equal.Click += EqualButton_Click;
            b_negative.Click += nigative_Click;
            b_percent.Click += Percent_Cick;

            this.MouseDown += FormMain_MouseDown;
            this.MouseMove += FormMain_MouseMove;
            this.MouseUp += FormMain_MouseUp;

            // تفعيل استقبال ضغطات الكيبورد على مستوى الفورم
            this.KeyPreview = true;

            // ربط أحداث الكيبورد
            this.KeyPress += (object sender, KeyPressEventArgs e) =>
            {
                try
                {
                    if (e.KeyChar == (char)Keys.Back)
                    {
                        if (label1.Text.Length > 0)
                            label1.Text = label1.Text.Substring(0, label1.Text.Length - 1);
                        return;
                    }

                    int value = int.Parse(e.KeyChar + "");
                    if (label1.Text == "0")
                    {
                        label1.Text = value + "";
                        return;
                    }
                    label1.Text += value + "";
                } catch {
                    char[] supportedOp = ['+', '*', '-', '/'];
                    if (!supportedOp.Contains(e.KeyChar)) return;
                    label3.Text = e.KeyChar + "";
                    label2.Text = label1.Text;
                    label1.Text = "";
                }
                
            };
        }
        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void FormMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void FormMain_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        private void Percent_Cick(object? sender, EventArgs e)
        {
            double num1 = double.Parse(label1.Text);
            double num2 = double.Parse(label2.Text);

            if (num1 == 0 || num2 == 0)
            {
                return;
            }

            label1.Text = ((num2 / 100) * num1).ToString();
        }

        private void nigative_Click(object sender, EventArgs e)
        {
            if (label1.Text == null) return;
            double num1 = double.Parse(label1.Text) * -1;
            label1.Text = num1.ToString();
        }
        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (btn != null)
            {
                string value = btn.Name.Substring(1, 1);
                if (label1.Text == "0")
                    label1.Text = value;
                else
                    label1.Text += value;
            }
        }

        private void EqualButton_Click(object sender, EventArgs e)
        {
            try
            {

                double num1 = double.Parse(label1.Text);
                double num2 = double.Parse(label2.Text);
                double result = 0;

                if (num1 == 0 || num2 == 0)
                {
                    return;
                }
                Console.WriteLine(num1.ToString(), num2.ToString());
                switch (label3.Text)
                {
                    case "+":
                        result = num1 + num2;
                        break;
                    case "-":
                        result = num1 - num2;
                        break;
                    case "x":
                    case "*":
                        result = num1 * num2;
                        break;
                    case "/":
                        result = num1 / num2;
                        break;
                }

                label1.Text = result.ToString();
                label2.Text = "";
                label3.Text = "";
            }
            catch (Exception) { }
        }

        private System.EventHandler OperatorButton_Click(string op)
        {
            return (object _, EventArgs _) =>
            {
                label3.Text = op;
                label2.Text = label1.Text;
                label1.Text = "";
            };
        }

        private void DotButton_Click(object sender, EventArgs e)
        {
            if (!label1.Text.Contains('.'))
            {
                label1.Text += ".";
            }
        }

        private void bac_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
        }

        private void b_close_MouseHover(object sender, EventArgs e)
        {
            b_close.BackgroundImage = mac_calc.Properties.Resources.close_hover;

        }

        private void b_close_MouseLeave(object sender, EventArgs e)
        {
            b_close.BackgroundImage =mac_calc.Properties.Resources.close1;
        }

        private void b_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void b_mini_MouseHover(object sender, EventArgs e)
        {
            b_minimize.BackgroundImage =mac_calc.Properties.Resources.minmize_hover;

        }

        private void b_mini_MouseLeave(object sender, EventArgs e)
        {
            b_minimize.BackgroundImage =mac_calc.Properties.Resources.minimize;
        }

        private void b_mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}

