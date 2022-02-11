using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace RiddleInterface
{
  public class Outcome
  {
    // description showable in creator
    public String Name { get; set; }

    // id in the book
    public int Id { get; set; }
  }
  
  public class OutcomeConverter : IValueConverter
  {
    List<Outcome> Outcomes { get; set; }

    // from data(id) to Outcome
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new Exception("Not implemented");
    }

    // back to the data
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      Outcome c = value as Outcome;
      // no outcome
      if (c == null)
        return -1;
      return c.Id;
    }
  }

  public interface IRiddleHandler
  {
    // base folder for this plugin data
    String BaseFolder { get; set; }

    // name of the content handler
    String Name { get; }
    
    // list of the possible outcomes
    List<Outcome> Outcomes { get; set; }

    // setting assotiated with the content handler ( rotation etc )
    Control Settings { get; set; }

    // showable content
    Control Viewport { get; set; }

    // final page for displaying to reader
    Control DisplayPage { get; set; }

    // sets the page as not answered yet
    void ClearAnswer();
    
    void Save(String name);

    // creates write module
    void Create();
  }
}
