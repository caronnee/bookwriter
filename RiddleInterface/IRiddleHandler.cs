using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;

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
      return c.Id;
    }
  }

  public delegate void OnSuccessAction(int id);

  public interface IRiddleHandler
  {
    // what should happen when riddle's aswer is recognized. It is common for every riddlr 
    OnSuccessAction onAnswer { get; set; }

    // name of the content handler
    String Name { get; }
    
    // list of the possible outcomes
    List<Outcome> Outcomes { get; set; }

    // setting assotiated with the content handler
    Control Settings { get; set; }

    // showable content
    Control Viewport { get; set; }

    // final page for displaying to reader
    Control DisplayPage { get; set; }

    // convert to read only control that can be showable
    void CreateReadOnly();

    // sets the page as not answered yet
    void ClearAnswer();

    // save to a file
    void Load(Stream stream);

    // save to a file
    void Save(Stream stream);

    // creates write module
    void Create();
  }
}
