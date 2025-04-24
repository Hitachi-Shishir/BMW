using System.Web.UI;

public class DummyTemplate : ITemplate
{
    public void InstantiateIn(Control container)
    {
        container.Controls.Add(new LiteralControl("&nbsp;"));
    }

}