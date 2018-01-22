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

namespace Diff {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        class Diff {
            //T max<T>(ref T a, ref T b) { return a > b ? a : b; }
            int max(int a, int b) { return a > b ? a : b; }

            const int maxLength = 1000;

            int [,] createMatrix(string a, string b) {
                int[,] theMatrix;
                theMatrix = new int[a.Length + 1, b.Length + 1];
                for (int i = 0; i <= a.Length; i++) {
                    theMatrix[i, 0] = 0;
                }
                for (int j = 0; j <= b.Length; j++) {
                    theMatrix[0, j] = 0;
                }
                for (int i = 0; i < a.Length; i++) {
                    for (int j = 0; j < b.Length; j++) {
                        if (a[i] == b[j]) {
                            theMatrix[i + 1, j + 1] = theMatrix[i, j] + 1;
                        }
                        else {
                            theMatrix[i + 1, j + 1] = max(theMatrix[i, j + 1], theMatrix[i + 1, j]);
                        }
                    }
                }
                /**/
                Console.Write("    ");
                for (int jj = 0; jj < b.Length; jj++) {
                    Console.Write(b[jj] + " ");
                }
                Console.WriteLine();
                for (int ii=0; ii<=a.Length; ii++) {
                    if(ii>0) Console.Write(a[ii-1]+ " "); else Console.Write("  ");
                    for (int jj=0; jj<=b.Length; jj++) {
                        Console.Write(theMatrix[ii,jj] + " ");
                    }
                    Console.WriteLine();
                }/**/
                return theMatrix;
            }

            void reverseStringBuilder(StringBuilder sb) {
                int i = 0;
                int j = sb.Length-1;
                while(i<j) {
                    char t = sb[i];
                    sb[i] = sb[j];
                    sb[j] = t;
                    i++;
                    j--;
                }
            }

            string createDiffString(string a, string b, int [,] theMatrix) {
                int i = a.Length;
                int j = b.Length;
                StringBuilder diffString=new StringBuilder(i + j - theMatrix[i, j]);
                while (i > 0 || j > 0) {
                    if (i > 0 && j > 0 && a[i - 1] == b[j - 1]) {
                        diffString.Append( a[i - 1]);
                        diffString.Append(' ');
                        i--;
                        j--;
                    }
                    else if (i > 0 && (j == 0 || theMatrix[i, j - 1] < theMatrix[i - 1, j])) {
                        diffString.Append(a[i - 1]);
                        diffString.Append('-');
                        i--;
                    }
                    else if (j > 0 && (i == 0 || theMatrix[i, j - 1] >= theMatrix[i - 1, j])) {
                        diffString.Append(b[j-1]);
                        diffString.Append('+');
                        j--;
                    }
                }

                reverseStringBuilder(diffString);
                return diffString.ToString();
            }

            public Diff(string a, string b, out string c) {
                if (a.Length == 0 || b.Length == 0 || a.Length > maxLength || b.Length > maxLength) {
                    c = "";
                    return;
                }

                int [, ] theMatrix=createMatrix(a, b);
                c = createDiffString(a, b, theMatrix);
            }
        }


        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            string c;
            //new Diff("XMJYAUZ", "MZJAWXU", out c);
            //new Diff("Her er min lille tekst", "her er min anden lille tekst", out c);

            new Diff("abc", "acb", out c);

            //new Diff("computer", "houseboat", out c);

            //  http://www.flounder.com/csharp_color_table.htm
            comparedText.Inlines.Clear();
            for(int i=0; i<c.Length; i++) {
                switch(c[i]) {
                    case ' ':
                        comparedText.Inlines.Add(c[++i]+"");
                        break;
                    case '+': comparedText.Inlines.Add(new Run(c[++i]+"") { Background = Brushes.PaleGreen }); break;
                    case '-': comparedText.Inlines.Add(new Run(c[++i] + "") { Background = Brushes.LightSalmon }); break;
                    default:break;
                }
            }


            //comparedText.Text = c;
        }
    }
}
