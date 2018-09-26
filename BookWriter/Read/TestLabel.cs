using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MyBook
{
    public class TestLabel : Label
    {
        private ControlTemplate _leftPageTemplate;
        public TestLabel()
            : base()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _leftPageTemplate = Template;
            Grid o = _leftPageTemplate.FindName("GridTest", this) as Grid;

            System.Diagnostics.Debug.Assert(o != null);
        }
    }
}
