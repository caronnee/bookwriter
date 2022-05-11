using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.BookContent
{
  public class LocationPart
  {
    public String Title { get; set; }
    public String Content { get; set; }
  }
  public class MapPart
  {
    public String Name { get; set; }
    public String MapSource { get; set; }
  }
  public class WorldDescription
  {
    public String Name { get; set; }
    public List<LocationPart> Info { get; set; }
    public List<MapPart> Maps { get; set; }
  }
}
