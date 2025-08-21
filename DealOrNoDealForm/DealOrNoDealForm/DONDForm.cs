using System;

namespace DealOrNoDealForm
{
    public partial class DONDForm : Form
    {
        private int NumOfBoxesClicked = 0;
        private List<float> Values = [0.01f, 0.1f, 0.5f, 1, 5, 10, 50, 100, 250, 500, 750,
        1000, 2000, 3000, 4000, 5000, 7500, 10000, 25000, 50000, 75000, 100000];
        private Dictionary<int, float> BoxValues = Enumerable.Range(0, 22)
            .ToDictionary(i => i, i => 0.0f);
        private List<Label> ValueLabels = new List<Label>();
        //private TextBox textBox;
        TextBox textBox = new TextBox
        {
            Location = new Point(650, 50),
            Size = new Size(500, 100),
            Font = new Font("Arial", 20, FontStyle.Bold),
            ForeColor = Color.Black,
            BackColor = Color.DarkGray,
            Multiline = true,
            ReadOnly = true,
            TextAlign = HorizontalAlignment.Center,
            Text = "Welcome to Deal or No Deal!" + Environment.NewLine +
                       "Please select a box to start with",
            SelectionStart = 0
        };
        private int BankerInterval = 6;
        public DONDForm()
        {
            InitializeComponent();
            Load += DONDForm_Load;
        }

        private void DONDForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;

            for (int i = 0; i < 2; i++)
            {
                Color bgColor = Color.Blue;
                if (i == 1)
                {
                    bgColor = Color.Red;
                }
                for (int j = 0; j < 11; j++)
                {
                    String valueText = "";
                    if (Values[i * 11 + j] < 1)
                    {
                        float tempVal = Values[i * 11 +j ] * 100;
                        valueText = $"{tempVal}p";
                    }
                    else
                    {
                        valueText = $"£{Values[i * 11 + j]}";
                    }

                    Label label = new Label
                    {
                        Text = valueText,
                        Location = new Point(10 + j * 100, 400 + i * 30),
                        Size = new Size(80, 20),
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        ForeColor = Color.White,
                        BackColor = bgColor,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    this.Controls.Add(label);
                    ValueLabels.Add(label);
                }
            }
            this.Controls.Add(textBox);

            for (int i= 0; i< BoxValues.Count; i++)
            {
                Random random = new Random();
                float boxValue = Values[random.Next(0, Values.Count)];
                int index = Values.IndexOf(boxValue);
                Values.RemoveAt(index);
                BoxValues[i] = boxValue;
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Button button = new Button
                    {
                        Text = $"{i * 6 + j + 1}",
                        Location = new Point(10 + j * 100, 10 + i * 100),
                        Size = new Size(80, 80),
                        BackColor = Color.Red
                    };
                    button.Click += (s, args) => ButtonClick(s, args);
                    this.Controls.Add(button);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Button button = new Button
                {
                    Text = $"{i + 19}",
                    Location = new Point(110 + i * 100, 310),
                    Size = new Size(80, 80),
                    BackColor = Color.Red
                };
                button.Click += (s, args) => ButtonClick(s, args);
                this.Controls.Add(button);
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (NumOfBoxesClicked == 0)
            {
                button.Location = new Point(850, 200);
                button.Enabled = false;
                Label userBoxLabel = new Label
                {
                    Text = $"Your Box:",
                    Location = new Point(750, 100),
                    Size = new Size(500, 50),
                    Font = new Font("Arial", 20, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                NumOfBoxesClicked++;
                textBox.Text = "You have selected box " + button.Text + ". Now, please select 5 boxes to open.";
                return;  
            }
            if (button != null)
            {
                int boxNumber = Int32.Parse(button.Text) - 1; // Convert to zero-based index
                ShowValue(boxNumber, button);
                button.Dispose();
            }
            NumOfBoxesClicked++;
            textBox.SelectionLength = 0;
            int addition = 0;
            if (NumOfBoxesClicked == BankerInterval && NumOfBoxesClicked < 15)
            {
                BankerOffer();
                addition = 3;
            }
            else if (NumOfBoxesClicked == BankerInterval && NumOfBoxesClicked >= 15)
            {
                BankerOffer();
                addition = 1;
            }
            BankerInterval += addition;
            if (BankerInterval - NumOfBoxesClicked == 1)
            {
                textBox.Text = "Please select 1 more box";
            }
            else
            {
                textBox.Text = $"Please select {BankerInterval - NumOfBoxesClicked} more boxes";
            }
        }

        private void ShowValue(int boxNumber, object sender)
        {
            Button button = sender as Button;
            String text = "";
            float value = BoxValues[boxNumber];
            Color bgColor = Color.Blue;
            if (value > 750)
            {
                bgColor = Color.Red;
            }
            if (value >= 1)
            {
                text = $"£{value}";
            }
            else
            {
                value *= 100;
                text = $"{value}p";  
            }
            int labelWidth = 80;
            int labelHeight = 30;
            Label valueLabel = new Label
            {
                Text = text,
                Location = new Point(button.Location.X + (button.Width - labelWidth) / 2,
                        button.Location.Y + (button.Height - labelHeight) / 2),
                Size = new Size(80, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = bgColor,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(valueLabel);
            for (int i = 0; i < ValueLabels.Count; i++)
            {
                if (ValueLabels[i].Text == text)
                {
                    ValueLabels[i].Dispose();
                }
            }
        }

        private void BankerOffer()
        {
            textBox.Text = "The banker is making an offer.";
            MessageBox.Show("Banker Offer", "Banker Offer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
