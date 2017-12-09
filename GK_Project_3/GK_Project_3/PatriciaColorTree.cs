

using System;
using System.Windows.Media;

namespace GK_Project_3
{
    public class RSTColorTree
    {
        int maxb = 8;
        private Node root;

        private class Node
        {
            public Color Value;
            public Color NewColor;
            public Node[] next=new Node[8];
        }

        public Color GetNewColor(Color oldColor)
        {
            int branch = 0;
            Node p = Search(oldColor, out branch);
            if(p!=null)
            {
                if (branch >= 0 && p.next[branch] != null)
                    return p.next[branch].NewColor;
                else if (branch < 0)
                    return p.NewColor;
            }
            return Colors.Black;
        }

        byte Bit(Color c,int bit)
        {
            byte R = c.R;
            byte G = c.G;
            byte B = c.B;

            return (byte)(Bit(R,bit)+Bit(G,bit)+Bit(B,bit));
        }

        int Bit(byte val,int bit)
        {
            int mask = 1 <<bit;
            int masked_n = val & mask;
            int thebit = masked_n >> bit;
            return thebit;
        }

        
        private  Node Search(Color color,out int branch)
        {
            int b = maxb;
            Node p = root;
            branch = -1;
            if (p == null || p.Value==color)
                return p;

            branch = Bit(color, b--);
            while (p.next[branch]!=null && p.next[branch].Value!=color)
            {
                p = p.next[branch];
                branch = Bit(color, b--);
            }

            return p;
        }

        public void Insert(Color value, Color newColor)
        {
            int branch = 0;
            Node p = Search(value, out branch);
            if(p==null)
            {
                root = new Node() { Value = value, NewColor = newColor };
                return;
            }
            if(branch<0)
            {
                p.NewColor = newColor;
                return;
            }
            if( branch>=0 && p.next[branch]!=null)
            {
                p.next[branch].NewColor = newColor;
                return;
            }
            p.next[branch] = new Node() { Value = value, NewColor = newColor };
        }
    }
}
