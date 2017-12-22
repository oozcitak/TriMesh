using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MeshTest
{
    public partial class TestForm : Form
    {
        private class MeshAnim
        {
            private static float featureSize = 1;

            public string Name;
            public SimpleCAD.Drawable NewVertex;
            public List<SimpleCAD.Drawable> Mesh;
            public List<SimpleCAD.Drawable> RedTri;
            public List<SimpleCAD.Drawable> GreenTri;
            public SimpleCAD.Drawable Edge;

            public MeshAnim(string name, TriMesh.Vertex vertex, List<TriMesh.Triangle> mesh, List<TriMesh.Triangle> reds, List<TriMesh.Triangle> greens, TriMesh.Halfedge edge)
            {
                Name = name;
                if (vertex != null)
                {
                    NewVertex = new SimpleCAD.Circle(vertex.ToPointF(), featureSize);
                    NewVertex.FillStyle = SimpleCAD.FillStyle.Yellow;
                    NewVertex.OutlineStyle = SimpleCAD.OutlineStyle.Black;
                }
                if (mesh != null)
                {
                    Mesh = new List<SimpleCAD.Drawable>();
                    foreach (TriMesh.Triangle t in mesh)
                    {
                        Mesh.AddRange(DrawTri(t, SimpleCAD.OutlineStyle.Black, SimpleCAD.FillStyle.LightGray));
                    }
                }
                if (reds != null)
                {
                    RedTri = new List<SimpleCAD.Drawable>();
                    foreach (TriMesh.Triangle t in reds)
                    {
                        RedTri.AddRange(DrawTri(t, SimpleCAD.OutlineStyle.Black, SimpleCAD.FillStyle.LightCoral));
                    }
                }
                if (greens != null)
                {
                    GreenTri = new List<SimpleCAD.Drawable>();
                    foreach (TriMesh.Triangle t in greens)
                    {
                        GreenTri.AddRange(DrawTri(t, SimpleCAD.OutlineStyle.Black, SimpleCAD.FillStyle.LightGreen));
                    }
                }
                if (edge != null)
                {
                    Edge = new SimpleCAD.Line(edge.V1.ToPointF(), edge.V2.ToPointF());
                    Edge.OutlineStyle = new SimpleCAD.OutlineStyle(Color.Blue, 3);
                }
            }

            private static List<SimpleCAD.Drawable> DrawTri(TriMesh.Triangle t, SimpleCAD.OutlineStyle pen, SimpleCAD.FillStyle fill)
            {
                SimpleCAD.Triangle viewtri = new SimpleCAD.Triangle(t.V1.ToPointF(), t.V2.ToPointF(), t.V3.ToPointF());
                viewtri.FillStyle = fill;
                viewtri.OutlineStyle = pen;

                return new List<SimpleCAD.Drawable> { viewtri };
            }
        }

        bool animate = false;
        int animDir = 0;
        int keyDownCount = 0;
        SimpleCAD.Drawable selected = null;
        TriMesh.DelaunayMesh mesh = null;
        int step = 0;
        int maxStep = 0;
        List<MeshAnim> anim = new List<MeshAnim>();
        float featureSize = 1;

        public TestForm()
        {
            InitializeComponent();

            // Mesh
            mesh = new TriMesh.DelaunayMesh();
            mesh.DividingTriangle += Mesh_DividingTriangle;
            mesh.DividedTriangle += Mesh_DividedTriangle;
            mesh.FlippingEdge += Mesh_FlippingEdge; mesh.FlippedEdge += Mesh_FlippedEdge;
            mesh.InsertVertex += Mesh_InsertVertex;
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                double x = r.NextDouble() * 100;
                double y = r.NextDouble() * 100;
                double z = r.NextDouble() * 10;
                mesh.AddVertex(x, y, z);
            }
            anim.Add(new MeshAnim("Started", null, null, null, null, null));
            mesh.Triangulate();
            statusLabel.Text = "Created " + mesh.Triangles.Count + " triangles from " + mesh.InputVertices.Count + " input vertices in " + mesh.ElapsedTime.TotalSeconds.ToString("0.00") + " seconds.";

            anim.Add(new MeshAnim("Completed", null, mesh.Triangles.ToList(), null, null, null));

            step = 0;
            maxStep = anim.Count - 1;

            UpdateModel();

            // View all
            meshView.View.ZoomToExtents();
        }

        private void Mesh_FlippedEdge(object sender, TriMesh.FlippedEdgeEventArgs e)
        {
            if(animate)
                anim.Add(new MeshAnim("Flipped", null, e.CurrentMesh.ToList(), null, new List<TriMesh.Triangle>() { e.NewTriangle1, e.NewTriangle2 }, e.Edge));
        }

        private void Mesh_FlippingEdge(object sender, TriMesh.FlippingEdgeEventArgs e)
        {
            if (animate)
                anim.Add(new MeshAnim("Flipping", null, e.CurrentMesh.ToList(), new List<TriMesh.Triangle>() { e.BadTriangle1, e.BadTriangle2 }, null, e.Edge));
        }

        private void Mesh_DividingTriangle(object sender, TriMesh.DividingTriangleEventArgs e)
        {
            if (animate)
                anim.Add(new MeshAnim("Dividing", e.NewVertex, e.CurrentMesh.ToList(), e.Triangles.ToList(), null, null));
        }

        private void Mesh_DividedTriangle(object sender, TriMesh.DividedTriangleEventArgs e)
        {
            if (animate)
                anim.Add(new MeshAnim("Divided", e.NewVertex, e.CurrentMesh.ToList(), null, e.NewTriangles.ToList(), null));
        }

        private void Mesh_InsertVertex(object sender, TriMesh.InsertVertexEventArgs e)
        {
            if (animate)
                anim.Add(new MeshAnim("Vertex Inserted", e.NewVertex, e.CurrentMesh.ToList(), null, null, null));
        }

        private void UpdateModel()
        {
            meshView.Model.Clear();

            if (maxStep == 0)
            {
                Text = "Mesh Test";
            }
            else
            {
                // Triangles
                if (anim[step].Mesh != null)
                {
                    foreach (SimpleCAD.Drawable t in anim[step].Mesh)
                    {
                        meshView.Model.Add(t);
                    }
                }

                // Red triangles
                if (anim[step].RedTri != null)
                {
                    foreach (SimpleCAD.Drawable t in anim[step].RedTri)
                    {
                        if (ReferenceEquals(t, selected)) continue;

                        meshView.Model.Add(t);
                    }
                }

                // Green triangles
                if (anim[step].GreenTri != null)
                {
                    foreach (SimpleCAD.Drawable t in anim[step].GreenTri)
                    {
                        if (ReferenceEquals(t, selected)) continue;

                        meshView.Model.Add(t);
                    }
                }

                // Selected triangle
                if (selected != null)
                {
                    meshView.Model.Add(selected);
                }

                // Flip edge
                if (anim[step].Edge != null)
                {
                    meshView.Model.Add(anim[step].Edge);
                }

                // Vertices
                if (anim[step].NewVertex != null)
                {
                    meshView.Model.Add(anim[step].NewVertex);
                }

                foreach (TriMesh.Vertex v in mesh.Vertices)
                {
                    SimpleCAD.Circle cir = new SimpleCAD.Circle((float)v.X, (float)v.Y, featureSize);
                    cir.FillStyle = SimpleCAD.FillStyle.Transparent;
                    cir.OutlineStyle = SimpleCAD.OutlineStyle.Black;
                    meshView.Model.Add(cir);
                }

                Text = "Mesh Test (Step " + (step + 1).ToString() + "/" + (maxStep + 1).ToString() + ") - " + anim[step].Name;
            }

            meshView.Refresh();
        }

        private void meshView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                animDir = -1;
                keyDownCount = 0;
                timerAnim.Interval = 100;
            }
            else if (e.KeyCode == Keys.Right)
            {
                animDir = 1;
                keyDownCount = 0;
                timerAnim.Interval = 100;
            }
        }

        private void meshView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && animDir == -1)
            {
                if (keyDownCount == 0) timerAnim_Tick(null, null);
                animDir = 0;
            }
            else if (e.KeyCode == Keys.Right && animDir == 1)
            {
                if (keyDownCount == 0) timerAnim_Tick(null, null);
                animDir = 0;
            }
        }

        private void timerAnim_Tick(object sender, EventArgs e)
        {
            if (animDir != 0)
            {
                keyDownCount++;
                if (keyDownCount == 25)
                    timerAnim.Interval = 50;
                else if (keyDownCount == 50)
                    timerAnim.Interval = 25;
                else if (keyDownCount == 100)
                    timerAnim.Interval = 10;

                step += animDir;
                if (step == -1) step = maxStep;
                if (step == maxStep + 1) step = 0;
                selected = null;
                UpdateModel();
            }
        }

        private void meshView_MouseMove(object sender, MouseEventArgs e)
        {
            PointF pt = meshView.View.ScreenToWorld(e.Location);
            coordLabel.Text = pt.X.ToString("F2") + ", " + pt.Y.ToString("F2");
        }

        private void meshView_ItemClick(object sender, SimpleCAD.ItemClickEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.None)
            {
                selected = null;
                PointF pt = meshView.View.ScreenToWorld(e.Location);
                foreach (SimpleCAD.Drawable d in anim[step].Mesh)
                {
                    SimpleCAD.Triangle t = d as SimpleCAD.Triangle;
                    if (t != null && t.Contains(pt))
                    {
                        selected = t;
                        break;
                    }
                }
                UpdateModel();
            }
        }

        private void meshView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                e.IsInputKey = true;
        }
    }

    public static class Extensions
    {
        public static PointF ToPointF(this TriMesh.Vertex v)
        {
            return new PointF((float)v.X, (float)v.Y);
        }
    }
}
