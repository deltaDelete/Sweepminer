using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweepminer.Models;
public class Difficulty {
    public string Name { get; set; }
    public int Rows { get; set; }
    public int Columns { get; set; }
    public int Mines { get; set; }
}
