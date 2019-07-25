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
using System.Xml;

namespace LoopThroughXmlDocument
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private const string booksFile = @"C:\BegVCSharp\BOOKS.xml";
        private const string booksFile = @"C:\VSRepositories\LoopThroughXmlDocument\BOOKS.xml";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonLoop_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load(booksFile);
            textBlockResults.Text = FormatText(document.DocumentElement as XmlNode, "", "");
        }

        private string FormatText(XmlNode node, string text, string indent)
        {
            if (node is XmlText)
            {
                text += node.Value;
                return text;
            }
            if (string.IsNullOrEmpty(indent))
                indent = "";
            else
            {
                text += "\r\n" + indent;
            }
            if (node is XmlComment)
            {
                text += node.OuterXml;
                return text;
            }
            text += "<" + node.Name;
            if (node.Attributes.Count > 0)
            {
                AddAttributes(node, ref text);
            }
            if (node.HasChildNodes)
            {
                text += ">";
                foreach (XmlNode child in node.ChildNodes)
                {
                    text = FormatText(child, text, indent + " ");
                }
                if (node.ChildNodes.Count == 1 &&
                (node.FirstChild is XmlText || node.FirstChild is XmlComment))
                    text += "</" + node.Name + ">";
                else
                    text += "\r\n" + indent + "</" + node.Name + ">";
            }
            else
                text += " />";
            return text;
        }

        private void AddAttributes(XmlNode node, ref string text)
        {
            foreach (XmlAttribute xa in node.Attributes)
            {
                text += " " + xa.Name + "='" + xa.Value + "'";
                //value xmlAttributes
            }
        }

        private void ButtonCreateNode_Click(object sender, RoutedEventArgs e)
        {
            // Load the XML document.
            XmlDocument document = new XmlDocument();
            document.Load(booksFile);
            // Get the root element.
            XmlElement root = document.DocumentElement;
            // Create the new nodes.
            XmlElement newBook = document.CreateElement("book");
            XmlElement newTitle = document.CreateElement("title");
            XmlElement newAuthor = document.CreateElement("author");
            XmlElement newCode = document.CreateElement("code");
            XmlText title = document.CreateTextNode("Beginning Visual C# 2010");
            XmlText author = document.CreateTextNode("Karli Watson et al7");
            XmlText code = document.CreateTextNode("1234567890");
            XmlComment comment = document.CreateComment("The previous edition");
            // Insert the elements.
            newBook.AppendChild(comment);
            newBook.AppendChild(newTitle);
            newBook.AppendChild(newAuthor);
            newBook.AppendChild(newCode);
            newTitle.AppendChild(title);
            newAuthor.AppendChild(author);
            newCode.AppendChild(code);
            root.InsertAfter(newBook, root.FirstChild);
            document.Save(booksFile);
        }
    }
}
