using System;
using System.Threading;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupCustomComponents();
        }

        private void SetupCustomComponents()
        {
            dataGridView1.Columns.Add("x", "Число x");
            dataGridView1.Columns.Add("y", "Результат f(x)");

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 10;
            progressBar1.Value = 0;

            timer1.Interval = 1000;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            dataGridView1.Rows.Clear();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value++;
                int x = progressBar1.Value - 3;

                Thread t = new Thread(new ParameterizedThreadStart(CalculateAndDisplay));
                t.Start(x);
            }
            else
            {
                timer1.Stop();
            }
        }

        private void CalculateAndDisplay(object obj)
        {
            int x = (int)obj;
            FunctionCalculator calculator = new FunctionCalculator();
            string resultString;

            try
            {
                double result = calculator.Calculate(x);
                resultString = Math.Round(result, 4).ToString();
            }
            catch (DivideByZeroException)
            {
                resultString = "Помилка: ділення на нуль";
            }
            catch (Exception)
            {
                resultString = "Невідома помилка";
            }

            this.Invoke(new Action(() =>
            {
                dataGridView1.Rows.Add(x, resultString);
            }));
        }
    }

    public class FunctionCalculator
    {
        public double Calculate(int x)
        {
            if (x >= 2)
            {
                return 0.6 * x;
            }
            else
            {
                if (x == 0)
                {
                    throw new DivideByZeroException();
                }
                return 2.0 / (3 * x);
            }
        }
    }
}