using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace treeview
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
			if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
				return;
			// получаем выбранный файл
			string filename = openFileDialog1.FileName;
			// читаем файл в строку
			//string fileText = System.IO.File.ReadAllText(filename);
			OpenFilesToTreeView(filename, treeView1);
        }

        public void OpenFilesToTreeView (string NameFiles, TreeView treeview)
        {
            string lineFile;
            string[] tmplineFile;
            FileStream file1 = new FileStream(NameFiles, FileMode.Open);
            using (StreamReader readerFiles = new StreamReader(file1))
            {
                while ((lineFile = readerFiles.ReadLine()) != null)
                {
                    tmplineFile = new[] { lineFile };
                    PopulateTreeView(treeView1, tmplineFile);
                }
            }
        }

        void PopulateTreeView(TreeView treeView, string[] paths)
        {
            TreeNode lastNode = null;
            TreeNode[] nodes = new TreeNode[0];
            string subPathAgg;
            foreach (string path in paths)
            {
                subPathAgg = string.Empty;
                foreach (string subPath in path.Split('\\'))
                {
                    string[] temporary = path.Split('\\');
                    
                        subPathAgg += subPath + "\\";
                        nodes = treeView.Nodes.Find(subPathAgg, true);
                        if (nodes.Length == 0)
                            if (lastNode == null)
                                lastNode = treeView.Nodes.Add(subPathAgg, subPath, 0);
                            else
                                lastNode = lastNode.Nodes.Add(subPathAgg, subPath, 0);
                        else
                            lastNode = nodes[0];
                    
                }
            }
            lastNode = null;
            
        }
      public class MySortClass : IComparer
        {
 
            public int Compare(object _left, object _right)
            {
                TreeNode left = _left  as TreeNode;
                TreeNode right = _right as TreeNode;

				Regex leftRegex = new Regex(@"[a-z].*[0-9]*");
				Match leftMatch = leftRegex.Match(left.Text);

				Regex rightRegex = new Regex(@"[a-z].*[0-9]*");
				Match rightMatch = leftRegex.Match(right.Text);

				string lleftMatch = leftRegex.Match(left.Text).ToString();
				string rrightMatch = rightRegex.Match(right.Text).ToString();
				int value1;
				int.TryParse(string.Join("", lleftMatch.Where(c => char.IsDigit(c))), out value1);
				int value2;
				int.TryParse(string.Join("", rrightMatch.Where(c => char.IsDigit(c))), out value2);
				if (leftMatch.Success || rightMatch.Success)
				{
					if (leftMatch.Success && rightMatch.Success)
					{
						if (value1 > value2)
						{
							return 0;
						}
						if (value1 == value2)
						{
							return string.Compare(left.Text, right.Text);
						}
							return 1;
					}
			
					if (value1 > value2)
					{
						return 1;
					}
					return 0;
				}

				else
				{

					if (string.Compare(left.Text, right.Text) >= 0)
					{
						return 1;
					}
					else
					{

						return 0;

					}
				} 
            }
        }

		private void PrintRecursive(TreeNode treeNode,string filename)
		{
			//System.Diagnostics.Debug.WriteLine(treeNode.Text);
			//MessageBox.Show(treeNode.Text+ " " + treeNode.Parent + " " + treeNode.Level);
			int lev = treeNode.Level;
			string path = filename;
			
			if (File.Exists(path))
			{
				string s = treeNode.Text.Insert(0, new string(' ', lev));
				File.AppendAllText(path, s + Environment.NewLine, Encoding.UTF8);
				//sw.WriteLine(treeNode.Text.PadLeft(lev, pad));
			}
			else {
				FileStream file1 = new FileStream(path, FileMode.OpenOrCreate);
				file1.Close();
			}
				foreach (TreeNode tn in treeNode.Nodes)
			{
				PrintRecursive(tn, filename);
			}
		
		}

		private void CallRecursive(TreeView treeView)
		{
			if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
				return;
			// получаем выбранный файл
			string filename = saveFileDialog1.FileName;
			TreeNodeCollection nodes = treeView.Nodes;
			foreach (TreeNode n in nodes)
			{
				PrintRecursive(n, filename);
			}
		}
		private void Button3_Click(object sender, EventArgs e)
        {
			treeView1.TreeViewNodeSorter = new MySortClass();

		}
		private void Button4_Click(object sender, EventArgs e)
		{
			CallRecursive(treeView1);
		}
	}
}
