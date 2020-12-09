using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace lifesim
{
    public partial class Form1 : Form
    {
        private bool start = true;
        int mouseX = 0;
        int mouseY = 0;
        Graphics g;
        Timer Timer1 = new Timer();
        Timer loopTimer = new Timer();
        square[][] sqs;
        bool[][] checkedSq;
        List<square> addList = new List<square>();
        List<square> delList = new List<square>();
        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            sqs = new square[ClientRectangle.Width / 10][];
            checkedSq = new bool[ClientRectangle.Width / 10][];
            for (int i = 0; i < ClientRectangle.Width / 10; i++)
            {
                sqs[i] = new square[ClientRectangle.Height / 10];
                checkedSq[i] = new bool[ClientRectangle.Height / 10];
                for(int j = 0; j < ClientRectangle.Height / 10; j++)
                {
                    checkedSq[i][j] = false;
                }
            }
            square sq = new square(2, 3);
            square sq1 = new square(2, 2);
            square sq2 = new square(3, 2);
            addGlider(4, 5, false, false);

            addGlider(11, 5, true, false);

            addGlider(18, 5, true, true);
            addGlider(100, 100, false, false);
            addGlider(25, 5, false, true);
            putSquare(sq);
            //putSquare(sq1);
            putSquare(sq2);
            this.Paint += new System.Windows.Forms.PaintEventHandler(Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(Form1_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(Form1_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(Form1_MouseMove);
        }

        private void addGlider(int x, int y, bool reverseX, bool reverseY)
        {
            int rvX = 1;
            int rvY = 1;
            if (reverseX)
            {
                rvX = -1;
            }
            if (reverseY)
            {
                rvY = -1;
            }
            newSquare(x, y + (rvY * 2));
            newSquare(x + (rvX * 1), y + (rvY * 2));
            newSquare(x + (rvX * 2), y + (rvY * 2));
            newSquare(x + (rvX * 2), y + (rvY * 1));
            newSquare(x + (rvX * 1), y);
        }

        private void newSquare(int x, int y)
        {
            square sq = new square((x + ClientRectangle.Width / 10) % (ClientRectangle.Width / 10), (y + ClientRectangle.Height / 10) % (ClientRectangle.Height / 10));
            putSquare(sq);
        }

        private void putSquare(square sq)
        {
            addList.Add(sq);
            Point p = sq.getPoint();
            sqs[p.X][p.Y] = sq;
        }
        private void InitializeTimer()
        {
            // Call this procedure when the application starts.  
            // Set to 1 second.  
            Timer1.Interval = 100;
            loopTimer.Interval = 50;
            loopTimer.Tick += new EventHandler(loopTimer_Tick);
            Timer1.Tick += new EventHandler(Timer1_Tick);

            // Enable timer.  
            //Timer1.Enabled = true;
        }
        private void loopTimer_Tick(object Sender, EventArgs e)
        {
            int xPos = mouseX;
            int yPos = mouseY;
            if (checkedSq[xPos / 10][yPos / 10] == false)
            {
                if (sqs[xPos / 10][yPos / 10] == null)
                {
                    Console.WriteLine("new sq");
                    square newSq = new square(xPos / 10, yPos / 10);
                    addList.Add(newSq);
                    this.Invalidate(getRect(newSq));
                }
                else
                {
                    Console.WriteLine("del sq");
                    square newSq = new square(xPos / 10, yPos / 10);
                    delList.Add(newSq);
                    this.Invalidate(getRect(newSq));
                }
                checkedSq[xPos / 10][yPos / 10] = true;
            }
            
        }
        private void Timer1_Tick(object Sender, EventArgs e)
        {
            addList = new List<square>();
            delList = new List<square>();
            for (int i = 0; i < sqs.Length; i++)
            {
                for (int j = 0; j < sqs[1].Length; j++)
                {
                    if (sqs[i][j] != null)
                    {
                        square currSq = sqs[i][j];
                        if (cellDies(i, j))
                        {
                            currSq.setEnabled(false);
                            this.Invalidate(getRect(currSq));
                            delList.Add(currSq);
                        }
                        else
                        {
                            addList.Add(currSq);
                        }
                    }
                    else
                    {
                        if (cellCreated(i, j))
                        {
                            square newSq = new square(i, j);
                            addList.Add(newSq);
                            this.Invalidate(getRect(newSq));
                        }
                    }
                }
            }
            this.Update();
        }
        private Rectangle getRect(square sq)
        {
            Point sqPt = sq.getPoint();
            Point pt = new Point(sqPt.X * 10 + 1, sqPt.Y * 10 + 1);
            Size sz = new Size(9, 9);
            Rectangle r = new Rectangle(pt, sz);
            return r;
        }

        private bool cellDies(int x, int y)
        {
            int num = numNeighbors(x, y);
            if (num == 2 || num == 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int numNeighbors(int x, int y)
        {
            int total = -1;
            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    int xPos = (i + ClientRectangle.Width / 10) % (ClientRectangle.Width / 10);
                    int yPos = (j + ClientRectangle.Height / 10) % (ClientRectangle.Height / 10);
                    if (sqs[xPos][yPos] != null)
                    {
                        total++;
                    }
                }
            }
            if (total > 0)
            {
                Console.WriteLine(total);
            }
            return total;
        }

        private bool cellCreated(int x, int y)
        {
            int num = numNeighbors(x, y);
            if (num == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void addSquare(square sq)
        {
            Point p = sq.getPoint();
            int x = p.X;
            int y = p.Y;
            sqs[x][y] = sq;
            fillSquare(x, y);
        }

        private void removeSquare(square sq)
        {
            Point p = sq.getPoint();
            int x = p.X;
            int y = p.Y;
            sqs[x][y] = null;
            deleteSquare(x, y);
        }
        private int getNeighbors(square sq, List<square> sqs)
        {
            int total = 0;
            foreach (square s in sqs)
            {
                if (sq.IsNeighbor(s.getPoint()))
                {
                    total++;
                }
            }
            return total;
        }

        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs pe)
        {
            g = pe.Graphics;
            if (start)
            {
                Pen p = new Pen(Color.Black);
                int y = ClientRectangle.Width;
                int x = ClientRectangle.Height;
                for (int i = 0; i < y; i += 10)
                {
                    g.DrawLine(p, i, 0, i, y * 2);
                }
                for (int j = 0; j < x; j += 10)
                {
                    g.DrawLine(p, 0, j, x * 2, j);
                }
                start = false;
            }
            foreach (square sq in addList)
            {
                addSquare(sq);
            }
            foreach (square sq in delList)
            {
                removeSquare(sq);
            }
            addList = new List<square>();
            delList = new List<square>();
            Console.WriteLine("paint");
        }


        public void deleteSquare(int x, int y)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush br = new SolidBrush(Control.DefaultBackColor);
            Point pt = new Point(x * 10 + 1, y * 10 + 1);
            Size sz = new Size(9, 9);
            Rectangle r = new Rectangle(pt, sz);
            g.FillRectangle(br, r);
        }

        private void Form1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }
        public void fillSquare(int x, int y)
        {
            Pen p = new Pen(Color.Black);
            SolidBrush br = new SolidBrush(Color.Black);
            Point pt = new Point(x * 10 + 1, y * 10 + 1);
            Size sz = new Size(9, 9);
            Rectangle r = new Rectangle(pt, sz);
            g.FillRectangle(br, r);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Timer1.Enabled == false)
            {
                Timer1.Enabled = true;
                button1.Text = "Stop";
            }
            else
            {
                Timer1.Enabled = false;
                button1.Text = "Start";
            }
        }

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (loopTimer.Enabled == false)
            {
                loopTimer.Enabled = true;
            }
        }

        private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(loopTimer.Enabled == true)
            {
                loopTimer.Enabled = false;
                for (int i = 0; i < ClientRectangle.Width / 10; i++)
                {
                    for (int j = 0; j < ClientRectangle.Height / 10; j++)
                    {
                        checkedSq[i][j] = false;
                    }
                }
                this.Update();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Timer1.Enabled == false)
            {
                for (int i = 0; i < ClientRectangle.Width / 10; i++)
                {
                    for (int j = 0; j < ClientRectangle.Height / 10; j++)
                    {
                        if (sqs[i][j] != null)
                        {
                            addList.Add(sqs[i][j]);
                        }
                    }
                }
                start = true;
                this.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Timer1.Enabled == false)
            {
                for (int i = 0; i < ClientRectangle.Width / 10; i++)
                {
                    for (int j = 0; j < ClientRectangle.Height / 10; j++)
                    {
                        if (sqs[i][j] != null)
                        {
                            delList.Add(sqs[i][j]);
                        }
                    }
                }
                start = true;
                this.Refresh();
            }
        }
    }
}
