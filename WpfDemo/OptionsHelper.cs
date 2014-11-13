// Copyright (c) 2014, https://github.com/rhcad/touchvg/vgwpf

using Newtonsoft.Json.Linq;
using touchvg.core;
using touchvg.view;

namespace WpfDemo
{
    public class OptionsHelper
    {
        public static JObject GetOptions(WPFViewHelper helper)
        {
            OptionCallback c = new OptionCallback();
            helper.GetOptions(c);
            return c.Options;
        }

        public static void SetOptions(WPFViewHelper helper, JObject dict)
        {
            foreach (var item in dict)
            {
                helper.SetOption(item.Key, item.Value.ToString());
            }
        }

        private class OptionCallback : MgOptionCallback
        {
            public JObject Options = new JObject();

            public override void onGetOptionBool(string name, bool value)
            {
                Options[name] = value;
            }
            public override void onGetOptionInt(string name, int value)
            {
                Options[name] = value;
            }
            public override void onGetOptionFloat(string name, float value)
            {
                Options[name] = value;
            }
        }
    }
}
