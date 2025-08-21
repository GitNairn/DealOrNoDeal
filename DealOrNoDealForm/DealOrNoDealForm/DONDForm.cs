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
        public DONDForm()
        {
            InitializeComponent();
            Load += DONDForm_Load;
        }

        private void DONDForm_Load(object sender, EventArgs e)
        {
            // Initialize the form or load resources here if needed
            this.BackColor = Color.Black; // Example of setting a property
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
            if (button != null)
            {
                int boxNumber = Int32.Parse(button.Text) - 1; // Convert to zero-based index
                ShowValue(boxNumber, button);
                button.Dispose();
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
            if (value < 1)
            {
                value *= 100;
                text = $"{value}p";

            }
            else
            {
                text = $"£{value}";
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
        }
    }
}
