using System;
using System.Windows.Forms;
using System.IO;

namespace ImageViewer
{
    class TreeControl
    {
        Program MainApp;

        TreeView DirTree;
        TreeNode root;

        private static string[] all_dir;
        public static string[] All_dir
        {
            get
            {
                return all_dir;
            }
            set
            {
                all_dir = value;
            }
        }

        public TreeControl(Program main)
        {
            MainApp = main;

            DirTree = new TreeView();
            DirTree.Name = "DirTree";
            All_dir = Directory.GetLogicalDrives();

            foreach (string file in all_dir)
            {
                try
                {
                    root = new TreeNode(file);
                    root.Name = file.ToString();
                    DirTree.Nodes.Add(root);
                    Directory_Searching(file, all_dir, root);
                }
                catch (UnauthorizedAccessException e)
                { }
            }
            SizeInitialize();
            DirTree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(TreeNodeMouseDoubleClick);
            DirTree.AfterSelect += new TreeViewEventHandler(TreeSelect);
            MainApp.ClientSizeChanged += new EventHandler(ClientResize);

            MainApp.Controls.Add(DirTree);
        }

        private void ClientResize(object sender, EventArgs e)
        {
            SizeInitialize();
        }

        private void SizeInitialize()
        {
            DirTree.Width = (int)(MainApp.Width * 0.104);
            DirTree.Left = (int)(MainApp.Width * 0.042);
            DirTree.Height = (int)(MainApp.Height * 0.5);
            DirTree.Top = (int)(MainApp.Height * 0.104);
        }

        private void Directory_Searching(string path, string[] dir, TreeNode root)
        {
            if (Directory.GetDirectories(path) != null)
            {
                dir = null;
                dir = Directory.GetDirectories(path);
                string[] dir2 = null;

                foreach (string file in dir)
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(file);

                        TreeNode child = new TreeNode(di.Name);
                        child.Name = di.Name;
                        if (root.Nodes.Find(child.Name, false).Length == 0)
                            root.Nodes.Add(child);
                        dir2 = Directory.GetDirectories(file);
                        foreach (string child_dir in dir2)
                        {
                            DirectoryInfo di2 = new DirectoryInfo(child_dir);
                            TreeNode grandchild = new TreeNode(di2.Name);
                            grandchild.Name = di2.Name;

                            TreeNode[] Tarray = root.Nodes.Find(child.Name, false);
                            if (Tarray[0].Nodes.Find(grandchild.Name, false).Length == 0)
                                Tarray[0].Nodes.Add(grandchild);
                            else
                                child.Nodes.Add(grandchild);
                        }
                    }
                    catch (UnauthorizedAccessException e)
                    { }
                }
            }
        }
        
        private void TreeNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string nodeKey = e.Node.FullPath;
            Console.WriteLine("NodeName : {0}", nodeKey);

        }

        private void TreeSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string nodeKey = e.Node.FullPath;

                Directory_Searching(nodeKey, All_dir, e.Node);
                ImageControl.All_Img = Directory.GetFiles(nodeKey, "*.jpg", SearchOption.AllDirectories);
                ImageControl.ThreadSleep = false;
                
            }
            catch (UnauthorizedAccessException error)
            { }
            catch (FormatException error)
            { }
        }
    }
}
