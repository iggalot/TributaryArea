using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MathLibrary.MathVectors;

namespace TribAreaApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            OnUserCreate();
        }

        private void OnUserCreate()
        {
            Node node1 = new Node(new Vec3D(100, 250, 0));
            Node node2 = new Node(new Vec3D(300, 50, 0));

            Beam beam1 = new Beam(node1.Loc, node2.Loc);

            beam1.Draw(MainCanvas);


        }

        internal class Beam {
            public Node Start { get; set; }
            public Node End { get; set; }
            public Node MiddleNode { get; set; }

            public Vec3D RafterDirBelow { get; set; }
            public Vec3D RafterDirAbove { get; set; }

            public Beam(Vec3D n1, Vec3D n2)
            {
                Start = new Node(n1);
                End = new Node(n2);
                MiddleNode = new Node(new Vec3D((float)0.5 * (n1.X + n2.X), (float)0.5 * (n1.Y + n2.Y), (float)0.5 * (n1.Z + n2.Z)));

                Vec3D belowMathOps = MathOps.Vec_Sub(End.Loc, Start.Loc);
                belowMathOps = MathOps.Vec_Normalize(belowMathOps);
                RafterDirBelow = MathOps.Vec_Normalize(MathOps.Vec_CrossProduct(belowMathOps, new Vec3D(0, 0, 1)));
                RafterDirAbove = MathOps.Vec_Normalize(MathOps.Vec_CrossProduct(new Vec3D(0, 0, 1), belowMathOps));
            }
            public void Draw(Canvas c)
            {
                DrawingHelpersLibrary.DrawingHelpers.DrawLine(c, Start.Loc.X, Start.Loc.Y, End.Loc.X, End.Loc.Y, Brushes.Black);
                Start.Draw(c, Brushes.Blue);
                End.Draw(c, Brushes.Green);
                MiddleNode.Draw(c, Brushes.Red);

                // Draw Rafters Below direction
                float length = 100;
                Vec3D end = new Vec3D((float)(length * RafterDirBelow.X), length * RafterDirBelow.Y, length * RafterDirBelow.Z);
                Vec3D pt1 = MathOps.Vec_Add(MiddleNode.Loc, end);
                DrawingHelpersLibrary.DrawingHelpers.DrawLine(c, MiddleNode.Loc.X, MiddleNode.Loc.Y, pt1.X, pt1.Y, Brushes.Blue);

                // Draw Rafter Above direction
                end = new Vec3D((float)(length * RafterDirAbove.X), length * RafterDirAbove.Y, length * RafterDirAbove.Z);
                pt1 = MathOps.Vec_Add(MiddleNode.Loc, end);
                DrawingHelpersLibrary.DrawingHelpers.DrawLine(c, MiddleNode.Loc.X, MiddleNode.Loc.Y, pt1.X, pt1.Y, Brushes.Red);
            }
        }

        internal class Node
        {
            public Vec3D Loc { get; set; }
            public Node(Vec3D pt)
            {
                Loc = pt;
            }

            public void Draw(Canvas c, Brush fill)
            {
                DrawingHelpersLibrary.DrawingHelpers.DrawCircle(c, Loc.X, Loc.Y, fill, Brushes.Black, 10, 1);
            }
        }
    }
}
