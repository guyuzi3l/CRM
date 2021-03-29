using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using static CRM.Models.WithdrawalModel;

namespace CRM.Utilities
{
    public class OptionUtilities
    {
        public ListItem[] GetGroupOptions()
        {
            GroupUtilities groupUtilities = new GroupUtilities();
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("Select Group", ""));
            foreach (var p in groupUtilities.GetGroupOptions())
            {
                items.Add(new ListItem(p.Value, p.Key.ToString()));
            }
            return items.ToArray();
        }

        public ListItem[] GetPSPOptions()
        {
            PspUtilities pspUtilities = new PspUtilities();
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("Select PSP", ""));
            foreach (var p in pspUtilities.GetPspOptions())
            {
                items.Add(new ListItem(p.Value, p.Key.ToString()));
            }
            return items.ToArray();
        }

        public ListItem[] GetDocumentStatusOptions()
        {
            List<ListItem> items = new List<ListItem>();
            items.Add(new ListItem("Select Status", ""));
            var status = Enum.GetValues(typeof(DocumentStatus)).Cast<DocumentStatus>().ToList();
            foreach (var p in status)
            {
                items.Add(new ListItem(p.ToString(), status.IndexOf(p).ToString()));
            }
            return items.ToArray();
        }
    }
}