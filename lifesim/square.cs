using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace lifesim
{
    class square
    {
        private Point pt;
        private bool enabled = true;
        public square(Point pt1)
        {
            pt = pt1;
        }

        public square(int x, int y)
        {
            pt = new Point(x, y);
        }

        public Boolean IsNeighbor(Point pt2)
        {
            if ((pt2.X != pt.X && pt2.Y != pt.Y))
            {
                return (Math.Abs(pt.X - pt2.X) <= 1) && (Math.Abs(pt.Y - pt2.Y) <= 1);
            }
            else
            {
                return false;
            }
        }

        public bool getEnabled()
        {
            return enabled;
        }

        public int getX()
        {
            return pt.X;
        }

        public int getY()
        {
            return pt.Y;
        }

        public Point getPoint()
        {
            return pt;
        }

        public void setEnabled(bool b)
        {
            enabled = b;
        }
    }
}
