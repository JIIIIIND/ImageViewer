using System;
using System.Windows.Forms;
using System.IO;
using System.Collections;

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
        private ArrayList directory;

        public TreeControl(Program main)
        {
            MainApp = main;
            directory = new ArrayList();
            DirTree = new TreeView();
            DirTree.Name = "DirTree";
            All_dir = Directory.GetLogicalDrives();
            foreach(string path in All_dir)
            {
                try
                {
                    string[] children = Directory.GetDirectories(path);
                    directory.Add(path);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            All_dir = ListToArray(directory);

            foreach (string file in all_dir)
            {
                try
                {
                    root = new TreeNode(file);
                    root.Name = file.ToString();
                    
                    Directory_Searching(file, all_dir, root);
                    DirTree.Nodes.Add(root);
                }
                catch (UnauthorizedAccessException e)
                {
                }
                catch (IOException e)
                {
                }
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
            try
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
            catch (IOException e)
            {
                
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
            catch (IOException error)
            {
                MessageBox.Show("This Path is not valid", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string[] ListToArray(ArrayList list)
        {
            string[] result = new string[list.Count];
            for(int i = 0; i < list.Count; i++)
            {
                result[i] = (string)list[i];
            }
            return result;
        }
    }
}
