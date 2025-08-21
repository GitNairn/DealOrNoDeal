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
        Label previousLabel;
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
                    String valueText = GetMoneyText(Values[i * 11 + j]);
                    Label label = new Label
                    {
                        Text = valueText,
                        Location = new Point(10 + j * 100, 420 + i * 30),
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

            for (int i = 0; i < BoxValues.Count; i++)
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
                        BackColor = Color.Red,
                        Font = new Font("Arial", 16, FontStyle.Bold),
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
                    BackColor = Color.Red,
                    Font = new Font("Arial", 16, FontStyle.Bold),
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
                    Location = new Point(820, 80),
                    Size = new Size(150, 200),
                    Font = new Font("Arial", 20, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                this.Controls.Add(userBoxLabel);
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
            BoxValues.Remove(Int32.Parse(button.Text) - 1); // Remove the box value from the dictionary
            if (NumOfBoxesClicked == BankerInterval && NumOfBoxesClicked < 18)
            {
                BankerOffer();
                addition = 3;
            }
            else if (NumOfBoxesClicked == BankerInterval && NumOfBoxesClicked >= 18)
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
            float value = BoxValues[boxNumber];
            String text = GetMoneyText(value);
            Color bgColor = Color.Blue;
            if (value > 750)
            {
                bgColor = Color.Red;
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
            if (previousLabel != null)
            {
                previousLabel.Dispose();
            }
            previousLabel = valueLabel;
        }

        private void BankerOffer()
        {
            textBox.Text = $"The banker is making an offer. {Values.Count}";
            float offer = CalculateBankerOffer();
            if (DialogResult.Yes == MessageBox.Show($"The banker offers you {GetMoneyText(offer)}. Do you accept?", "Banker's Offer",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                textBox.Text = $"You accepted the offer of {GetMoneyText(offer)}. Thank you for playing!";
                MessageBox.Show($"Congratulations! You won {GetMoneyText(offer)}!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else if (BoxValues.Count == 1)
            {
                float finalValue = BoxValues.First().Value;
                textBox.Text = $"You have only one box left. You win {GetMoneyText(finalValue)}. Thank you for playing!";
                MessageBox.Show($"Congratulations! You won {GetMoneyText(finalValue)}!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                textBox.Text = "You declined the offer. Please continue selecting boxes.";
            }
        }

        private float CalculateBankerOffer()
        {
            float bankerOffer = 0;
            foreach (float value in BoxValues.Values)
            {
                bankerOffer += value;
            }
            bankerOffer /= BoxValues.Count;
            bankerOffer *= 0.18f;
            bankerOffer = RoundToSignificantFigures(bankerOffer, 2);
            return bankerOffer;
        }

        private float RoundToSignificantFigures(float value, int significantFigures)
        {
            if (value == 0)
                return 0;
            float scale = (float)Math.Pow(10, significantFigures - (int)Math.Floor(Math.Log10(Math.Abs(value))));
            return (float)Math.Round(value * scale) / scale;
        }

        private String GetMoneyText(float value)
        {
            if (value < 1)
            {
                return $"{value * 100}p";
            }
            else
            {
                return $"£{value}";
            }
        }
    }
}
