﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSharpGL;

namespace Texture2DArray
{
    public partial class FormMain : Form
    {
        private Scene scene;
        private ActionList actionList;

        public FormMain()
        {
            InitializeComponent();

            this.Load += FormMain_Load;
            this.winGLCanvas1.OpenGLDraw += winGLCanvas1_OpenGLDraw;
            this.winGLCanvas1.Resize += winGLCanvas1_Resize;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            SceneNodeBase rootElement = GetRootElement();

            var position = new vec3(1, 0, 4);
            var center = new vec3(0, 0, 0);
            var up = new vec3(0, 1, 0);
            var camera = new Camera(position, center, up, CameraType.Perspecitive, this.winGLCanvas1.Width, this.winGLCanvas1.Height);
            this.scene = new Scene(camera, this.winGLCanvas1)
            {
                RootElement = rootElement,
                ClearColor = Color.SkyBlue.ToVec4(),
            };

            var list = new ActionList();

            var transformAction = new TransformAction(scene);
            list.Add(transformAction);
            var renderAction = new RenderAction(scene);
            list.Add(renderAction);

            var guiLayoutAction = new GUILayoutAction(scene);
            list.Add(guiLayoutAction);
            var guiRenderAction = new GUIRenderAction(scene);
            list.Add(guiRenderAction);

            this.actionList = list;

            Match(this.trvScene, scene.RootElement);
            this.trvScene.ExpandAll();


        }

        private WinCtrlRoot GetRootControl()
        {
            var root = new WinCtrlRoot(this.winGLCanvas1.Width, this.winGLCanvas1.Height);

            var bitmap = new Bitmap(@"particle.png");
            {
                var control = new CtrlImage(bitmap, false);
                control.Margin = new GUIPadding(10, 10, 10, 10);
                control.Width = 100; control.Height = 50;
                bitmap.Dispose();
                root.Children.Add(control);
            }
            {
                var control = new CtrlButton() { Anchor = GUIAnchorStyles.Left | GUIAnchorStyles.Bottom };
                control.Margin = new GUIPadding(10, 10, 10, 70);
                control.Width = 100; control.Height = 50;
                root.Children.Add(control);
                control.Focused = true;
            }

            return root;
        }

        private SceneNodeBase GetRootElement()
        {
            var bmps = new Bitmap[5];
            for (int i = 0; i < bmps.Length; i++)
            {
                bmps[i] = new Bitmap(string.Format("{0}.png", i));
            }

            var node = LayeredRectangleNode.Create(bmps);
            return node;
        }

        private void Match(TreeView treeView, SceneNodeBase nodeBase)
        {
            treeView.Nodes.Clear();
            var node = new TreeNode(nodeBase.ToString()) { Tag = nodeBase };
            treeView.Nodes.Add(node);
            Match(node, nodeBase);
        }

        private void Match(TreeNode node, SceneNodeBase nodeBase)
        {
            foreach (var item in nodeBase.Children)
            {
                var child = new TreeNode(item.ToString()) { Tag = item };
                node.Nodes.Add(child);
                Match(child, item);
            }
        }

        private void winGLCanvas1_OpenGLDraw(object sender, PaintEventArgs e)
        {
            this.actionList.Act();
        }

        void winGLCanvas1_Resize(object sender, EventArgs e)
        {
            this.scene.Camera.AspectRatio = ((float)this.winGLCanvas1.Width) / ((float)this.winGLCanvas1.Height);
        }

        private void trvScene_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.propGrid.SelectedObject = e.Node.Tag;

            this.lblState.Text = string.Format("{0} objects selected.", 1);
        }
    }
}
